using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using Sensors.Weather;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogUltrasonic : GrovePiSensor, IRefresher
    {
        private readonly UltrasonicSensor _ultrasonicSensor;
        private double value;

        internal GrovePiAnalogUltrasonic(UltrasonicSensor ultrasonicSensor, string name ,GrovePort port)
            : base(name,"cm",SensorType.UltrasonicSensor, port)
        {
            _ultrasonicSensor = ultrasonicSensor;
        }

        public override double Value => value;

        public override void Refresh()
        {
            value = _ultrasonicSensor.Value;
        }
    }
}
