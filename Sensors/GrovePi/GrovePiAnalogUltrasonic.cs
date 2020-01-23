using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogUltrasonic : IGrovePiSensor
    {
        private UltrasonicSensor _ultrasonicSensor;

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
                return _ultrasonicSensor.Value;
            }
        }

        public string Unit => "cm";
    }
}
