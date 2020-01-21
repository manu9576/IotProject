using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    public enum SensorType
    {
        AnalogSensor,
        DhtSensor,
        GrooveTemperartureSensor,
        LightSensor,
        PotentiometerSensor,
        SoundSensor,
        UltrasonicSensor
    }

    public class GrovePiSensor
    {
        AnalogSensor _analogSensor;
        DhtSensor _dhtSensor;

        public GrovePiSensor(SensorType sensorType, GrovePort port)
        {
            var grovePi = GrovePiDevice.GetDevice();

            switch (sensorType)
            {
                case SensorType.AnalogSensor:
                    _analogSensor = new AnalogSensor(grovePi, port);
                    break;
                case SensorType.DhtSensor:
                    _dhtSensor = new DhtSensor(grovePi, port, DhtType.Dht11);
                    break;
                case SensorType.GrooveTemperartureSensor:
                    _analogSensor = new GroveTemperatureSensor(grovePi, port);
                    break;
                case SensorType.LightSensor:
                    _analogSensor = new LightSensor(grovePi, port);
                    break;
                case SensorType.PotentiometerSensor:
                    _analogSensor = new PotentiometerSensor(grovePi, port);
                    break;
                case SensorType.SoundSensor:
                    _analogSensor = new SoundSensor(grovePi, port);
                    break;
                case SensorType.UltrasonicSensor:
                    _analogSensor = new UltrasonicSensor(grovePi, port);
                    break;
            }
        }

    }
}
