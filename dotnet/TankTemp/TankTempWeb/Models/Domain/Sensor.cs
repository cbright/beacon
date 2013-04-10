using System.Collections.Generic;

namespace TankTempWeb.Models.Domain
{
    public abstract class Sensor
    {
        public int Id { get; set; }

        public string SerialNumber { get; set; }

        public string Description { get; set; }

        public string Unit { get; set; }

        public Network Network { get; set; }
    }
}