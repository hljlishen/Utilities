using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Utilities.Display
{
    public struct ExclusiveSwitchableManagerProperties
    {
        public Point Location;
        public uint MaxiumButtonInRow;
        public Size ButtonSize;

        public ExclusiveSwitchableManagerProperties(Point location) : this(location, 3, new Size(80, 40)) 
        { 
        }

        public ExclusiveSwitchableManagerProperties(Point location, uint maxiumButtonInRow, Size buttonSize)
        {
            MaxiumButtonInRow = maxiumButtonInRow;
            ButtonSize = buttonSize;
            Location = location;
        }
    }
    public class ExclusiveSwitchableManager : DynamicElement<ExclusiveSwitchableManagerProperties>
    {
        private Dictionary<ButtonElement, ISwtichable> buttonMap = new Dictionary<ButtonElement, ISwtichable>();
        private uint currentRowButtonNumber = 0;
        private Point currentPos;

        public ExclusiveSwitchableManager(ExclusiveSwitchableManagerProperties p)
        {
            Model = p;
            currentPos = p.Location;
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            lock(Locker)
            {
                foreach (var btn in buttonMap.Keys)
                {
                    displayer.Elements.Add(LayerId, btn);
                    displayer.Elements.Add(LayerId, buttonMap[btn] as GraphicElement);
                }
            }
        }

        public void Add(ISwtichable s)
        {
            lock(Locker)
            {
                if (buttonMap.Values.Contains(s))
                    return;
                var properties = new ButtenProperties(currentPos, Model.ButtonSize, s.Name);
                var btn = new ButtonElement(properties);
                btn.Update(properties);
                btn.Clicked += Btn_Clicked;
                buttonMap.Add(btn, s);
                UpdateGraphic();

                currentRowButtonNumber++;
                if(currentRowButtonNumber < Model.MaxiumButtonInRow)
                {
                    currentPos.X += Model.ButtonSize.Width;
                }
                else
                {
                    currentRowButtonNumber = 0;
                    currentPos.X = Model.Location.X;
                    currentPos.Y += Model.ButtonSize.Height;
                }
            }
        }

        private void Btn_Clicked(ButtonElement obj)
        {
            lock(Locker)
            {
                var switchable = buttonMap[obj];
                if (switchable.IsOn)
                    switchable.Off();
                else
                {
                    foreach (var btn in buttonMap.Keys)     //确保只有一个控件能工作
                    {
                        if (btn == obj)
                            buttonMap[btn].On();
                        else
                        {
                            buttonMap[btn].Off();
                            btn.Selected = false;
                        }
                    }
                }
            }
        }
        protected override void DrawDynamicElement(RenderTarget rt)
        {
            //此控件不绘制任何图像，由它保存的button和ISwitchable自己绘制
        }
    }
}
