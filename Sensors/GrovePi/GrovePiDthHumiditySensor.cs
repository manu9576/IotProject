using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using Sensors.Weather;
using System.ComponentModel;

namespace Sensors.GrovePi
{
    internal class GrovePiDthHumiditySensor : GrovePiSensor, IRefresher, ISensor
    {
        public GrovePiDthHumiditySensor(DhtSensor dhtSensor, string name, GrovePort port)
            : base(name, "%", SensorType.DhtHumiditySensor, port)
        {
            this.DhtSensor = dhtSensor;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override double Value => DhtSensor.LastRelativeHumidity;

        public DhtSensor DhtSensor { get; }

        public override void Refresh()
        {
            DhtSensor.Read();
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Value"));
        }
    }
}