using System;

namespace TankTempWeb.Models.Api.v1
{
    public class TempatureObservation
    {
        public float Value { get; set; }

        public DateTime ObservedAt { get; set; }

        public string SensorSerialNumber { get; set; }
    }
}