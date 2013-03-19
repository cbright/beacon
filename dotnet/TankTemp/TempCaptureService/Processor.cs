using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;

namespace TempCaptureService
{
    public class Processor
    {
        private readonly string _url;
        private DateTime _lastUpdate;

        public Processor(string url)
        {
            _url = url;
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(Processor));

        public void ProcessRawMeasurement(object sender, string rawData)
        {
            Log.Debug("Processing UDP message.");
            var measurement = JsonConvert.DeserializeObject<TempatureMeasurement>(rawData);

            if(DateTime.UtcNow.Subtract(_lastUpdate) < new TimeSpan(0,0,1,0))
                return;

            _lastUpdate = DateTime.UtcNow;

            Task.Factory.StartNew(() =>
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var id = Int64.Parse(measurement.Id, NumberStyles.HexNumber);
                    var json = JsonConvert.SerializeObject(new { Id = id, measurement.Tempature, measurement.ObservedAt });
                    client.UploadString(string.Format(_url,id), json);
                    Log.InfoFormat("Tank tempature observed at {0} UTC {1} Local is {2:F}c",
                        measurement.ObservedAt, measurement.ObservedAt.ToLocalTime(), measurement.Tempature);
                }
            });  
        }
    }
}