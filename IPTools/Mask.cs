using System;
using System.Collections.Generic;

namespace IPTools
{
    public class Mask : IPAddress
    {
        public Mask() { }
        public Mask(string IP) : base(IP) { }
        public Mask(int num) : base(num) { }
        public Mask(int first, int second, int third, int fourth) : base(first, second, third, fourth) { }

        public int GetNumOfValidHosts()
        {
            var mask = Binary;
            var binaryNum = "0";
            for (int i = 30; i >= 0; i--)
            {
                if (mask[i] == '1') break;
                binaryNum = '1' + binaryNum;
            }
            return Convert.ToInt32(binaryNum, 2);
        }

        public Mask GetSubnetMaskForNumOfHosts(int hosts)
        {
            var submask = new string('0', Convert.ToString(hosts, 2).Length).PadLeft(32, '1');
            return new Mask(Convert.ToInt32(submask, 2));
        }

        public List<NetworkAddress> GetNetworksForSubnet(NetworkAddress NetAddress, Mask SubnetMask)
        {
            var mask = Binary;
            var subMask = SubnetMask.Binary;
            var network = NetAddress.Binary;
            var mainNetwork = "";
            for (int i = 0; i < 32; i++)
            {
                if (mask[i] == '1' && subMask[i] == '1')
                    mainNetwork += network[i];
                else break;
            }
            var subLenght = subMask.TrimEnd('0').Length - mask.TrimEnd('0').Length;
            var sub = Convert.ToInt32(new string('1', subLenght), 2);
            List<NetworkAddress> list = new List<NetworkAddress>();
            for (int i = 0; i < sub; i++)
            {
                var subNetwork = (mainNetwork + Convert.ToString(i, 2).PadLeft(subLenght,'0')).PadRight(32, '0');
                list.Add(new NetworkAddress(Convert.ToInt32(subNetwork, 2)));
            }
            return list;
        }
    }
}
