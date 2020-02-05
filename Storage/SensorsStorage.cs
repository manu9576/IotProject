﻿using Sensors;
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
        private CancellationTokenSource cancellationTokenSource;
        private SensorsContext db;


        private SensorsStorage()
        {
            sensors = new Dictionary<Sensor, ISensor>();
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
            Device device;

            string deviceName = Environment.MachineName;

            db = new SensorsContext();

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

            cancellationTokenSource = new CancellationTokenSource();
            PeriodicMesureTask(intervalInSeconds * 1000, cancellationTokenSource.Token);

        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
            db.Dispose();
        }

        void PeriodicMesureTask(int intervalInMS, CancellationToken cancellationToken)
        {

            Task.Run(async () =>
            {
                while (true)
                {

                    Mesure();

                    await Task.Delay(intervalInMS , cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            });
        }

        private void Mesure()
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

            db.SaveChanges();
        }
    }
}