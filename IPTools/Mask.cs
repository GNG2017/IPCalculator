/* MIT License
 *
 * Copyright (c) 2020 Glöckl Nimród
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace IPTools
{
    public class Mask : IPAddress
    {
        private static Regex MaskRegex = new Regex(@"^1+0+$");
        public Mask() { }
        public Mask(string IP) : base(IP)
        {
            if (!MaskRegex.IsMatch(Binary))
                throw new InvalidDataException("Invalid mask!");
        }
        public Mask(int num) : base(num)
        {
            if (!MaskRegex.IsMatch(Binary))
                throw new InvalidDataException("Invalid mask!");
        }
        public Mask(int first, int second, int third, int fourth) : base(first, second, third, fourth) { }

        public int Prefix => Binary.Count(x => x == '1');

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

        public List<Network> GetNetworksForSubnet(NetworkAddress NetAddress, Mask SubnetMask)
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
            List<Network> list = new List<Network>();
            for (int i = 0; i < sub; i++)
            {
                var subNetwork = (mainNetwork + Convert.ToString(i, 2).PadLeft(subLenght, '0')).PadRight(32, '0');
                list.Add(new Network(new NetworkAddress(Convert.ToInt32(subNetwork, 2)), SubnetMask));
            }
            return list;
        }
    }
}
