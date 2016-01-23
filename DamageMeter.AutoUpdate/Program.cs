﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace DamageMeter.AutoUpdate
{
    public class Program
    {
        private static void Main()
        {
            bool aIsNewInstance, isUpdating;
            var _unique = new Mutex(true, "ShinraMeter", out aIsNewInstance);
            while (!aIsNewInstance)
            {
                Console.WriteLine("Sleep");
                Thread.Sleep(1000);
                _unique = new Mutex(true, "ShinraMeter", out aIsNewInstance);
            }
            Thread.Sleep(1000);
            var uniqueUpdating = new Mutex(true, "ShinraMeterUpdating", out isUpdating);

            UpdateManager.DestroyRelease();
            CountError(0);
            var source = Directory.GetDirectories(UpdateManager.ExecutableDirectory + @"\..\release\")[0];
            UpdateManager.Copy(source, UpdateManager.ExecutableDirectory + @"\..\..\");
            Console.WriteLine("New version installed");
            Process.Start(UpdateManager.ExecutableDirectory + @"\..\..\ShinraMeter.exe");
            Environment.Exit(0);
        }

        private static void CountError(int numberTry)
        {
            if (numberTry > 2)
            {
                Console.WriteLine("Error");
                return;
            }
            try
            {
                if (Count()) return;
                numberTry++;
                CountError(numberTry);
            }
            catch
            {
                numberTry++;
                CountError(numberTry);
            }
        }

        private static bool Count()
        {
            using (var client = new HttpClient())
            {
                var response =
                    client.GetAsync(
                        new Uri("http://diclah.com/~yukikoo/counter/counter.php?version=" + UpdateManager.Version))
                        .Result;
                return response.IsSuccessStatusCode;
            }
        }
    }
}