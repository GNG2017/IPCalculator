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
using System.Linq;
using System.Text.RegularExpressions;

namespace IPTools
{
    public class IPAddress
    {
        internal int IP = -1;

        internal string Binary => Convert.ToString(IP, 2).PadLeft(32, '0');

        public int First => Convert.ToInt32(FirstBinary, 2);
        public int Second => Convert.ToInt32(SecondBinary, 2);
        public int Third => Convert.ToInt32(ThirdBinary, 2);
        public int Fourth => Convert.ToInt32(FourthBinary, 2);

        public string FirstBinary => Binary.Substring(0, 8);
        public string SecondBinary => Binary.Substring(8, 8);
        public string ThirdBinary => Binary.Substring(16, 8);
        public string FourthBinary => Binary.Substring(24, 8);

        public static int ToWholeNumber(int first, int second, int third, int fourth)
        {
            if (first > 255 || first < 0)
                throw new ArgumentOutOfRangeException(nameof(first), first, "The first octet must be between 0 and 255!");
            if (second > 255 || second < 0)
                throw new ArgumentOutOfRangeException(nameof(second), second, "The second octet must be between 0 and 255!");
            if (third > 255 || third < 0)
                throw new ArgumentOutOfRangeException(nameof(third), third, "The third octet must be between 0 and 255!");
            if (fourth > 255 || fourth < 0)
                throw new ArgumentOutOfRangeException(nameof(fourth), fourth, "The fourth octet must be between 0 and 255!");
            int num = 0;
            num += fourth;
            num += third << 8;
            num += second << 16;
            num += first << 24;
            return num;
        }

        public IPAddress(int num) => IP = num;

        internal IPAddress() { }
        public static IPAddress FromBinaryString(string binary) => new IPAddress() { IP = Convert.ToInt32(binary, 2) };

        public IPAddress(int first, int second, int third, int fourth) => IP = ToWholeNumber(first, second, third, fourth);

        public static Regex IPRegex = new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");

        public IPAddress(string ip)
        {
            if (!IPRegex.IsMatch(ip))
                throw new Exception("Invalid IP address!");
            var split = ip.Split('.');
            int[] numSplit;
            try
            {
                numSplit = split.Select(x => int.Parse(x)).ToArray();
            }
            catch (FormatException ex)
            {
                throw new FormatException("One of the octets was in the correct format!", ex);
            }
            IP = ToWholeNumber(numSplit[0], numSplit[1], numSplit[2], numSplit[3]);
        }

        public override string ToString() => $"{First}.{Second}.{Third}.{Fourth}";

        public string ToBinaryString() => $"{FirstBinary}.{SecondBinary}.{ThirdBinary}.{FourthBinary}";

        public EClassType GetClass()
        {
            if (IP >= 16777217) // 1.0.0.1
            {
                if (IP <= 2130706430) // 126.255.255.254
                    return EClassType.A;
                if (IP >= -2147418111) // 128.1.0.1
                {
                    if (IP <= -1073741826) // 192.255.255.254
                        return EClassType.B;
                    if (IP >= -1073741567) // 192.0.1.1
                    {
                        if (IP <= -536871170) // 223.255.254.254
                            return EClassType.C;
                        if (IP >= -536870912) // 224.0.0.0
                        {
                            if (IP <= -268435457) // 239.255.255.254
                                return EClassType.D;
                            if (IP >= -268435456) // 240.0.0.0
                            {
                                if (IP <= -16777218) // 254.255.255.254
                                    return EClassType.E;
                            }
                        }
                    }
                }
            }
            return EClassType.None;
        }

        public static bool operator ==(IPAddress address1, IPAddress address2)
        {
            if (address1 is null)
                return address2 is null;

            return address1.Binary.Equals(address2.Binary);
        }

        public static bool operator !=(IPAddress address1, IPAddress address2) => !(address1 == address2);
    }

    public enum EClassType
    {
        A, B, C, D, E, None = 255
    }
}
