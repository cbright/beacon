using System;

namespace TankTempWeb.Models.Domain
{
    public class HourlySummary
    {
        public int Id { get; set; }

        public float Low { get; set; }

        public float High { get; set; }

        public float Median { get; set; }

        public float Mode { get; set; }

        public float StandardDeviation { get; set; }

        public int SensorId { get; set; }

        public DateTime HourOf { get; set; }
    }
}