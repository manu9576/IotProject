using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sensors.GrovePi
{

    public static class GrovePiSensorBuilder
    {
        private static readonly List<GrovePort> _usedPort = new List<GrovePort>();
        private static readonly Dictionary<GrovePort, DhtSensor> _dhtSensor = new Dictionary<GrovePort, DhtSensor>();
        private static readonly List<ISensor> Sensors = new List<ISensor>();
        private static CancellationTokenSource cancellationTokenSource;

        public static ISensor CreateSensor(SensorType sensorType, GrovePort port, string name)
        {

            cancellationTokenSource = new CancellationTokenSource();

            PeriodicRefreshTask(1000, cancellationTokenSource.Token);
#if DEBUG
            return new GrovePiFakeSensor(sensorType, name);
#endif

            var grovePi = GrovePiDevice.GetDevice();

            switch (sensorType)
            {
                case SensorType.AnalogSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    _usedPort.Add(port);
                    var analogSensor = new GrovePiAnalogSensor(new AnalogSensor(grovePi, port), name);
                    Sensors.Add(analogSensor);
                    return analogSensor;

                case SensorType.DhtTemperatureSensor:
                    return new GrovePiDthTemperatureSensor(GetDhtSensor(grovePi, port), name);

                case SensorType.DhtHumiditySensor:
                    return new GrovePiDthHumiditySensor(GetDhtSensor(grovePi, port), name);

                case SensorType.PotentiometerSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    _usedPort.Add(port);
                    var potentiometreSensor = new GrovePiAnalogPotentiometer(new PotentiometerSensor(grovePi, port), name);
                    Sensors.Add(potentiometreSensor);
                    return potentiometreSensor;

                case SensorType.UltrasonicSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    _usedPort.Add(port);
                    var ultrasonicSensor= new GrovePiAnalogUltrasonic(new UltrasonicSensor(grovePi, port), name);
                    Sensors.Add(ultrasonicSensor);
                    return ultrasonicSensor;

                case SensorType.GrooveTemperartureSensor:
                    ThrowExceptionIfPortIsUsed(port);
                    _usedPort.Add(port);
                    var temperatureSensor = new GrovePiAnalogTemperature(new GroveTemperatureSensor(grovePi, port), name);
                    Sensors.Add(temperatureSensor);
                    return temperatureSensor;

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

        private static DhtSensor GetDhtSensor(Iot.Device.GrovePiDevice.GrovePi grovePi, GrovePort port)
        {
            if (_dhtSensor.Keys.Contains(port))
            {
                return _dhtSensor[port];
            }

            ThrowExceptionIfPortIsUsed(port);

            _usedPort.Add(port);
            _dhtSensor.Add(port, new DhtSensor(grovePi, port, DhtType.Dht11));

            return _dhtSensor[port];
        }

        private static void PeriodicRefreshTask(int intervalInMS, CancellationToken cancellationToken)
        {

            Task.Run(async () =>
            {
                while (true)
                {

                    foreach (var sensor in Sensors)
                        sensor.Refresh();

                    await Task.Delay(intervalInMS, cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            });
        }
    }
}
