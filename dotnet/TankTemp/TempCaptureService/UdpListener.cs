using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using log4net;

namespace TempCaptureService
{
    public class UdpListener
    {
        private readonly UdpClient _client;
        private const int Port = 3968;
        private bool _isStarted;
        private Thread _listenerThread;
        private bool _stop;
        public event Action<object,string> MeasurementTaken;
        private static readonly ILog Log = LogManager.GetLogger(typeof(UdpListener));

        public void InvokeMeasurementTaken(string arg2)
        {
            var handler = MeasurementTaken;
            if (handler != null) handler(this, arg2);
        }

        public UdpListener(Action<object,string> measurementObservedCallBack)
        {
            MeasurementTaken += measurementObservedCallBack;
            _client = new UdpClient();
        }

        public void Start()
        {
            _client.EnableBroadcast = true;
            _listenerThread = new Thread(Server);
            _listenerThread.Start();
            _isStarted = true;
        }

        public void Stop()
        {
            if (!_isStarted) return;
            
            _stop = true;
            _listenerThread.Join();
            _stop = false;
            _client.Close();
            _isStarted = false;
        }

        private void Server()
        {
            try
            {
                byte[] data;
                var endpoint = new IPEndPoint(IPAddress.Any, Port);
                _client.Client.Bind(endpoint);
                while (!_stop)
                {

                    data = _client.Receive(ref endpoint);

                    var rawMeasurement = Encoding.ASCII.GetString(data, 0, data.Length);
                    Log.DebugFormat("Measurement received from ardiuno (IP: {0}, Port:{1}): {2}", rawMeasurement,
                                   endpoint.Address, endpoint.Port);
                    InvokeMeasurementTaken(rawMeasurement);
                }
            }catch(Exception ex)
            {
                Log.Fatal("Error listening for UDP packets.",ex);
            }
        }

    }

}