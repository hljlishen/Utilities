using System.Drawing;

namespace Utilities.Display
{
    public struct MarkerModel
    {
        public Color LineColor;
        public float LineWidth;
        public uint ObjectNumber;
        public Color SelectedLineColor;
        public float SelectedLineWidth;
        public string FontName;
        public float FontSize;
        public Color FontColor;
        public string SelectedFontName;
        public float SelectedFontSize;
        public Color SelectedFontColor;

        public MarkerModel(Color lineColor, float lineWidth, uint markerNumber, Color selectedLineColor, float selectedLineWidth, string fontName, float fontSize, Color fontColor, string selectedFontName, float selectedFontSize, Color selectedFontColor)
        {
            LineColor = lineColor;
            LineWidth = lineWidth;
            ObjectNumber = markerNumber;
            SelectedLineColor = selectedLineColor;
            SelectedLineWidth = selectedLineWidth;
            FontName = fontName;
            FontSize = fontSize;
            FontColor = fontColor;
            SelectedFontName = selectedFontName;
            SelectedFontSize = selectedFontSize;
            SelectedFontColor = selectedFontColor;
        }
    }
}
