using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TankTempWeb.Models.Api.v1
{
    [DataContract]
    [KnownType(typeof(TemperatureObservation))]
    public class Observation
    {
        [DataMember,Required]
        public int SensorId { get; set; }

        [DataMember,Required]
        public DateTime ObservedAt { get; set; }
    }

    [DataContract]
    public class TemperatureObservation : Observation
    {
        [DataMember,Required]
        public float Value { get; set; }
    }
}