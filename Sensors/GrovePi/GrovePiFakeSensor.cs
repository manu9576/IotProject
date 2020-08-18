using Sensors.Weather;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sensors.GrovePi
{
    public class GrovePiFakeSensor : ISensor, IRefresher, INotifyPropertyChanged
    {
        private readonly SensorType sensorType;
        private readonly Random rand;
        private readonly double maxValue;
        private double value;
        private string name;
        private int sensorId;

        public event PropertyChangedEventHandler PropertyChanged;

        public GrovePiFakeSensor(SensorType sensorType, string name, int sensorId)
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

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
                OnPropertyChanged();
            }
        }

        public double Value
        {
            get
            {
                return value;
            }
            private set
            {
                this.value = value;
                OnPropertyChanged();
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

        public int SensorId
        {
            get
            {
                return sensorId;
            }
        }

        public void Refresh()
        {
            Value = rand.NextDouble() * maxValue;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}