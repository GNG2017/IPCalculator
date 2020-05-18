using IPTools;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace IPCalculator
{
    class Program
    {
        static readonly Regex OneOctetIP = new Regex(@"^\d{1,3}$");
        static readonly Regex HalfIP = new Regex(@"^\d{1,3}\.\d{1,3}$");
        static void Main()
        {
            Console.Title = "IPCalculator - © 2020 Glöckl Nimród";
            using var writer = new StreamWriter("output.txt");
            writer.WriteLine("-BEGINING OF FILE-");
            while (true)
            {
                Console.WriteLine("Options: ");
                Console.WriteLine("1: |    Host + Subnet = Network         |     \"A hálózati cím meghatározása\"       - 9.1.3.6/9.1.3.13");
                Console.WriteLine("2: | Network + Subnet = Num of hosts    |  \"Az állomások számának meghatározása\"   - 9.1.3.7/9.1.3.14");
                Console.WriteLine("3: | Network + Subnet = Network details | \"Az érvényes állomáscímek meghatározása\" - 9.1.3.8/9.1.3.15");
                Console.WriteLine("4: | Network + mask + num of hosts = More subnets |");
                Console.WriteLine("5: Exit");
                NetworkAddress network;
                Mask mask;
                try
                {
                    switch (Console.ReadLine())
                    {
                        case "1":
                            var host = GetHostAddress();
                            if (host == null)
                                break;
                            mask = GetSubnetMask();
                            if (mask == null)
                                break;
                            NetworkAddress networkAddress;
                            try
                            {
                                networkAddress = host.GetNetworkAddress(mask);
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error: " + ex.Message);
                                Console.ResetColor();
                                break;
                            }
                            Console.WriteLine("Network address:");
                            Console.WriteLine(networkAddress.ToBinaryString());
                            Console.WriteLine(networkAddress);
                            Console.WriteLine();
                            writer.WriteLine("A hálózati cím meghatározása");
                            writer.WriteLine();
                            writer.WriteLine("Host cím:");
                            writer.WriteLine($" {host.ToBinaryString()}");
                            writer.WriteLine($" {host}");
                            writer.WriteLine();
                            writer.WriteLine("Alhálózati maszk:");
                            writer.WriteLine($" {mask.ToBinaryString()}");
                            writer.WriteLine($" {mask}");
                            writer.WriteLine();
                            writer.WriteLine(" Hálózati cím:");
                            writer.WriteLine($"  {networkAddress.ToBinaryString()}");
                            writer.WriteLine($"  {networkAddress}");
                            writer.WriteLine();
                            writer.WriteLine();
                            break;
                        case "2":
                            network = GetNetworkAddress();
                            if (network == null)
                                break;
                            mask = GetSubnetMask();
                            if (mask == null)
                                break;
                            int numOfValidHosts;
                            try
                            {
                                numOfValidHosts = mask.GetNumOfValidHosts();
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error: " + ex.Message);
                                Console.ResetColor();
                                break;
                            }
                            Console.WriteLine($"Number of Valid Hosts: {numOfValidHosts}");
                            writer.WriteLine("Az állomások számának meghatározása");
                            writer.WriteLine();
                            writer.WriteLine("Hálózati cím:");
                            writer.WriteLine($" {network.ToBinaryString()}");
                            writer.WriteLine($" {network}");
                            writer.WriteLine();
                            writer.WriteLine("Alhálózati maszk:");
                            writer.WriteLine($" {mask.ToBinaryString()}");
                            writer.WriteLine($" {mask}");
                            writer.WriteLine();
                            writer.WriteLine(" Az állomások száma:");
                            writer.WriteLine($"  {Convert.ToString(numOfValidHosts, 2).PadLeft(8, '0')}");
                            writer.WriteLine($"  {numOfValidHosts}");
                            writer.WriteLine();
                            writer.WriteLine();
                            break;
                        case "3":
                            network = GetNetworkAddress();
                            if (network == null)
                                break;
                            mask = GetSubnetMask();
                            if (mask == null)
                                break;
                            HostAddress first;
                            HostAddress last;
                            IPAddress broadcast;
                            NetworkAddress next;
                            try
                            {
                                first = network.GetFistUseableHostIP(mask);
                                last = network.GetLastUseableHostIP(mask);
                                broadcast = network.GetBroadcastAddress(mask);
                                next = network.GetNextNetworkAddress(mask);
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error: " + ex.Message);
                                Console.ResetColor();
                                break;
                            }
                            Console.WriteLine("Első használható host: " + first);
                            Console.WriteLine("Utolsó használható host: " + last);
                            Console.WriteLine("Szórási cím: " + broadcast);
                            Console.WriteLine("Következő hálózat címe: " + next);
                            writer.WriteLine("Az érvényes állomáscímek meghatározása");
                            writer.WriteLine();
                            writer.WriteLine("Hálózati cím:");
                            writer.WriteLine($" {network.ToBinaryString()}");
                            writer.WriteLine($" {network}");
                            writer.WriteLine();
                            writer.WriteLine("Alhálózati maszk:");
                            writer.WriteLine($" {mask.ToBinaryString()}");
                            writer.WriteLine($" {mask}");
                            writer.WriteLine();
                            writer.WriteLine(" Első kiosztható cím:");
                            writer.WriteLine($"  {first.ToBinaryString()}");
                            writer.WriteLine($"  {first}");
                            writer.WriteLine();
                            writer.WriteLine(" Utolsó kiosztható cím:");
                            writer.WriteLine($"  {last.ToBinaryString()}");
                            writer.WriteLine($"  {last}");
                            writer.WriteLine();
                            writer.WriteLine(" Szórási cím:");
                            writer.WriteLine($"  {broadcast.ToBinaryString()}");
                            writer.WriteLine($"  {broadcast}");
                            writer.WriteLine();
                            writer.WriteLine(" Következő hálózati cím:");
                            writer.WriteLine($"  {next.ToBinaryString()}");
                            writer.WriteLine($"  {next}");
                            writer.WriteLine();
                            writer.WriteLine();
                            break;
                        case "4":
                            var numOfHosts = GetNumOfHosts();
                            if (numOfHosts == -1) break;
                            network = GetNetworkAddress();
                            if (network == null) break;
                            mask = GetSubnetMask();
                            if (mask == null) break;
                            var submask = mask.GetSubnetMaskForNumOfHosts(numOfHosts);
                            Console.WriteLine("Subnet mask: " + submask);
                            var networks = mask.GetNetworksForSubnet(network, submask);
                            for (int i = 0; i < networks.Count; i++)
                            {
                                Console.WriteLine($"#{i + 1} network:");
                                Console.WriteLine("Network address: " + networks[i]);
                                Console.WriteLine("First host address: " + networks[i].GetFistUseableHostIP(submask));
                                Console.WriteLine("Last host address: " + networks[i].GetLastUseableHostIP(submask));
                                Console.WriteLine("Broadcast address: " + networks[i].GetBroadcastAddress(submask));
                            }
                            break;
                        case "5":
                            Console.WriteLine("Are you sure?[y/N]");
                            if (Console.ReadLine().ToLower() != "y") break;
                            writer.WriteLine("-END OF FILE-");
                            writer.Flush();
                            writer.Close();
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Unknown option!");
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("An unknown error occured!");
                    Console.WriteLine("Error: " + ex.Message);
                    Console.WriteLine(ex);
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }
        }

        static int GetNumOfHosts()
        {
            while (true)
            {
                Console.Write("Number of hosts: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int result))
                    return result;
                else if (input.ToLower() == "c")
                    return -1;
                else
                    Console.WriteLine("Invalid number!");
            }
        }

        static NetworkAddress GetNetworkAddress()
        {
            try
            {
                Console.Write("Network Address[192.168.]: ");
                var input = Console.ReadLine();
                if (input.ToLower().Contains('c'))
                    return null;
                if (HalfIP.IsMatch(input))
                    return new NetworkAddress("192.168." + input);
                return new NetworkAddress(input);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + ex.Message);
                Console.ResetColor();
                return null;
            }
        }

        static Mask GetSubnetMask()
        {
            try
            {
                Console.Write("Subnet Mask[255.255.255.|255.255.]: ");
                var input = Console.ReadLine();
                if (input.ToLower().Contains('c'))
                    return null;
                if (HalfIP.IsMatch(input))
                    return new Mask("255.255." + input);
                else if (OneOctetIP.IsMatch(input))
                    return new Mask(255, 255, 255, int.Parse(input));
                return new Mask(input);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + ex.Message);
                Console.ResetColor();
                return null;
            }
        }

        static HostAddress GetHostAddress()
        {
            try
            {
                Console.Write("Host address[192.168.]: ");
                var input = Console.ReadLine();
                if (input.ToLower().Contains('c'))
                    return null;
                if (HalfIP.IsMatch(input))
                    return new HostAddress("192.168." + input);
                else
                    return new HostAddress(input);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + ex.Message);
                Console.ResetColor();
                return null;
            }
        }
    }
}
