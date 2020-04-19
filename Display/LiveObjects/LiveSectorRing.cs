using System;
using System.Drawing;
using Utilities.Tools;

namespace Utilities.Display
{
    public class LiveSectorRing : LiveObject
    {
        public PointF ScrP1 { get; set; }
        public PointF ScrP2 { get; set; }
        public PointF Center { get; set; }
        public override bool IsPointNear(PointF m)
        {
            var p1Dis = Functions.DistanceBetween(ScrP1.ToPoint2F(), Center.ToPoint2F());
            var p2Dis = Functions.DistanceBetween(ScrP2.ToPoint2F(), Center.ToPoint2F());
            var angle1 = Functions.StandardAngle(90 - Functions.RadianToDegree(Math.Atan2(Center.Y - ScrP1.Y, ScrP1.X - Center
                .X)));
            var angle2 = Functions.StandardAngle(90 - Functions.RadianToDegree(Math.Atan2(Center.Y - ScrP2.Y, ScrP2.X - Center
                .X)));

            var mDis = Functions.DistanceBetween(m.ToPoint2F(), Center.ToPoint2F());
            var anglem = Functions.StandardAngle(90 - Functions.RadianToDegree(Math.Atan2(Center.Y - m.Y, m.X - Center
                .X)));

            var begin = Functions.FindSmallArcBeginAngle(angle1, angle2);
            var end = Functions.FindSmallArcEndAngle(angle1, angle2);
            if (begin > end)
            {
                return (anglem >= begin || anglem <= end) && mDis >= Math.Min(p1Dis, p2Dis) && mDis <= Math.Max(p1Dis, p2Dis);
            }
            else
                return anglem >= begin && anglem <= end && mDis >= Math.Min(p1Dis, p2Dis) && mDis <= Math.Max(p1Dis, p2Dis);
        }
    }
}
