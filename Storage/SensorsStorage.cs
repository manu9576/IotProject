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

        private readonly Dictionary<Sensor, ISensor> sensors;
        private readonly CancellationTokenSource cancellationTokenSource;
        private SensorsContext db;

        private SensorsStorage()
        {
            sensors = new Dictionary<Sensor, ISensor>();

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

        private void OpenDB()
        {

            if(db != null)
            {
                db.Dispose();
            }

            db = new SensorsContext();

            sensors.Clear();

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
                sens.Name == sensor.Name &&
                sens.Device == device);

                if (dbSensor == null)
                {
                    dbSensor = new Sensor
                    {
                        Name = sensor.Name,
                        Unit = sensor.Unit
                    };

                    device.Sensors.Add(dbSensor);
                    db.SaveChanges();
                }

                sensors.Add(dbSensor, sensor);
            }

        }

        private void CloseDB()
        {
            db.Dispose();
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();

        }

        void PeriodicMesureTask(int intervalInMS, CancellationToken cancellationToken)
        {

            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        OpenDB();
                        Measurement();
                        CloseDB();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("DB exception: " + ex);
                    }

                    await Task.Delay(intervalInMS, cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            });
        }

        private void Measurement()
        {
            var dateTime = DateTime.Now;

            foreach (var sensor in sensors.Keys)
            {
                sensor.Measures.Add(new Measure
                {
                    Value = sensors[sensor].Value,
                    DateTime = dateTime
                });
            }

            var re = db.SaveChanges();

            Console.WriteLine(re);


        }
    }
}
