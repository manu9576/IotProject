namespace Storage.Models
{
    public interface IDtoSensor
    {
        public int SensorId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
    }
}
