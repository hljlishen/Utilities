using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Utilities.Display
{
    public class DisplayerGroupManager
    {
        private Dictionary<Panel, Displayer> PanelMap = new Dictionary<Panel, Displayer>();
        private Displayer draggingDisplayer;
        private Panel draggingPanel;
       
        public void AddDisplayer(Displayer d)
        {
            if (PanelMap.ContainsKey(d.Panel))
                throw new Exception();
            PanelMap.Add(d.Panel, d);
            d.Panel.MouseDoubleClick += Panel_MouseDoubleClick;
        }

        private void Panel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(draggingDisplayer == null)
            {
                draggingPanel = sender as Panel;
                draggingDisplayer = PanelMap[draggingPanel];
            }
            else
            {
                var dropPanel = sender as Panel;
                var dropDisplayer = PanelMap[dropPanel];
                dropDisplayer.Stop();
                draggingDisplayer.Stop();
                dropDisplayer.SetPanel(draggingPanel);
                draggingDisplayer.SetPanel(dropPanel);
                PanelMap[dropPanel] = draggingDisplayer;
                PanelMap[draggingPanel] = dropDisplayer;
                dropDisplayer.Start();
                draggingDisplayer.Start();

                draggingDisplayer = null;
                draggingPanel = null;
            }
        }
    }
}
