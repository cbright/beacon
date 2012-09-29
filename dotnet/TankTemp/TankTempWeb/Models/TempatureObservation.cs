using System;

namespace TankTempWeb.Models
{
    public class TempatureObservation
    {
        /// <summary>
        /// The id of the reading
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The temapture observed in Celsius.
        /// </summary>
        public float Tempature { get; set; }

        /// <summary>
        /// The date and time of the observation in UTC
        /// </summary>
        public DateTime ObservedAt { get; set; }

        /// <summary>
        /// Reference to the sensor that made the observation
        /// </summary>
        public TempatureSensor Sensor { get; set; }
    }
}