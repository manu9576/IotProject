using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogUltrasonic : ISensor
    {
        private readonly UltrasonicSensor _ultrasonicSensor;
        private double value;

        internal GrovePiAnalogUltrasonic(UltrasonicSensor ultrasonicSensor, string name)
        {
            _ultrasonicSensor = ultrasonicSensor;
            Name = name;
        }

        public string Name { get; private set; }

        public double Value
        {
            get
            {
                return value;
            }
        }

        public string Unit => "cm";

        public void Refresh()
        {
            value = _ultrasonicSensor.Value;
        }
    }
}
