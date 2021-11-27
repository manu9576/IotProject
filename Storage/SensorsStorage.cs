using Sensors;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Storage
{
    public class SensorsStorage : ISensorsStorage
    {
        private static ISensorsStorage sensorsStorage = null;
        private readonly CancellationTokenSource cancellationTokenSource = null;

        private SensorsStorage()
        {
            cancellationTokenSource = new CancellationTokenSource();
        }

        public static ISensorsStorage Instance
        {
            get
            {
                if (sensorsStorage == null)
                {
                    sensorsStorage = new SensorsStorage();
                }

                return sensorsStorage;
            }
        }

        public static int GetNewSensorId(string sensorName)
        {
            var db = new SensorsContext();
            string deviceName = Environment.MachineName;

            IDtoDevice device = GetOrCreateDeviceByName(deviceName, db);

            var dbSensor = new Sensor
            {
                Name = sensorName
            };
            device.Sensors.Add(dbSensor);

            db.SaveChanges();

            return dbSensor.SensorId;
        }

        private static IDtoDevice GetOrCreateDeviceByName(string deviceName, SensorsContext dataBase)
        {
            if (DeviceNameExistInDataBase(deviceName, dataBase))
            {
                return dataBase.Devices.FirstOrDefault(dev => dev.Name == deviceName);
            }
            else
            {
                return CreateDeviceInDataBase(deviceName, dataBase);
            }
        }

        private static IDtoDevice CreateDeviceInDataBase(string deviceName, SensorsContext dataBase)
        {
            var device = new Device
            {
                Name = deviceName
            };

            dataBase.Devices.Add(device);
            dataBase.SaveChanges();

            return device;
        }

        private static bool DeviceNameExistInDataBase(string deviceName, SensorsContext dataBase)
        {
            return !dataBase.Devices.Any(dev => dev.Name == deviceName);
        }

        public void Start(int intervalInSeconds)
        {
            PeriodicMesureTask(intervalInSeconds * 1000, cancellationTokenSource.Token);
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        private Dictionary<IDtoSensor, ISensor> ReadSensors(SensorsContext db)
        {
            var sensors = new Dictionary<IDtoSensor, ISensor>();
            // TODO remove static call !!
            var deviceSensors = SensorsManager.Sensors.ToList();
            IDtoSensor dbSensor = null;

            foreach (var sensor in deviceSensors)
            {
                dbSensor = db.Sensors.FirstOrDefault(sens =>
                    sens.SensorId == sensor.SensorId);

                if (dbSensor == null)
                {
                    Console.WriteLine("Error while reading sensor: sensor not found " + sensor.SensorId);
                    continue;
                }

                if (dbSensor.Name != sensor.Name || dbSensor.Unit != sensor.Unit)
                {
                    dbSensor.Name = sensor.Name;
                    dbSensor.Unit = sensor.Unit;

                    db.SaveChanges();
                }

                sensors.Add(dbSensor, sensor);
            }

            return sensors;
        }

        private void PeriodicMesureTask(int intervalInMS, CancellationToken cancellationToken)
        {

            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        using var db = new SensorsContext();
                        var sensors = ReadSensors(db);
                        Measurement(db, sensors);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("DB exception: " + ex);
                    }

                    await Task.Delay(intervalInMS, cancellationToken);
                }
            });
        }

        private void Measurement(SensorsContext db, Dictionary<IDtoSensor, ISensor> sensors)
        {
            var dateTime = DateTime.Now;

            foreach (var sensor in sensors.Keys)
            {
                var value = sensors[sensor].Value;

                if (double.IsNormal(value))
                {
                    sensor.Measures.Add(new Measure
                    {
                        Value = value,
                        DateTime = dateTime
                    });
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while saving new values: " + ex.Message);
            }
        }
    }
}
