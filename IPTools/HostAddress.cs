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

        public Network GetNetwork(Mask mask) => new Network(GetNetworkAddress(mask), mask);
    }
}
