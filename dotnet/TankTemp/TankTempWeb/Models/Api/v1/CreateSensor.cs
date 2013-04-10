using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace TankTempWeb.Models.Api.v1
{
    [DataContract]
    public class CreateSensor
    {
        [DataMember]
        public int NetworkId { get; set; }

        [DataMember]
        public string SerialNumber { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Unit { get; set; }

        [DataMember]
        public string Type { get; set; }
    }
}