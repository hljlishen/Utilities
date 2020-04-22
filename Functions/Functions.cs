using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Utilities.Tools
{
    public class Functions
    {
        public const float Interval = 0.0001f;
        public static ColorF GetColorFFromRgb(int r, int g, int b) => new ColorF(new ColorI(r, g, b));

        public static double DegreeToRadian(double angle) => Math.PI * angle / 180;//角度转弧度

        public static double RadianToDegree(double radian) => 180 * radian / Math.PI;//弧度转角度

        public static Rect ZoomRectangle(Rect rect, double percent)     //以rect的中心点为中心，将rect放缩percent的百分比
        {
            var r = new Rect();
            var location = new Point2F();
            double offsetX, offsetY;
            r.Width = (int)(rect.Width * percent);
            r.Height = (int)(rect.Height * percent);

            if (percent < 1)    //缩小
            {
                offsetX = (double)(rect.Width - r.Width) / 2;
                offsetY = (double)(rect.Height - r.Height) / 2;
                location.X = rect.Left + (int)offsetX;
                location.Y = rect.Top + (int)offsetY;
            }
            else   //放大
            {
                offsetX = (double)(r.Width - rect.Width) / 2;
                offsetY = (double)(r.Height - rect.Height) / 2;
                location.X = rect.Left - (int)offsetX;
                location.Y = rect.Top - (int)offsetY;
            }

            r.Left = (int)location.X + rect.Left;
            r.Top = (int)location.Y + rect.Top;
            r.Right += (int)(location.X);
            r.Bottom += (int)location.Y;
            return r;
        }

        public static double AngleToNorth(PointF center, PointF p)
        {
            var angle = 90 - RadianToDegree(Math.Atan2(center.Y - p.Y, p.X - center.X));
            return StandardAngle(angle);
        }
        public static double FindSmallArcBeginAngle(double a, double b)
        {
            var max = Math.Max(a, b);
            var min = Math.Min(a, b);

            return max - min <= 180 ? min : max;
        }

        public static double FindSmallArcEndAngle(double a, double b)
        {
            var max = Math.Max(a, b);
            var min = Math.Min(a, b);

            return max - min <= 180 ? max : min;
        }

        public static Point2F FindCenterPosition(Rect rect)
        {
            if (rect == null)
                throw (new Exception("rectangle对象为空"));
            var p = new Point2F
            {
                X = rect.Left + rect.Width / 2,
                Y = rect.Top + rect.Height / 2
            };

            return p;
        }       //找矩形中心点

        public static float AngleToNorth(Point2F center, Point2F p)    //两点连线与垂直线的夹角
        {
            var distance = (float)DistanceBetween(center, p);
            var x = p.X - center.X;
            var angleR = (float)Math.Asin(x / distance);
            var angle = RadianToDegree(angleR);

            //float y = center.Y - p.Y;
            //double angleR1 = Math.Acos(y / distance);
            //angleR1 = RadianToDegree(angleR1);

            if (p.X >= center.X && p.Y <= center.Y)   //第一象限
            {

            }
            else if (p.X >= center.X && p.Y > center.Y) //第二象限
            {
                angle = 180 - angle;
            }
            else if (p.X < center.X && p.Y > center.Y) //第三象限
            {
                angle = 180 + Math.Abs(angle);
            }
            else//第四象限
            {
                angle = 360 + angle;
            }

            return (float)angle;
        }

        public static double DistanceBetween(Point2F p1, Point2F p2)
        {
            var a = Math.Pow((double)p1.X - p2.X, 2);
            var b = Math.Pow((double)p1.Y - p2.Y, 2);
            var c = Math.Sqrt(a + b);

            return c;
        }   //两点间的距离
        public static double DistanceBetween(Point p1, Point p2)
        {
            var a = Math.Pow((double)p1.X - p2.X, 2);
            var b = Math.Pow((double)p1.Y - p2.Y, 2);
            var c = Math.Sqrt(a + b);

            return c;
        }

        public static Rect MoveRect(Rect r, Point2F p) //将r横向移动p.x, 纵向移动p.y
        {
            var ret = r;
            ret.Left += (int)p.X;
            ret.Right += (int)p.X;
            ret.Top += (int)p.Y;
            ret.Bottom += (int)p.Y;

            return ret;
        }

        public static RectF MoveRectToPoint(RectF rect, Point2F point)
        {
            float width = rect.Width;
            float height = rect.Height;

            rect.Left = point.X;
            rect.Top = point.Y;
            rect.Right = rect.Left + width;
            rect.Bottom = rect.Top + height;

            return rect;
        }

        public static Point2F PointToPoint2F(Point p) => new Point2F(p.X, p.Y);

        public static Point Point2FToPoint(Point2F p) => new Point((int)p.X, (int)p.Y);

        public static Rectangle D2DRectToGdiRectangle(Rect r)
        {
            var ret = new Rectangle();

            //左上角位置
            var p = new Point(r.Left, r.Top);
            ret.Location = p;

            ret.Width = r.Right - r.Left;
            ret.Height = r.Bottom - r.Top;

            return ret;
        }

        public static float FindSmallArcBeginAngle(float a, float b)
        {
            var max = Math.Max(a, b);
            var min = Math.Min(a, b);

            return max - min <= 180 ? min : max;
        }

        public static float FindSmallArcEndAngle(float a, float b)
        {
            var max = Math.Max(a, b);
            var min = Math.Min(a, b);

            return max - min <= 180 ? max : min;
        }

        public static bool FloatEquals(float a, float b) => Math.Abs(a - b) < Interval;
        public static bool DoubleEquals(double a, double b) => Math.Abs(a - b) < Interval;
        //public static bool IsPointInRect(Point2F p, Rect r) => p.X >= r.Left && p.X <= r.Right && p.Y >= r.Top && p.Y <= r.Bottom;
        //public static bool IsPointInRect(Point p, RectF r) => p.X >= r.Left && p.X <= r.Right && p.Y >= r.Top && p.Y <= r.Bottom;
        public static double StandardAngle(double angle) //将角度转化为0-360的浮点数
        {
            if (DoubleEquals(angle, 0))
                return 0;
            else if (DoubleEquals(angle, 360))
                return 360;
            else if (angle > 0 && angle < 360)
                return angle;
            else if (angle < 0)
            {
                while (angle < 0)
                    angle += 360;
            }
            else
            {
                angle %= 360;
            }

            return angle;
        }

        public static double ReverAngleDirection(double angle)
        {
            double rAngle = 360f - angle;
            rAngle = StandardAngle(rAngle);

            return rAngle;
        }

        public static byte[] FloatToBytes(float value, int validBits)  //浮点数保留精度后转为byte数组，高位在前
        {
            List<byte> ls = new List<byte>();
            int valueInt = (int)(value * Math.Pow(10, validBits));
            int rightShiftCount = 0;

            while (true)
            {
                int tmp = valueInt >> (rightShiftCount++ * 8);
                if (tmp == 0)
                    break;

                ls.Insert(0, (byte)(tmp & 0xff));
            }

            return ls.ToArray();
        }

        public static float BytesToFloat(byte[] value, int validBits)
        {
            int tmp = 0;
            foreach (var t in value)
            {
                tmp <<= 8;
                tmp += t;
            }

            return (float)(tmp / Math.Pow(10, validBits));
        }

        public static int MakeInt(byte[] data, int pos, int count)     //用字节组合成整型，高位在前
        {
            int ret = 0;
            for (int i = 0; i < count; i++, pos++)
            {
                ret <<= 8;
                ret += data[pos];
            }

            return ret;
        }

        public static double CalRotationTimeSpan(float beginAngle, float endAngle, int rotationsPerMinute, int rotationDirection)      //rotationDirection = 1为顺时针， rotationDirection=2位逆时针
        {
            if (FloatEquals(beginAngle, endAngle)) return 0;
            double angleSpan;
            double degreePerSecond = rotationsPerMinute * 360f / 60f;
            switch (rotationDirection)
            {
                case 1: //顺时针
                    if (beginAngle > endAngle)      //天线需要跨越正北
                    {
                        angleSpan = endAngle + 360 - beginAngle;
                    }
                    else
                    {
                        angleSpan = endAngle - beginAngle;
                    }
                    break;
                case 2: //逆时针
                    if (beginAngle > endAngle)
                    {
                        angleSpan = beginAngle - endAngle;
                    }
                    else
                    {
                        angleSpan = beginAngle + 360 - endAngle;
                    }
                    break;
                default:
                    throw new Exception("CalRotationTimeSpan函数收到的转动方向错误");
            }

            return angleSpan / degreePerSecond;
        }

        public static int RandomInt(int minimum, int maximum)
        {
            var r = new Random(DateTime.Now.Millisecond);
            return r.Next(minimum, maximum);
        }

        public static byte[] IntToByteLsb(int number, int reqiredMinimumLength)   //整型转化为byte数组，小端模式
        {
            List<byte> ls = new List<byte>();
            if (number < 255)
                ls.Add((byte)number);
            else
            {
                int tmp = number;

                for (int shiftCount = 0; ; shiftCount += 8)
                {
                    tmp = number >> shiftCount;
                    if (tmp == 0)
                        break; ;
                    byte b = (byte)(tmp & 0xff);
                    ls.Add(b);
                }
            }

            int actualLength = ls.Count;
            for (int i = 0; i < reqiredMinimumLength - actualLength; i++)
                ls.Add(0);

            return ls.ToArray();
        }

        public static byte CalBytesXor(byte[] data, int offset, int length) //计算异或和
        {
            int tmp = data[offset] ^ data[offset + 1];
            for (int dataIndex = offset + 2; dataIndex <= length - offset - 1; dataIndex++)
            {
                tmp ^= data[dataIndex];
            }

            return (byte)(tmp & 0xff);
        }

        public static T[] GetArrayRange<T>(T[] array, int startIndex, int count)
        {
            if (startIndex + count > array.Length)
                throw new Exception("长度超出数组范围");

            T[] ret = new T[count];

            for (int i = 0; i < count; i++)
            {
                ret[i] = array[startIndex + i];
            }
            return ret;
        }
    }
}
