using BlazorBootstrap;
using System.Drawing;

namespace BetterBudgetWeb.Data
{
    public class ColorWheel
    {
        private static Color[] _colors = {
            Color.Indigo,
            Color.Aquamarine,
            Color.DarkCyan,
            Color.LimeGreen,
            Color.Goldenrod,
            Color.Red,
            Color.MediumVioletRed,
            Color.Teal,
            Color.MediumSpringGreen,
            Color.Chartreuse,
            Color.RoyalBlue,
            Color.MediumSlateBlue,
            Color.Wheat,
            Color.Firebrick,
            Color.CadetBlue,
            Color.DarkOliveGreen,
            Color.BlueViolet,
            Color.DarkSalmon,
            Color.Blue,
            Color.SandyBrown,
        };
        private static int _colorsCount => _colors.Length;
        private static int _colorsIndex = 0;
        public static Color NextColor()
        {
            _colorsIndex++;
            if (_colorsIndex == _colors.Length)
                _colorsIndex = 0;

            return _colors[_colorsIndex];
        }
        public static string NextColorString()
        {
            return NextColor().ToHexString();
        }
        public static string RandomInSet(string RGB)
        {
            if (string.IsNullOrEmpty(RGB))
                RGB = "R";

            Random rnd = new Random();
            int r = RGB.ToUpper().StartsWith("R") ? 255 : rnd.Next(256);
            int g = RGB.ToUpper().StartsWith("G") ? 255 : rnd.Next(256);
            int b = RGB.ToUpper().StartsWith("B") ? 255 : rnd.Next(256);

            Color randomColor = Color.FromArgb(r,g,b);

            return randomColor.ToHexString();
        }

    }
}
