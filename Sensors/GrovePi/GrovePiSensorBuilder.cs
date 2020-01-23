using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sensors.GrovePi
{

    public class GrovePiSensorBuilder
    {
        private static Dictionary<GrovePort, object> _existingSensor = new Dictionary<GrovePort, object>();

        public static IGrovePiSensor CreateSensor(SensorType sensorType, GrovePort port, string name)
        {
            var grovePi = GrovePiDevice.GetDevice();

            switch (sensorType)
            {
                case SensorType.AnalogSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    return new GrovePiAnalogSensor(new AnalogSensor(grovePi, port), name);

                case SensorType.DhtTemperatureSensor:
                    return new GrovePiDthTemperatureSensor(grovePi, port);

                case SensorType.DhtHumiditySensor:
                    return new GrovePiDthTemperatureSensor(grovePi, port);

                case SensorType.PotentiometerSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    return new GrovePiAnalogPotentiometer(new PotentiometerSensor(grovePi, port), name);

                case SensorType.UltrasonicSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    return new GrovePiAnalogUltrasonic(new UltrasonicSensor(grovePi, port), name);

                case SensorType.GrooveTemperartureSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    return new GrovePiAnalogTemperature(new GroveTemperatureSensor(grovePi, port), name);

                case SensorType.LightSensor:
                case SensorType.SoundSensor:
                default:
                    throw new Exception("GrovePiSensor: unsupported sensor type: " + sensorType);
            }
        }

        private static void ThrowExceptionIfPortIsUsed(GrovePort port)
        {
            if (_existingSensor.Keys.Contains(port))
            {
                throw new Exception("The port '" + port + "' is already used.");
            }
        }
    }
}
