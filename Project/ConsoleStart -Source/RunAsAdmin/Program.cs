using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        string gameName = "DNS_Changer";
        string gameExe = gameName + ".exe";

        // Search for the game executable in the current directory and its subdirectories
        string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), gameExe, SearchOption.AllDirectories);

        if (files.Length > 0)
        {
            string gamePath = files[0];

            // Check if the game is already running
            if (IsProcessRunning(gameName))
            {
                Console.WriteLine("Game is already running.");
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = gamePath,
                Verb = "runas" // Run as administrator
            };

            try
            {
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Game executable not found.");
        }
    }

    static bool IsProcessRunning(string processName)
    {
        Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(processName));
        return processes.Length > 0;
    }
}