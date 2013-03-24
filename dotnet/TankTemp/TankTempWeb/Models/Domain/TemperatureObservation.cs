using System;

namespace TankTempWeb.Models.Domain
{
    public class TemperatureObservation 
    {
        public int Id { get; set; }

        public float Value { get; set; }

        public DateTime ObservedAt { get; set; }

        public TemperatureSensor Sensor { get; set; }
    }
}