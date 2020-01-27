using Sensors;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storage
{
    public class SensorsStorage
    {
        private static SensorsStorage sensorsStorage = null;

        private Device device;
        private List<ISensor> sensors;

        public static SensorsStorage GetInstance()
        {
            if(sensorsStorage == null)
            {
                sensorsStorage = new SensorsStorage();
            }

            return sensorsStorage;
        }

        public void Start(int intervalInMunite)
        {
            string deviceName = Environment.MachineName;

            using (var db = new SensorsContext())
            {

                if(db.Devices.Any(dev => dev.Name == deviceName))
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


                sensors = SensorsManager.Sensors.ToList();

                foreach(var sensor in sensors)
                {
                    if(device.Sensors.Any(dbSensor => dbSensor.Name == sensor.Name))
                    {


                        device.Sensors.A
                    }
                }
            }
        }
    }
}
