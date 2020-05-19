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
