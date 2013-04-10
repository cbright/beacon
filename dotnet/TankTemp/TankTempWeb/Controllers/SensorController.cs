using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using SignalR;
using TankTempWeb.Data;
using TankApi = TankTempWeb.Models.Api.v1;
using DomainModel = TankTempWeb.Models.Domain;
using TankTempWeb.Models.Domain;


namespace TankTempWeb.Controllers
{
    public class SensorController : ApiController
    {
        //private static log4net.ILog _log = log4net.LogManager.GetLogger(typeof(SensorController).DeclaringType);
        private readonly ISensorRepository _sensors;
        private readonly IRepository<Network> _networks;

        public SensorController(ISensorRepository sensors,IRepository<Network> networks)
        {
            _sensors = sensors;
            _networks = networks;
        }

        public TankApi.Sensor Get(int id)
        {
            var sensor = _sensors.Get(id);
            if (sensor == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = string.Format("Sensor {0} not found.",id) });

            var apiSensor = new TankTempWeb.Models.Api.v1.Sensor()
            {
                Description = sensor.Description,
                Id = sensor.Id,
                SerialNumber = sensor.SerialNumber,
                Unit = sensor.Unit,
                Type = sensor.GetType().Name
            };

            return apiSensor;
        }

        public HttpResponseMessage Post(TankApi.CreateSensor sensor)
        {
            var network = _networks.Get(sensor.NetworkId);
            if(network == null){
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound) 
                    { ReasonPhrase = string.Format("Network {0} does not exist", sensor.NetworkId) });
            }

            Sensor newSensor = null;

            switch (sensor.Type){
                case "TemperatureSensor":
                    newSensor = new TemperatureSensor();
                    break;
                default:
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) 
                    { ReasonPhrase = string.Format("Unsupported Sensor type {0}", sensor.Type) });
            }

            newSensor.Description = sensor.Description;
            newSensor.SerialNumber = sensor.SerialNumber;
            newSensor.Unit = sensor.Unit;
            network.AddSensor(newSensor);

            _sensors.Save(newSensor);

            var createdSensor = new TankApi.Sensor()
            {
                Id = newSensor.Id,
                Description = newSensor.Description,
                Type = newSensor.GetType().Name,
                SerialNumber = newSensor.SerialNumber,
                Unit = newSensor.Unit
            };

            return Request.CreateResponse<TankApi.Sensor>(HttpStatusCode.Created, createdSensor);
        }
    }
}
