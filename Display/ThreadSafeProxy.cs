//using System.Drawing;
//using System.Windows.Forms;
//using Utilities.Guards;
//using Utilities.Mapper;

//namespace Utilities.Display
//{
//    public class ThreadSafeProxy : SmartElement
//    {
//        protected SmartElement element;
//        protected readonly object Locker = new object();

//        public static SmartElement MakeThreadSafe(SmartElement e) => new ThreadSafeProxy(e);

//        public override bool HasChanged()
//        {
//            lock (Locker)
//            {
//                return element.HasChanged();
//            }
//        }

//        public ThreadSafeProxy(SmartElement element)
//        {
//            Guard.AssertNotNull(element);
//            this.element = element;
//        }

//        public override void Draw(Graphics g, IScreenToCoordinateMapper mapper)
//        {
//            lock (Locker)
//            {
//                element.Draw(g, mapper);
//            }
//        }

//        public override void MouseDown(object sender, MouseEventArgs e, Displayer displayer)
//        {
//            lock (Locker)
//            {
//                element.MouseDown(sender, e, displayer);
//            }
//        }

//        public override void MouseMove(object sender, MouseEventArgs e, Displayer displayer)
//        {
//            lock (Locker)
//            {
//                element.MouseMove(sender, e, displayer);
//            }
//        }

//        public override void MouseUp(object sender, MouseEventArgs e, Displayer displayer)
//        {
//            lock (Locker)
//            {
//                element.MouseUp(sender, e, displayer);
//            }
//        }

//        public override void Update(object data)
//        {
//            lock (Locker)
//            {
//                element.Update(data);
//            }
//        }
//    }
//}
