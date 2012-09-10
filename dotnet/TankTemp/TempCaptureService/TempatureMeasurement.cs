using System;

namespace TempCaptureService
{
    public class TempatureMeasurement
    {
        public TempatureMeasurement()
        {
            ObservedAt = DateTime.UtcNow;
        }

        public string Id { get; set; }

        public float Tempature { get; set; }

        public DateTime ObservedAt { get; set; }
    }
}