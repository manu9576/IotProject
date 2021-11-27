﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Storage.Models
{
    public class Device : IDtoDevice
    {
        [Key]
        public int DeviceId { get; set; }
        public string Name { get; set; }

        public IList<IDtoSensor> Sensors { get; } = new List<IDtoSensor>();
    }
}
