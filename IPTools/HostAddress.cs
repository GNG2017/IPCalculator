using System;

namespace IPTools
{
    public class HostAddress : IPAddress
    {
        public HostAddress() { }
        public HostAddress(int num) : base(num) { }
        public HostAddress(string IP) : base(IP) { }

        public NetworkAddress GetNetworkAddress(Mask smask)
        {
            var host = Binary;
            var mask = smask.Binary;
            var network = "";
            for (int i = 0; i < 32; i++)
            {
                if (mask[i] == '1')
                    network += host[i];
                else
                    network += '0';
            }
            return new NetworkAddress(Convert.ToInt32(network, 2));
        }
    }
}
