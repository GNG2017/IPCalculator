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
    }
}
