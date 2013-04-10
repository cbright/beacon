using System;
using TechTalk.SpecFlow;
using TankTempWeb.Models.Api.v1;
using NUnit.Framework;
using System.Net;
using System.Diagnostics;
using System.Web;
using Newtonsoft.Json;

namespace TankTempTests.Acceptance
{
    [Binding]
    public class CreateSensorSteps
    {
        private TankTempWeb.Models.Api.v1.CreateSensor _sensor = new TankTempWeb.Models.Api.v1.CreateSensor();
        private HttpWebResponse _response;

        [Given(@"I have a sensor as")]
        public void GivenIHaveASensorAs(Table table)
        {
            _sensor = new TankTempWeb.Models.Api.v1.CreateSensor
            {
                NetworkId = Convert.ToInt32(table.Rows[0][0]),
                SerialNumber = table.Rows[0][1],
                Description = table.Rows[0][2],
                Unit = table.Rows[0][3],
                Type = table.Rows[0][4],
            };
        }
        
        [When(@"I POST to ""(.*)""")]
        public void WhenIPOSTToApiVSensor(string uri)
        {
            var request = WebRequest.CreateHttp(string.Concat("http://127.0.0.1:81/", uri));
            request.ContentType = "application/json";
            request.Method = "POST";
            using (var requestStream = request.GetRequestStream()){
                var jsonSensor = JsonConvert.SerializeObject(_sensor);
                var senorBytes = System.Text.Encoding.UTF8.GetBytes(jsonSensor);
                requestStream.Write(senorBytes, 0, senorBytes.Length);
            }

            try
            {

                _response = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException webex)
            {
                _response = webex.Response as HttpWebResponse;
            }

        }
        
        [Then(@"the result HTTP Status code should be ""(.*)""")]
        public void ThenTheResultHTTPStatusCodeShouldBe(int p0)
        {
            Assert.AreEqual(p0,(int)_response.StatusCode);
        }
    }
}
