using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Sensors.GrovePi
{
    public interface IRgbLcdDisplay
    {
        IRgbLcdDisplay SetBacklightRgb(byte red, byte green, byte blue);
        IRgbLcdDisplay SetText(string text);
        IRgbLcdDisplay SetText(string line1, string line2);
    }

    public class GrovePiRgbLcdDisplay : IRgbLcdDisplay
    {
        private const byte DisplayRgbI2CAddress = 0x62;
        private const byte DisplayTextI2CAddress = 0x3E;

        private const int GroveRgpLcdMaxLength = 16;
        private const int GroveRgpLcdRows = 2;

        private const byte BlueCommandAddress = 2;
        private const byte GreenCommandAddress = 3;
        private const byte RedCommandAddress = 4;

        private const byte TextCommandAddress = 0x80;
        private const byte ClearDisplayCommandAddress = 0x01;
        private const byte DisplayOnCommandAddress = 0x08;
        private const byte NoCursorCommandAddress = 0x04;
        private const byte TwoLinesCommandAddress = 0x28;
        private const byte SetCharacterCommandAddress = 0x40;
        private const byte NewLineCommand = 0xc0;

        internal I2cDevice RgbDirectAccess;
        internal I2cDevice TextDirectAccess;

        internal GrovePiRgbLcdDisplay(I2cDevice rgbDevice, I2cDevice textDevice)
        {
            RgbDirectAccess = rgbDevice ?? throw new ArgumentNullException(nameof(rgbDevice));
            TextDirectAccess = textDevice ?? throw new ArgumentNullException(nameof(textDevice));
        }


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

            var textWithouDiacirtics = ReplaceDegreeSymbol(RemoveDiacritics(text));

            foreach (var charater in textWithouDiacirtics)
            {
                if (charater.Equals('\n') || count == GroveRgpLcdMaxLength)
                {
                    count = 0;
                    row += 1;

                    if (row == GroveRgpLcdRows)
                        break;

                    TextDirectAccess.Write(new byte[] { TextCommandAddress, NewLineCommand });

                    if (charater.Equals('\n'))
                        continue;
                }
                count += 1;
                TextDirectAccess.Write(new[] { SetCharacterCommandAddress, (byte)charater });
            }

            return this;
        }

        public IRgbLcdDisplay SetText(string line1, string line2)
        {
            TextDirectAccess.Write(new[] { TextCommandAddress, ClearDisplayCommandAddress });
            Thread.Sleep(50);
            TextDirectAccess.Write(new[] { TextCommandAddress, (byte)(DisplayOnCommandAddress | NoCursorCommandAddress) });
            TextDirectAccess.Write(new[] { TextCommandAddress, TwoLinesCommandAddress });

            foreach (var charater in FormatText(line1))
            {
                TextDirectAccess.Write(new[] { SetCharacterCommandAddress, (byte)charater });
            }

            TextDirectAccess.Write(new byte[] { TextCommandAddress, NewLineCommand });

            foreach (var charater in FormatText(line2))
            {
                TextDirectAccess.Write(new[] { SetCharacterCommandAddress, (byte)charater });
            }

            return this;
        }

        private static List<GrovePiRgbLcdDisplay> rgbLcdDisplays = new List<GrovePiRgbLcdDisplay>();

        public static GrovePiRgbLcdDisplay BuildRgbLcdDisplayImpl(int displayRgbI2CAdress = DisplayRgbI2CAddress, int displayTextI2C = DisplayTextI2CAddress)
        {
            if (rgbLcdDisplays.Any(display => display.RgbDirectAccess.ConnectionSettings.DeviceAddress == displayRgbI2CAdress))
            {
                throw new Exception($"Impossible to create display RGB: the adresse {displayRgbI2CAdress:X} is already used");
            }

            if (rgbLcdDisplays.Any(display => display.TextDirectAccess.ConnectionSettings.DeviceAddress == displayTextI2C))
            {
                throw new Exception($"Impossible to create display text: the adresse {displayTextI2C:X} is already used");
            }

            /* Initialize the I2C bus */
            var rgbConnectionSettings = new I2cConnectionSettings(1, displayRgbI2CAdress);
            var rgbDevice = I2cDevice.Create(rgbConnectionSettings);

            var textConnectionSettings = new I2cConnectionSettings(1, displayTextI2C);
            var textDevice = I2cDevice.Create(textConnectionSettings);

            return new GrovePiRgbLcdDisplay(rgbDevice, textDevice);
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private static string ReplaceDegreeSymbol(string text)
        {
            return text.Replace('°', '\x00DF');
        }

        private static string FormatText(string text)
        {
            var formatedText = ReplaceDegreeSymbol(RemoveDiacritics(text));

            if(formatedText.Length > GroveRgpLcdMaxLength)
            {
                var formatedTextWithoutSpace = formatedText.Replace(" ", "");

                if (formatedTextWithoutSpace.Length > GroveRgpLcdMaxLength)
                {
                    return formatedTextWithoutSpace.Substring(0, GroveRgpLcdMaxLength);
                }
                else 
                {
                    return formatedTextWithoutSpace;
                }
            }
            else
            {
                return formatedText;
            }
        }
    }
}
