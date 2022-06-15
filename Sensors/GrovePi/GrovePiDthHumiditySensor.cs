using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using Sensors.Weather;
using System.ComponentModel;

namespace Sensors.GrovePi
{
    internal class GrovePiDthHumiditySensor : GrovePiSensor, IRefresher, ISensor
    {
        public GrovePiDthHumiditySensor(DhtSensor dhtSensor, string name, int sensorId, GrovePort port, bool rgbDisplay)
            : base(name, "%", sensorId, SensorType.DhtHumiditySensor, port, rgbDisplay)
        {
            this.DhtSensor = dhtSensor;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override double Value => DhtSensor.LastRelativeHumidity;

        public DhtSensor DhtSensor { get; }

        public override void RefreshValues()
        {
            DhtSensor.Read();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
        }
    }
}