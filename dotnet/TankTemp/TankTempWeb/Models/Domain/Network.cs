namespace TankTempWeb.Models.Domain
{
    public class Network
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Iesi.Collections.Generic.ISet<Sensor> Sensors { get; set; }
    }
}