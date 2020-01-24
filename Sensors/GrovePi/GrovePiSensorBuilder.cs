using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sensors.GrovePi
{

    public static class GrovePiSensorBuilder
    {
        private static readonly List<GrovePort> _usedPort = new List<GrovePort>();
        private static readonly Dictionary<GrovePort, DhtSensor> _dhtSensor = new Dictionary<GrovePort, DhtSensor>();
        private static readonly Iot.Device.GrovePiDevice.GrovePi _grovePi = GrovePiDevice.GetDevice();

        public static IGrovePiSensor CreateSensor(SensorType sensorType, GrovePort port, string name)

        {
            switch (sensorType)
            {
                case SensorType.AnalogSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    _usedPort.Add(port);
                    return new GrovePiAnalogSensor(new AnalogSensor(_grovePi, port), name);

                case SensorType.DhtTemperatureSensor:
                    return new GrovePiDthTemperatureSensor(GetDhtSensor(port), name);

                case SensorType.DhtHumiditySensor:
                    return new GrovePiDthHumiditySensor(GetDhtSensor(port), name);

                case SensorType.PotentiometerSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    _usedPort.Add(port);
                    return new GrovePiAnalogPotentiometer(new PotentiometerSensor(_grovePi, port), name);

                case SensorType.UltrasonicSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    _usedPort.Add(port);
                    return new GrovePiAnalogUltrasonic(new UltrasonicSensor(_grovePi, port), name);

                case SensorType.GrooveTemperartureSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    _usedPort.Add(port);
                    return new GrovePiAnalogTemperature(new GroveTemperatureSensor(_grovePi, port), name);

                case SensorType.LightSensor:
                case SensorType.SoundSensor:
                default:
                    throw new Exception("GrovePiSensor: unsupported sensor type: " + sensorType);
            }
        }

        private static void ThrowExceptionIfPortIsUsed(GrovePort port)
        {
            if (_usedPort.Contains(port))
            {
                throw new Exception("The port '" + port + "' is already used.");
            }
        }

        private static DhtSensor GetDhtSensor(GrovePort port)
        {
            if (_dhtSensor.Keys.Contains(port))
            {
                return _dhtSensor[port];
            }

            ThrowExceptionIfPortIsUsed(port);

            _usedPort.Add(port);

            return new DhtSensor(_grovePi, port, DhtType.Dht11);
        }

    }
}
