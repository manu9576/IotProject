using System.Device.I2c;

namespace Sensors.GrovePi
{
    internal class GrovePiDevice
    {
        private static Iot.Device.GrovePiDevice.GrovePi _grovePi;

        public static Iot.Device.GrovePiDevice.GrovePi GetDevice()
        {
            if (_grovePi == null)
            {
                I2cConnectionSettings i2CConnectionSettings = new I2cConnectionSettings(1, Iot.Device.GrovePiDevice.GrovePi.DefaultI2cAddress);
                _grovePi = new Iot.Device.GrovePiDevice.GrovePi(I2cDevice.Create(i2CConnectionSettings));
            }

            return _grovePi;
        }
    }
}
