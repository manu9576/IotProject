﻿using Iot.Device.GrovePiDevice.Models;
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

        public static ISensor CreateSensor(SensorType sensorType, GrovePort port, string name)
        {

#if DEBUG
            return new GrovePiFakeSensor(sensorType, name);
#endif

            var grovePi = GrovePiDevice.GetDevice();

            switch (sensorType)
            {
                case SensorType.AnalogSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    var analogSensor = new GrovePiAnalogSensor(new AnalogSensor(grovePi, port), name, port);
                    refresher.AddSensor(analogSensor);
                    return analogSensor;

                case SensorType.DhtTemperatureSensor:
                    return new GrovePiDthTemperatureSensor(GetDhtSensor(grovePi, port, SensorType.DhtTemperatureSensor), name, port); ;

                case SensorType.DhtHumiditySensor:
                    return new GrovePiDthHumiditySensor(GetDhtSensor(grovePi, port, SensorType.DhtHumiditySensor), name, port);

                case SensorType.PotentiometerSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    var potentiometreSensor = new GrovePiAnalogPotentiometer(new PotentiometerSensor(grovePi, port), name, port);
                    refresher.AddSensor(potentiometreSensor);
                    return potentiometreSensor;

                case SensorType.UltrasonicSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    var ultrasonicSensor= new GrovePiAnalogUltrasonic(new UltrasonicSensor(grovePi, port), name, port);
                    refresher.AddSensor(ultrasonicSensor);
                    return ultrasonicSensor;

                case SensorType.GrooveTemperartureSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    var temperatureSensor = new GrovePiAnalogTemperature(new GroveTemperatureSensor(grovePi, port), name, port);
                    refresher.AddSensor(temperatureSensor);
                    return temperatureSensor;

                case SensorType.LightSensor:
                case SensorType.SoundSensor:
                default:
                    throw new Exception("GrovePiSensor: unsupported sensor type: " + sensorType);
            }
        }

        private static void ThrowExceptionIfPortIsUsed(GrovePort port)
        {
            if (_sensors.Any(sen => sen.Port ==port))
            {
                throw new Exception("The port '" + port + "' is already used.");
            }
        }

        private static DhtSensor GetDhtSensor(Iot.Device.GrovePiDevice.GrovePi grovePi, GrovePort port, SensorType sensorType)
        {
            var sensorPort = _sensors.Where(sen => sen.Port == port).ToList();

            if(sensorPort.Count >1)
            {
                throw new Exception("The port '" + port + "' is already used.");
            }

            if(sensorType == SensorType.DhtHumiditySensor)
            {
                if(sensorPort.Count == 1 && 
                    sensorPort[0].SensorType == SensorType.DhtTemperatureSensor)
                {
                    return (sensorPort[0] as GrovePiDthTemperatureSensor).DhtSensor;
                }

                return new DhtSensor(grovePi, port, DhtType.Dht11);
            }

            else if (sensorType == SensorType.DhtTemperatureSensor)
            {
                if (sensorPort.Count == 1 &&
                    sensorPort[0].SensorType == SensorType.DhtHumiditySensor)
                {
                    return (sensorPort[0] as GrovePiDthHumiditySensor).DhtSensor;
                }

                return new DhtSensor(grovePi, port, DhtType.Dht11);
            }

            throw new Exception("Type " + sensorType + " is not compatible for DhtSensor");
        }

    }
}
