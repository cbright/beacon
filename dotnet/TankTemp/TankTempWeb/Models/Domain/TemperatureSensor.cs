namespace TankTempWeb.Models.Domain
{
    public class TemperatureSensor : Sensor
    {
        public TemperatureSensor()
        {
            Observations = new Iesi.Collections.Generic.HashedSet<TemperatureObservation>();
        }

        public Iesi.Collections.Generic.ISet<TemperatureObservation> Observations { get; set; }

        public void RecordObservation(TemperatureObservation reading)
        {
            reading.Sensor = this;
            Observations.Add(reading);
        }
    }
}