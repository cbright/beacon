using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SignalR;
using TankTempWeb.Models;

namespace TankTempWeb.Controllers
{
    public class TempatureObservationsController : ApiController
    {
        public HttpResponseMessage PostTempatureObservation(int id,TempatureObservation observation)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TempatureObservationHub>();
            context.Clients.updateCurrentTempature(observation);

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
