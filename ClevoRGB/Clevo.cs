using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ClevoRGB
{
    public static class Clevo
    {
        #region Imports
        [DllImport("perkey_api.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr InitPerkeyIo();

        [DllImport("perkey_api.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetLEDStatus(byte cmd, byte data0, byte data1, byte data2, byte data3);

        [DllImport("perkey_api.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetLedData(byte cmd, byte type, byte[] arr, byte size);
        #endregion

        private static void SetKeyColor(byte key, byte r, byte g, byte b) => SetLEDStatus(1, key, r, g, b);

        private const byte ROWS = 5;
        private const byte COLUMNS = 19;

        private static readonly byte[] _colors = new byte[384];
        private static Dictionary<Key, int[]> _layout;

        public static bool Initialize()
        {
            try
            {
                InitPerkeyIo();//TODO: This initializes on systems without the keyboard
                _layout = Layouts.ISO15;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Update()
        {
            for (byte row = 0; row <= ROWS; row++)
            {
                for (byte col = 0; col <= COLUMNS; col++)
                {
                    var offset = GetIndex(row, col);
                    byte r = _colors[offset + 0];
                    byte g = _colors[offset + 1];
                    byte b = _colors[offset + 2];

                    byte key = Convert.ToByte(row << 5 | col);
                    SetKeyColor(key, r, g, b);
                }
            }
        }

        public static void SetColorFull(Color clr)
        {
            for (byte row = 0; row <= ROWS; row++)
            {
                for (byte col = 0; col <= COLUMNS; col++)
                {
                    SetColorWithIndex(GetIndex(row, col), clr);
                }
            }
        }

        public static void SetKeyColor(Key key, Color clr)
        {
            if (_layout.TryGetValue(key, out var index))
                for (byte i = 0; i < index.Length; i++)
                    SetColorWithIndex(index[i], clr);
        }

        public static void SetCoordColor(byte row, byte col, Color clr) => SetColorWithIndex(GetIndex(row, col), clr);

        private static void SetColorWithIndex(int idx, Color clr)
        {
            _colors[idx + 0] = clr.R;
            _colors[idx + 1] = clr.G;
            _colors[idx + 2] = clr.B;
        }

        private static int GetIndex(byte row, byte col)
        {
            return (64 * row) + (col * 3) + (col / 5);
        }
    }
}
