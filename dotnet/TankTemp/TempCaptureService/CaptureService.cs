using System.Configuration;
using System.ServiceProcess;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using Newtonsoft.Json;

namespace TempCaptureService
{
    public partial class CaptureService : ServiceBase
    {
        private readonly UdpListener _udpServer;
        private readonly Processor _processor;
        private static readonly ILog Log = LogManager.GetLogger(typeof (CaptureService));

        public CaptureService()
        {
            XmlConfigurator.Configure();
            InitializeComponent();
            _processor = new Processor(ConfigurationManager.AppSettings["postUrl"]);
            _udpServer = new UdpListener(_processor.ProcessRawMeasurement);
            Log.Info("Finished startup.");
        }

        protected override void OnStart(string[] args)
        {
            Log.Info("Starting UDP Listener.");
            _udpServer.Start();
            Log.Info("UDP listener started.");
        }

        protected override void OnStop()
        {
            Log.Info("Stopping UDP listner.");
            _udpServer.Stop();
            Log.Info("UPD listener stopped.");
        }


    }
}
