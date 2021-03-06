﻿using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System;
using System.Drawing;
using System.Windows.Forms;
using Utilities.Coordinates;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.Display
{
    public class LocationDisplay : DynamicElement<LocationDisplayModel>
    {
        private PointF currentCoordinateLocation;
        private PointF currentScreenLcation;
        private Brush textBrush;
        public TextFormat textFormat;

        public LocationDisplay(LocationDisplayModel displayModel)
        {
            DisplayModel = displayModel;
        }

        public LocationDisplay() : this(new LocationDisplayModel() { coordinateType = CoordinateType.Rectangular, FixLocation = new PointF(), FontColor = Color.White, FontName = "Berlin Sans FB Demi", FontSize = 25, LocationType = CoordinateLocation.FixedPosition})
        {
        }

        public LocationDisplayModel DisplayModel { get; private set; }

        public override void Dispose()
        {
            base.Dispose();
            textBrush?.Dispose();
            textFormat?.Dispose();
        }
        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            textBrush = DisplayModel.FontColor.SolidBrush(rt);
            DWriteFactory dw = DWriteFactory.CreateFactory();
            textFormat = dw.CreateTextFormat(DisplayModel.FontName, DisplayModel.FontSize);
            dw.Dispose();
        }

        protected override void BindEvents(Panel p)
        {
            base.BindEvents(p);
            Panel.MouseMove += Panel_MouseMove;
        }

        protected override void UnbindEvents(Panel p)
        {
            base.UnbindEvents(p);
            Panel.MouseMove -= Panel_MouseMove;
        }

        private void Panel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            currentScreenLcation = e.Location;
            currentCoordinateLocation = Mapper.GetCoordinateLocation(e.X, e.Y);
            UpdateView();
        }

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            if (HasChanged())
            {
                textBrush?.Dispose();
                textFormat?.Dispose();
                InitializeComponents(rt);
            }
            RectF r = GetDisplayRect(rt);
            string content = GetDisplayContent();
            rt.DrawText(content, textFormat, r, textBrush);
        }

        private string GetDisplayContent()
        {
            switch (DisplayModel.coordinateType)
            {
                case CoordinateType.Polar:
                    var p = new RectangularCoordinate(currentCoordinateLocation.X, currentCoordinateLocation.Y, 0).Polar;
                    return $"Az:{p.Az:0.0} ,Dis:{p.Dis:0.0}";
                case CoordinateType.Rectangular:
                    return $"X:{currentCoordinateLocation.X} ,Y:{currentCoordinateLocation.Y}";
                case CoordinateType.Screen:
                    return $"X:{currentScreenLcation.X} ,Y:{currentScreenLcation.Y}";
                default:
                    throw new Exception("CoordinateType变量的值无效");
            }
        }

        private RectF GetDisplayRect(RenderTarget rt)
        {
            if (DisplayModel.LocationType == CoordinateLocation.FollowMouse)
            {
                return new RectF(currentScreenLcation.X + 30, currentScreenLcation.Y, currentScreenLcation.X + 1000, currentScreenLcation.Y + 1000);
            }
            else
            {
                return new RectangleF(DisplayModel.FixLocation.X, DisplayModel.FixLocation.Y, 1000, 1000).ToRectF();
            }
        }

        protected override void DoUpdate(LocationDisplayModel t) => DisplayModel = t;
    }
}
