﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.IO;

namespace WebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        static void Main()
        {
            var host = new JobHost();

            try
            {
                host.Call(typeof(Functions).GetMethod("ScrapeAndStoreWOD"));
            }
            catch (InvalidOperationException error)
            {
                Program.WriteMessage(error.InnerException.Message);
            }
        }

        static void WriteMessage(string message)
        {
            Console.Error.Write(message);
        }
    }
}
