using System.Device.I2c;

namespace Sensors.GrovePi
{
    internal class GrovePiDevice
    {
        private static Iot.Device.GrovePiDevice.GrovePi _grovePi;
        private static I2cDevice _i2CDevice;

        public static Iot.Device.GrovePiDevice.GrovePi GetGrovePi()
        {
            if (_grovePi == null)
            {

                _grovePi = new Iot.Device.GrovePiDevice.GrovePi(GetI2Device());
            }

            return _grovePi;
        }

        public static I2cDevice GetI2Device()
        {
            if (_i2CDevice == null)
            {

                I2cConnectionSettings i2CConnectionSettings = new I2cConnectionSettings(1, Iot.Device.GrovePiDevice.GrovePi.DefaultI2cAddress);
                _i2CDevice = I2cDevice.Create(i2CConnectionSettings);

            }

            return _i2CDevice;
        }
    }
}
