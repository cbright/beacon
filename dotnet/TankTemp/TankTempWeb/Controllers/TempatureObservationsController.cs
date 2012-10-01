using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SignalR;
using TankTempWeb.Data;
using TankTempWeb.Models;

namespace TankTempWeb.Controllers
{
    public class TempatureObservationsController : ApiController
    {
        private readonly IConnectionManager _connectionManager;
        private readonly IRepository<TempatureSensor> _sensors;
        private readonly IRepository<TempatureObservation> _observations;

        public TempatureObservationsController(IConnectionManager connectionManager,
            IRepository<TempatureSensor> sensors,
            IRepository<TempatureObservation> observations)
        {
            _connectionManager = connectionManager;
            _sensors = sensors;
            _observations = observations;
        }

        public HttpResponseMessage PostTempatureObservation(int id,TempatureObservation observation)
        {
            var sensor = _sensors.Get(id);
            if(sensor == null){
                return new HttpResponseMessage(HttpStatusCode.NotFound){ReasonPhrase =  string.Format("Sensor Id {0} not found.",id)};
            }

            //update attached clients
            var context = _connectionManager.GetHubContext<TempatureObservationHub>();
            context.Clients.updateCurrentTempature(observation);

            observation.Sensor = sensor;
            _observations.Save(observation);

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
