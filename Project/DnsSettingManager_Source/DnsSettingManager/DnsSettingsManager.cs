using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Principal;

namespace DnsSettingManager
{
    public class DnsSettingsManager
    {
        public bool SetDnsSettings(string[] dnsServers)
        {
            try
            {
                if (!IsRunAsAdmin())
                {
                    // Restart the application as administrator
                    RunAsAdmin();
                }

                //string script = $"$interfaceAlias = '{networkAdapterName}'; Set-DnsClientServerAddress -InterfaceAlias $interfaceAlias -ServerAddresses {string.Join(",", dnsServers)}";
                //string command = $"powershell -ExecutionPolicy Bypass -Command \"{script}\"";


                NetworkInterface activeAdapter = GetActiveNetworkAdapter();
                if (activeAdapter == null)
                {
                    Console.WriteLine("No active network adapter found.");
                    return false;
                }

                string adapterName = activeAdapter.Name;
                string script = $"$interfaceAlias = '{adapterName}'; Set-DnsClientServerAddress -InterfaceAlias $interfaceAlias -ServerAddresses {string.Join(",", dnsServers)}";
                string command = $"powershell -ExecutionPolicy Bypass -Command \"{script}\"";



                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe")
                {
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process process = Process.Start(psi);
                process.StandardInput.WriteLine(command);
                process.StandardInput.WriteLine("exit");
                process.WaitForExit();

                Console.WriteLine("DNS settings updated successfully.");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);

                return false;
            }
        }

        public bool ResetDnsSettings()
        {
            try
            {
                if (!IsRunAsAdmin())
                {
                    // Restart the application as administrator
                    RunAsAdmin();
                }

                //ProcessStartInfo psi = new ProcessStartInfo("netsh", $"interface ip set dns name=\"{networkAdapterName}\" source=dhcp");

                NetworkInterface activeAdapter = GetActiveNetworkAdapter();
                if (activeAdapter == null)
                {
                    Console.WriteLine("No active network adapter found.");
                    return false;
                }

                string adapterName = activeAdapter.Name;
                ProcessStartInfo psi = new ProcessStartInfo("netsh", $"interface ip set dns name=\"{adapterName}\" source=dhcp");
                psi.RedirectStandardOutput = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;

                Process process = new Process();
                process.StartInfo = psi;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine("Failed to set DNS server to Obtain DNS server address automatically");

                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);

                return false;
            }
        }
        public bool IsDNSSet(out string[] dnsServers)
        {
            dnsServers = null;

            try
            {
                NetworkInterface activeAdapter = GetActiveNetworkAdapter();
                if (activeAdapter == null)
                {
                    Console.WriteLine("No active network adapter found.");
                    return false;
                }

                string adapterName = activeAdapter.Name;
                ProcessStartInfo psi = new ProcessStartInfo("netsh", $"interface ip show dns name=\"{adapterName}\"");
                psi.RedirectStandardOutput = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;

                Process process = new Process();
                process.StartInfo = psi;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // Check the output for DNS server information
                bool dnsSet = output.Contains("DNS servers configured through DHCP");

                if (dnsSet)
                {
                    // Extract the DNS server IP addresses from the output
                    List<string> serverList = new List<string>();

                    string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        if (line.Trim().StartsWith("Server:", StringComparison.OrdinalIgnoreCase))
                        {
                            string[] parts = line.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length > 1)
                            {
                                serverList.Add(parts[1].Trim());
                            }
                        }
                    }

                    dnsServers = serverList.ToArray();
                }

                return dnsSet;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }
        public List<string> GetAdaptors()
        {
            try
            {
                if (!IsRunAsAdmin())
                {
                    // Restart the application as administrator
                    RunAsAdmin();
                }

                var listAdaptors = new List<string>();
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionID != NULL");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string caption = queryObj["NetConnectionID"] as string;
                    listAdaptors.Add(caption);
                }

                return listAdaptors;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);

                return null;
            }
        }

        private NetworkInterface GetActiveNetworkAdapter()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                if (adapter.OperationalStatus == OperationalStatus.Up && adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    return adapter;
                }
            }

            return null;
        }

        private bool IsRunAsAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void RunAsAdmin()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
            startInfo.Verb = "runas"; // Request UAC elevation

            try
            {
                Process.Start(startInfo);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                // User cancelled the UAC prompt
                Console.WriteLine("This application requires administrator privileges to run. Please run it as an administrator.");
            }
        }
    }



}

