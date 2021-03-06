﻿using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public abstract class MarkerElement: DynamicElement<MarkerModel>
    {
        protected Brush normalLineBrush;
        protected Brush selectedLineBrush;
        protected TextFormat normalTextFormat;
        protected TextFormat selectedTextFormat;
        protected Brush normalTextBrush;
        protected Brush selectedTextBrush;

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            normalLineBrush?.Dispose();
            selectedLineBrush?.Dispose();
            normalTextFormat?.Dispose();
            selectedTextFormat?.Dispose();
            normalTextBrush?.Dispose();
            selectedTextBrush?.Dispose();

            normalLineBrush = Model.LineColor.SolidBrush(rt);
            selectedLineBrush = Model.SelectedLineColor.SolidBrush(rt);
            normalTextBrush = Model.FontColor.SolidBrush(rt);
            selectedTextBrush = Model.SelectedFontColor.SolidBrush(rt);
            using (var dw = DWriteFactory.CreateFactory())
            {
                normalTextFormat = dw.CreateTextFormat(Model.FontName, Model.FontSize);
                selectedTextFormat = dw.CreateTextFormat(Model.SelectedFontName, Model.SelectedFontSize);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            normalLineBrush?.Dispose();
            selectedLineBrush?.Dispose();
            normalTextFormat?.Dispose();
            selectedTextFormat?.Dispose();
            normalTextBrush?.Dispose();
            selectedTextBrush?.Dispose();
        }

        public MarkerElement(MarkerModel model)
        {
            Model = model;
        }
    }
}
