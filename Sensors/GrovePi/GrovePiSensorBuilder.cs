using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sensors.GrovePi
{

    public static class GrovePiSensorBuilder
    {
        private static readonly List<GrovePiSensor> _sensors = new List<GrovePiSensor>();
        private static readonly Refresher refresher = new Refresher();

        static GrovePiSensorBuilder()
        {
            refresher.Start();
        }

        public static ISensor CreateSensor(SensorType sensorType, GrovePort port, string name, int sensorId, bool rgbDisplay)
        {

#if DEBUG
            GrovePiFakeSensor fakeSensor = new GrovePiFakeSensor(sensorType, name, sensorId);
            refresher.AddSensor(fakeSensor);
            return fakeSensor;
#endif

            Iot.Device.GrovePiDevice.GrovePi grovePi = GrovePiDevice.GetGrovePi();

            switch (sensorType)
            {
                case SensorType.AnalogSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    var analogSensor = new GrovePiAnalogSensor(new AnalogSensor(grovePi, port), name, sensorId, port, rgbDisplay);
                    refresher.AddSensor(analogSensor);
                    _sensors.Add(analogSensor);
                    return analogSensor;

                case SensorType.DhtTemperatureSensor:
                    return GetDhtSensor(grovePi, port, SensorType.DhtTemperatureSensor, name, sensorId, rgbDisplay);

                case SensorType.DhtHumiditySensor:
                    return GetDhtSensor(grovePi, port, SensorType.DhtHumiditySensor, name, sensorId, rgbDisplay);

                case SensorType.PotentiometerSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    var potentiometreSensor = new GrovePiAnalogPotentiometer(new PotentiometerSensor(grovePi, port), name, sensorId, port, rgbDisplay);
                    refresher.AddSensor(potentiometreSensor);
                    _sensors.Add(potentiometreSensor);
                    return potentiometreSensor;

                case SensorType.UltrasonicSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    var ultrasonicSensor = new GrovePiAnalogUltrasonic(new UltrasonicSensor(grovePi, port), name, sensorId, port, rgbDisplay);
                    refresher.AddSensor(ultrasonicSensor);
                    _sensors.Add(ultrasonicSensor);

                    return ultrasonicSensor;

                case SensorType.GrooveTemperartureSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    var temperatureSensor = new GrovePiAnalogTemperature(new GroveTemperatureSensor(grovePi, port), name, sensorId, port, rgbDisplay);
                    refresher.AddSensor(temperatureSensor);
                    _sensors.Add(temperatureSensor);
                    return temperatureSensor;

                case SensorType.LightSensor:
                case SensorType.SoundSensor:
                default:
                    throw new Exception("GrovePiSensor: unsupported sensor type: " + sensorType);
            }
        }

        private static void ThrowExceptionIfPortIsUsed(GrovePort port)
        {
            if (_sensors.Any(sen => sen.Port == port))
            {
                throw new Exception("The port '" + port + "' is already used.");
            }
        }

        private static ISensor GetDhtSensor(Iot.Device.GrovePiDevice.GrovePi grovePi, GrovePort port, SensorType sensorType, string name, int sensorId, bool rgbDisplay)
        {
            var sensorPort = _sensors.Where(sen => sen.Port == port).ToList();

            if (sensorPort.Count > 1)
            {
                throw new Exception("The port '" + port + "' is already used.");
            }

            if (sensorType == SensorType.DhtHumiditySensor)
            {
                if (sensorPort.Count == 1)
                {
                    if (sensorPort[0].SensorType == SensorType.DhtTemperatureSensor)
                    {
                        var humiditySensor = new GrovePiDthHumiditySensor((sensorPort[0] as GrovePiDthTemperatureSensor).DhtSensor, name, sensorId, port, rgbDisplay);
                        _sensors.Add(humiditySensor);
                        return humiditySensor;
                    }
                    else
                    {
                        throw new Exception("The port '" + port + "' is already used.");
                    }
                }

                var dhtHumiditySensor = new GrovePiDthHumiditySensor(new DhtSensor(grovePi, port, DhtType.Dht22), name, sensorId, port, rgbDisplay);
                dhtHumiditySensor.Refresh();
                refresher.AddSensor(dhtHumiditySensor);
                _sensors.Add(dhtHumiditySensor);
                return dhtHumiditySensor;
            }

            else if (sensorType == SensorType.DhtTemperatureSensor)
            {
                if (sensorPort.Count == 1)
                {
                    if (sensorPort[0].SensorType == SensorType.DhtHumiditySensor)
                    {
                        var temperatureSensor = new GrovePiDthTemperatureSensor((sensorPort[0] as GrovePiDthHumiditySensor).DhtSensor, name, sensorId, port, rgbDisplay);
                        _sensors.Add(temperatureSensor);
                        return temperatureSensor;
                    }
                    else
                    {
                        throw new Exception("The port '" + port + "' is already used.");
                    }
                }

                var dhtTemperatureSensor = new GrovePiDthTemperatureSensor(new DhtSensor(grovePi, port, DhtType.Dht22), name, sensorId, port, rgbDisplay);
                dhtTemperatureSensor.Refresh();
                refresher.AddSensor(dhtTemperatureSensor);
                _sensors.Add(dhtTemperatureSensor);

                return dhtTemperatureSensor;
            }

            throw new Exception("Type " + sensorType + " is not compatible for DhtSensor");
        }
    }
}
