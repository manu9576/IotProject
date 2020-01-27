using System;
using System.Collections.Generic;
using System.Text;

namespace Sensors.GrovePi
{
    public class GrovePiFakeSensor : ISensor
    {
        private readonly SensorType sensorType;
        private readonly Random rand;
        private readonly double maxValue;

        public GrovePiFakeSensor(SensorType sensorType, string name)
        {
            this.sensorType = sensorType;
            Name = name;
            rand = new Random();

            switch (sensorType)
            {
                case SensorType.AnalogSensor:
                    maxValue = 5.0;
                    break;

                case SensorType.GrooveTemperartureSensor:
                case SensorType.DhtTemperatureSensor:
                    maxValue = 40.0;
                    break;

                case SensorType.PotentiometerSensor:
                case SensorType.DhtHumiditySensor:
                    maxValue = 100.0;
                    break;

                case SensorType.UltrasonicSensor:
                    maxValue = 50.0;
                    break;

                case SensorType.LightSensor:
                case SensorType.SoundSensor:
                default:
                    throw new Exception("Sensor type '" + sensorType + "' is not implemented");
            }
        }

        public string Name { get; private set; }

        public double Value
        {
            get
            {
                return rand.NextDouble() * maxValue;
            }
        }

        public string Unit
        {
            get
            {
                switch (sensorType)
                {
                    case SensorType.AnalogSensor:
                        return "V";

                    case SensorType.GrooveTemperartureSensor:
                    case SensorType.DhtTemperatureSensor:
                        return "°C";

                    case SensorType.PotentiometerSensor:
                    case SensorType.DhtHumiditySensor:
                        return "%";

                    case SensorType.UltrasonicSensor:
                        return "cm";

                    case SensorType.LightSensor:
                    case SensorType.SoundSensor:
                    default:
                        throw new Exception("Sensor type '" + sensorType + "' is not implemented");   
                }
            }
        }
    }
}