using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using Sensors.Weather;
using System.ComponentModel;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogUltrasonic : GrovePiSensor, IRefresher, ISensor
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly UltrasonicSensor _ultrasonicSensor;
        private double value;

        internal GrovePiAnalogUltrasonic(UltrasonicSensor ultrasonicSensor, string name, int sensorId, GrovePort port)
            : base(name, "cm", sensorId, SensorType.UltrasonicSensor, port)
        {
            _ultrasonicSensor = ultrasonicSensor;
        }

        public override double Value => value;

        public override void Refresh()
        {
            value = _ultrasonicSensor.Value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
        }
    }
}
