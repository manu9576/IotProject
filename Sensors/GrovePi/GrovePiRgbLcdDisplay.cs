using System;
using System.Device.I2c;
using System.Threading;

namespace Sensors.GrovePi
{
    public interface IRgbLcdDisplay
    {
        IRgbLcdDisplay SetBacklightRgb(byte red, byte green, byte blue);
        IRgbLcdDisplay SetText(string text);
    }

    public class GrovePiRgbLcdDisplay : IRgbLcdDisplay
    {


        internal static class Constants
        {
            public const byte Unused = 0;
            public const byte GroveVcc = 5;
            public const byte AdcVoltage = 5;
            public const int GroveRgpLcdMaxLength = 16;
            public const int GroveRgpLcdRows = 2;

            public const byte DisplayRgbI2CAddress = 0x62;
            public const byte DisplayTextI2CAddress = 0x3E;
        }

        private const byte RedCommandAddress = 4;
        private const byte GreenCommandAddress = 3;
        private const byte BlueCommandAddress = 2;
        private const byte TextCommandAddress = 0x80;
        private const byte ClearDisplayCommandAddress = 0x01;
        private const byte DisplayOnCommandAddress = 0x08;
        private const byte NoCursorCommandAddress = 0x04;
        private const byte TwoLinesCommandAddress = 0x28;
        private const byte SetCharacterCommandAddress = 0x40;

        private static GrovePiRgbLcdDisplay _rgbLcdDisplay;

        internal GrovePiRgbLcdDisplay(I2cDevice rgbDevice, I2cDevice textDevice)
        {
            if (rgbDevice == null) throw new ArgumentNullException(nameof(rgbDevice));
            if (textDevice == null) throw new ArgumentNullException(nameof(textDevice));

            RgbDirectAccess = rgbDevice;
            TextDirectAccess = textDevice;
        }

        internal I2cDevice RgbDirectAccess { get; }
        internal I2cDevice TextDirectAccess { get; }

        public IRgbLcdDisplay SetBacklightRgb(byte red, byte green, byte blue)
        {
            //TODO: Find out what these addresses are for , set const.
            RgbDirectAccess.Write(new byte[] { 0, 0 });
            RgbDirectAccess.Write(new byte[] { 1, 0 });
            RgbDirectAccess.Write(new byte[] { DisplayOnCommandAddress, 0xaa });
            RgbDirectAccess.Write(new[] { RedCommandAddress, red });
            RgbDirectAccess.Write(new[] { GreenCommandAddress, green });
            RgbDirectAccess.Write(new[] { BlueCommandAddress, blue });
            return this;
        }

        public IRgbLcdDisplay SetText(string text)
        {
            TextDirectAccess.Write(new[] { TextCommandAddress, ClearDisplayCommandAddress });
            Thread.Sleep(50);
            TextDirectAccess.Write(new[] { TextCommandAddress, (byte)(DisplayOnCommandAddress | NoCursorCommandAddress) });
            TextDirectAccess.Write(new[] { TextCommandAddress, TwoLinesCommandAddress });

            var count = 0;
            var row = 0;

            foreach (var c in text)
            {
                if (c.Equals('\n') || count == Constants.GroveRgpLcdMaxLength)
                {
                    count = 0;
                    row += 1;
                    if (row == Constants.GroveRgpLcdRows)
                        break;
                    TextDirectAccess.Write(new byte[] { TextCommandAddress, 0xc0 }); //TODO: find out what this address is
                    if (c.Equals('\n'))
                        continue;
                }
                count += 1;
                TextDirectAccess.Write(new[] { SetCharacterCommandAddress, (byte)c });
            }

            return this;
        }

        public static GrovePiRgbLcdDisplay BuildRgbLcdDisplayImpl()
        {
            if (null != _rgbLcdDisplay)
            {
                return _rgbLcdDisplay;
            }

            /* Initialize the I2C bus */
            var rgbConnectionSettings = new I2cConnectionSettings(1, Constants.DisplayRgbI2CAddress);

            var textConnectionSettings = new I2cConnectionSettings(1, Constants.DisplayTextI2CAddress);


            var rgbDevice = I2cDevice.Create(rgbConnectionSettings);
            var textDevice = I2cDevice.Create(textConnectionSettings);

            _rgbLcdDisplay = new GrovePiRgbLcdDisplay(rgbDevice, textDevice);

            return _rgbLcdDisplay;
        }

    }
}
