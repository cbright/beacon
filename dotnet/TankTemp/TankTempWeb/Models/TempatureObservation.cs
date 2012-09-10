using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TankTempWeb.Models
{
    public class TempatureObservation
    {
        public int Id { get; set; }

        public float Tempature { get; set; }

        public DateTime ObservedAt { get; set; }
    }
}