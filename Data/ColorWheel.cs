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
            if(_colorsIndex == _colors.Length)
                _colorsIndex = 0;

            return _colors[_colorsIndex];
        }
    }
}
