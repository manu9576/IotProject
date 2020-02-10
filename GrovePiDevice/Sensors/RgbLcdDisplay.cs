using Iot.Device.GrovePiDevice;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrovePiDevice.Sensors
{
    public interface IRgbLcdDisplay
    {
        IRgbLcdDisplay SetBacklightRgb(byte red, byte green, byte blue);
        IRgbLcdDisplay SetText(string text);
    }

    public class RgbLcdDisplay : IRgbLcdDisplay
    {


        internal static class Constants
        {
            public const byte Unused = 0;
            public const byte GroveVcc = 5;
            public const byte AdcVoltage = 5;
            public const int GroveRgpLcdMaxLength = 16;
            public const int GroveRgpLcdRows = 2;

            public const byte DisplayRgbI2CAddress = 0xC4;
            public const byte DisplayTextI2CAddress = 0x7C;
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

        private static RgbLcdDisplay _rgbLcdDisplay;

        internal RgbLcdDisplay(I2cDevice rgbDevice, I2cDevice textDevice)
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

        public static RgbLcdDisplay BuildRgbLcdDisplayImpl()
        {
            if (null != _rgbLcdDisplay)
            {
                return _rgbLcdDisplay;
            }

            /* Initialize the I2C bus */
            var rgbConnectionSettings = new I2cConnectionSettings(Constants.DisplayRgbI2CAddress >> 1, Iot.Device.GrovePiDevice.GrovePi.DefaultI2cAddress);

            var textConnectionSettings = new I2cConnectionSettings(Constants.DisplayTextI2CAddress >> 1, Iot.Device.GrovePiDevice.GrovePi.DefaultI2cAddress);


            //_rgbLcdDisplay = Task.Run(async () =>
            //{
            //    var dis = await GetDeviceInfo();

            //    // Create an I2cDevice with our selected bus controller and I2C settings
            //    var rgbDevice = await I2cDevice.FromIdAsync(dis[0].Id, rgbConnectionSettings);
            //    var textDevice = await I2cDevice.FromIdAsync(dis[0].Id, textConnectionSettings);
            //    return new RgbLcdDisplay(rgbDevice, textDevice);
            //}).Result;
            return _rgbLcdDisplay;
        }



        /**
    rgb_lcd.cpp
    2013 Copyright (c) Seeed Technology Inc.  All right reserved.

    Author:Loovee
    2013-9-18

    add rgb backlight fucnction @ 2013-10-15

    The MIT License (MIT)

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE.1  USA
/

# include <Arduino.h>
# include <stdio.h>
# include <string.h>
# include <inttypes.h>
# include <Wire.h>

# include "rgb_lcd.h"

        void i2c_send_byte(unsigned char dta)
        {
            Wire.beginTransmission(LCD_ADDRESS);        // transmit to device #4
            Wire.write(dta);                            // sends five bytes
            Wire.endTransmission();                     // stop transmitting
        }

        void i2c_send_byteS(unsigned char* dta, unsigned char len)
        {
            Wire.beginTransmission(LCD_ADDRESS);        // transmit to device #4
            for (int i = 0; i < len; i++)
            {
                Wire.write(dta[i]);
            }
            Wire.endTransmission();                     // stop transmitting
        }

        rgb_lcd::rgb_lcd() {
}

    void rgb_lcd::begin(uint8_t cols, uint8_t lines, uint8_t dotsize)
    {

        Wire.begin();

        if (lines > 1)
        {
            _displayfunction |= LCD_2LINE;
        }
        _numlines = lines;
        _currline = 0;

        // for some 1 line displays you can select a 10 pixel high font
        if ((dotsize != 0) && (lines == 1))
        {
            _displayfunction |= LCD_5x10DOTS;
        }

        // SEE PAGE 45/46 FOR INITIALIZATION SPECIFICATION!
        // according to datasheet, we need at least 40ms after power rises above 2.7V
        // before sending commands. Arduino can turn on way befer 4.5V so we'll wait 50
        delayMicroseconds(50000);


        // this is according to the hitachi HD44780 datasheet
        // page 45 figure 23

        // Send function set command sequence
        command(LCD_FUNCTIONSET | _displayfunction);
        delayMicroseconds(4500);  // wait more than 4.1ms

        // second try
        command(LCD_FUNCTIONSET | _displayfunction);
        delayMicroseconds(150);

        // third go
        command(LCD_FUNCTIONSET | _displayfunction);


        // finally, set # lines, font size, etc.
        command(LCD_FUNCTIONSET | _displayfunction);

        // turn the display on with no cursor or blinking default
        _displaycontrol = LCD_DISPLAYON | LCD_CURSOROFF | LCD_BLINKOFF;
        display();

        // clear it off
        clear();

        // Initialize to default text direction (for romance languages)
        _displaymode = LCD_ENTRYLEFT | LCD_ENTRYSHIFTDECREMENT;
        // set the entry mode
        command(LCD_ENTRYMODESET | _displaymode);


        // backlight init
        setReg(REG_MODE1, 0);
        // set LEDs controllable by both PWM and GRPPWM registers
        setReg(REG_OUTPUT, 0xFF);
        // set MODE2 values
        // 0010 0000 -> 0x20  (DMBLNK to 1, ie blinky mode)
        setReg(REG_MODE2, 0x20);

        setColorWhite();

    }

    /********** high level commands, for the user! 
    void rgb_lcd::clear()
    {
        command(LCD_CLEARDISPLAY);        // clear display, set cursor position to zero
        delayMicroseconds(2000);          // this command takes a long time!
    }

    void rgb_lcd::home()
    {
        command(LCD_RETURNHOME);        // set cursor position to zero
        delayMicroseconds(2000);        // this command takes a long time!
    }

    void rgb_lcd::setCursor(uint8_t col, uint8_t row)
    {

        col = (row == 0 ? col | 0x80 : col | 0xc0);
        unsigned char dta[2] = { 0x80, col };

        i2c_send_byteS(dta, 2);

    }

    // Turn the display on/off (quickly)
    void rgb_lcd::noDisplay()
    {
        _displaycontrol &= ~LCD_DISPLAYON;
        command(LCD_DISPLAYCONTROL | _displaycontrol);
    }

    void rgb_lcd::display()
    {
        _displaycontrol |= LCD_DISPLAYON;
        command(LCD_DISPLAYCONTROL | _displaycontrol);
    }

    // Turns the underline cursor on/off
    void rgb_lcd::noCursor()
    {
        _displaycontrol &= ~LCD_CURSORON;
        command(LCD_DISPLAYCONTROL | _displaycontrol);
    }

    void rgb_lcd::cursor()
    {
        _displaycontrol |= LCD_CURSORON;
        command(LCD_DISPLAYCONTROL | _displaycontrol);
    }

    // Turn on and off the blinking cursor
    void rgb_lcd::noBlink()
    {
        _displaycontrol &= ~LCD_BLINKON;
        command(LCD_DISPLAYCONTROL | _displaycontrol);
    }
    void rgb_lcd::blink()
    {
        _displaycontrol |= LCD_BLINKON;
        command(LCD_DISPLAYCONTROL | _displaycontrol);
    }

    // These commands scroll the display without changing the RAM
    void rgb_lcd::scrollDisplayLeft(void)
    {
        command(LCD_CURSORSHIFT | LCD_DISPLAYMOVE | LCD_MOVELEFT);
    }
    void rgb_lcd::scrollDisplayRight(void)
    {
        command(LCD_CURSORSHIFT | LCD_DISPLAYMOVE | LCD_MOVERIGHT);
    }

    // This is for text that flows Left to Right
    void rgb_lcd::leftToRight(void)
    {
        _displaymode |= LCD_ENTRYLEFT;
        command(LCD_ENTRYMODESET | _displaymode);
    }

    // This is for text that flows Right to Left
    void rgb_lcd::rightToLeft(void)
    {
        _displaymode &= ~LCD_ENTRYLEFT;
        command(LCD_ENTRYMODESET | _displaymode);
    }

    // This will 'right justify' text from the cursor
    void rgb_lcd::autoscroll(void)
    {
        _displaymode |= LCD_ENTRYSHIFTINCREMENT;
        command(LCD_ENTRYMODESET | _displaymode);
    }

    // This will 'left justify' text from the cursor
    void rgb_lcd::noAutoscroll(void)
    {
        _displaymode &= ~LCD_ENTRYSHIFTINCREMENT;
        command(LCD_ENTRYMODESET | _displaymode);
    }

    // Allows us to fill the first 8 CGRAM locations
    // with custom characters
    void rgb_lcd::createChar(uint8_t location, uint8_t charmap[])
    {

        location &= 0x7; // we only have 8 locations 0-7
        command(LCD_SETCGRAMADDR | (location << 3));


        unsigned char dta[9];
        dta[0] = 0x40;
        for (int i = 0; i < 8; i++)
        {
            dta[i + 1] = charmap[i];
        }
        i2c_send_byteS(dta, 9);
    }

    // Control the backlight LED blinking
    void rgb_lcd::blinkLED(void)
    {
        // blink period in seconds = (<reg 7> + 1) / 24
        // on/off ratio = <reg 6> / 256
        setReg(0x07, 0x17);  // blink every second
        setReg(0x06, 0x7f);  // half on, half off
    }

    void rgb_lcd::noBlinkLED(void)
    {
        setReg(0x07, 0x00);
        setReg(0x06, 0xff);
    }

    /*********** mid level commands, for sending data/cmds 

    // send command
    inline void rgb_lcd::command(uint8_t value)
    {
        unsigned char dta[2] = { 0x80, value };
        i2c_send_byteS(dta, 2);
    }

    // send data
    inline size_t rgb_lcd::write(uint8_t value) {

    unsigned char dta[2] = { 0x40, value };
    i2c_send_byteS(dta, 2);
    return 1; // assume sucess
}

void rgb_lcd::setReg(unsigned char addr, unsigned char dta)
{
    Wire.beginTransmission(RGB_ADDRESS); // transmit to device #4
    Wire.write(addr);
    Wire.write(dta);
    Wire.endTransmission();    // stop transmitting
}

void rgb_lcd::setRGB(unsigned char r, unsigned char g, unsigned char b)
{
    setReg(REG_RED, r);
    setReg(REG_GREEN, g);
    setReg(REG_BLUE, b);
}

const unsigned char color_define[4][3] = {
    {255, 255, 255},            // white
    {255, 0, 0},                // red
    {0, 255, 0},                // green
    {0, 0, 255},                // blue
};

void rgb_lcd::setColor(unsigned char color)
{
    if (color > 3)
    {
        return;
    }
    setRGB(color_define[color][0], color_define[color][1], color_define[color][2]);
}

         *
         * 
         * 
         * 
         **/

    }
}
