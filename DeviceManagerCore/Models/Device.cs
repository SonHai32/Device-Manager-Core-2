using System;
using System.Collections.Generic;

#nullable disable

namespace DeviceManagerCore.Models
{
    public partial class Device
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int DeviceQuantity { get; set; }
        public float DevicePrice { get; set; }
    }
}
