using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using log4net.Config;
using TempCaptureService;

namespace TempCaptureTestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            var p = new Processor(ConfigurationManager.AppSettings["postUrl"]);
            var server = new UdpListener(p.ProcessRawMeasurement);
            server.Start();
            Console.WriteLine("Press any key to shutdown.");
            Console.ReadKey();
            server.Stop();
        }
    }
}
