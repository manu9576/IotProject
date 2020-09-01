using Sensors;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Storage
{
    public class SensorsStorage
    {
        private static SensorsStorage sensorsStorage = null;
        private readonly CancellationTokenSource cancellationTokenSource = null;

        private SensorsStorage()
        {
            cancellationTokenSource = new CancellationTokenSource();
        }

        public static SensorsStorage GetInstance()
        {
            if (sensorsStorage == null)
            {
                sensorsStorage = new SensorsStorage();
            }

            return sensorsStorage;
        }

        public void Start(int intervalInSeconds)
        {
            PeriodicMesureTask(intervalInSeconds * 1000, cancellationTokenSource.Token);
        }

        private Dictionary<Sensor, ISensor> ReadSensors(SensorsContext db)
        {
            var sensors = new Dictionary<Sensor, ISensor>();

            Device device;

            string deviceName = Environment.MachineName;

            if (!db.Devices.Any(dev => dev.Name == deviceName))
            {
                device = new Device
                {
                    Name = deviceName
                };

                db.Devices.Add(device);
                db.SaveChanges();
            }
            else
            {
                device = db.Devices.FirstOrDefault(dev => dev.Name == deviceName);
            }

            var deviceSensors = SensorsManager.Sensors.ToList();
            Sensor dbSensor = null;

            foreach (var sensor in deviceSensors)
            {

                dbSensor = db.Sensors.FirstOrDefault(sens =>
                    sens.SensorId == sensor.SensorId);

                if (dbSensor == null)
                {
                    dbSensor = new Sensor();
                    device.Sensors.Add(dbSensor);
                    
                }

                dbSensor.Name = sensor.Name;
                dbSensor.Unit = sensor.Unit;
                db.SaveChanges();

                sensor.SensorId = dbSensor.SensorId;

                sensors.Add(dbSensor, sensor);
            }

            return sensors;
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        void PeriodicMesureTask(int intervalInMS, CancellationToken cancellationToken)
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

        private void Measurement(SensorsContext db, Dictionary<Sensor, ISensor> sensors)
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
                Console.WriteLine("Values saved: ");
                foreach (var sensor in sensors.Keys)
                {
                    var value = sensors[sensor].Value;
                    if (double.IsNormal(value))
                    {
                        Console.WriteLine("\t--Value: " + value + " / DateTime: " + dateTime);
                    }
                }
            }
        }
    }
}
