using System.Net;
using System.Net.Sockets;

namespace squad_games_2.Assets.Code.Tests.EditTests
{
    public class TestUtility
    {
        
        public static IPAddress GetIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach(var ip in host.AddressList)
            {
                if(ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            return null;
        }

    }
}