using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using Sensors.Weather;
using System.ComponentModel;

namespace Sensors.GrovePi
{
    internal class GrovePiDthTemperatureSensor : GrovePiSensor, IRefresher, ISensor
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public GrovePiDthTemperatureSensor(DhtSensor dhtSensor, string name, GrovePort port)
            : base(name, "°C", SensorType.DhtTemperatureSensor, port)
        {
            this.DhtSensor = dhtSensor;
        }

        public override double Value =>DhtSensor.LastTemperature;

        public DhtSensor DhtSensor { get; }

        public override void Refresh()
        {
            DhtSensor.Read();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
        }
    }
}