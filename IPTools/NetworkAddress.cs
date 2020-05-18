using System;

namespace IPTools
{
    public class NetworkAddress : IPAddress
    {
        public NetworkAddress() { }
        public NetworkAddress(int num) : base(num) { }
        public NetworkAddress(string IP) : base(IP) { }

        public HostAddress GetFistUseableHostIP(Mask smask)
        {
            var network = Binary;
            var mask = smask.Binary;
            var host = "";
            for (int i = 0; i < 31; i++)
            {
                if (mask[i] == '1')
                    host += network[i];
                else
                    host += '0';
            }
            host += '1';
            return new HostAddress(Convert.ToInt32(host, 2));
        }

        public HostAddress GetLastUseableHostIP(Mask smask)
        {
            var network = Binary;
            var mask = smask.Binary;
            var host = "";
            for (int i = 0; i < 31; i++)
            {
                if (mask[i] == '1')
                    host += network[i];
                else
                    host += '1';
            }
            host += '0';
            return new HostAddress(Convert.ToInt32(host, 2));
        }

        public IPAddress GetBroadcastAddress(Mask smask)
        {
            var network = Binary;
            var mask = smask.Binary;
            var host = "";
            for (int i = 0; i < 32; i++)
            {
                if (mask[i] == '1')
                    host += network[i];
                else
                    host += '1';
            }
            return new IPAddress(Convert.ToInt32(host, 2));
        }

        public NetworkAddress GetNextNetworkAddress(Mask smask)
        {
            var network = Binary;
            var mask = smask.Binary;
            var nextNetwork = "";
            for (int i = 0; i < 30; i++)
            {
                if (mask[i] == '0') break;
                nextNetwork += network[i];
            }
            var nextNet = Convert.ToString(Convert.ToInt32(nextNetwork, 2) + 1, 2).PadRight(32, '0');
            return new NetworkAddress(Convert.ToInt32(nextNet, 2));
        }
    }
}
