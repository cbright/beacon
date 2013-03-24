using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SignalR;
using TankTempWeb.Data;
using TankTempWeb.Models;
using TankTempWeb.Models.Api.v1;
using TankTempWeb.Models.Domain;
using TempatureObservation = TankTempWeb.Models.Api.v1.TempatureObservation;

namespace TankTempWeb.Controllers
{
    public class TempatureObservationsController : ApiController
    {

        private static log4net.ILog _log = log4net.LogManager.GetLogger(typeof(TempatureObservationsController).DeclaringType);
        private readonly IConnectionManager _connectionManager;
        private readonly ISensorRepository _sensors;

        public TempatureObservationsController(IConnectionManager connectionManager,
            ISensorRepository sensors)
        {
            _connectionManager = connectionManager;
            _sensors = sensors;
        }

        public HttpResponseMessage PostTempatureObservation(int id,TempatureObservation observation)
        {
            var sensor = _sensors.GetBySerialNumber(observation.SensorSerialNumber);
            if(sensor == null){
                return new HttpResponseMessage(HttpStatusCode.NotFound){ReasonPhrase =  string.Format("Sensor Id {0} not found.",id)};
            }

            var tempatureSensor = sensor as TemperatureSensor;
            if(tempatureSensor == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest){ReasonPhrase = string.Format("Sensor({0}) {1} is not a tempature sensor.",sensor.Id,sensor.Description)};
            }

            

            //update attached clients
            var context = _connectionManager.GetHubContext<TempatureObservationHub>();
            context.Clients.updateCurrentTempature(observation);

            var reading = new Models.Domain.TemperatureObservation()
                              {
                                  ObservedAt = observation.ObservedAt,
                                  Value = observation.Value,
                              };

            tempatureSensor.RecordObservation(reading);
            _sensors.Save(tempatureSensor);

            return new HttpResponseMessage(HttpStatusCode.OK);   
        }

        public TempatureObservation GetTempatureObservation(int id)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TempatureObservationHub>();
            context.Clients.updateTest(DateTime.UtcNow);

            return new TempatureObservation();
        }
    }
}
