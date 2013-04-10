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
using TemperatureObservation = TankTempWeb.Models.Api.v1.TemperatureObservation;

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


        public TemperatureObservation GetTempatureObservation(int id)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TempatureObservationHub>();
            context.Clients.updateTest(DateTime.UtcNow);

            return new TemperatureObservation();
        }
    }
}
