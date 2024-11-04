using System.Collections.Generic;
using System.Linq;

namespace RimeFramework.Core
{
    public class PanelGroup
    {
        public string Name { get; private set; }

        private readonly List<Panel> _panels = new ();

        public PanelGroup(string name) => Name = name;

        public Panel Open<T>() where T : Panel, new()
        {
            Panel panel = _panels.First(panel => panel as T != null);
            if (panel == _panels.Last())
            {
                return panel;
            }
            _panels.Last().Hide();
            
            if (panel == null)
            {
                panel = Pools.OnTake<T>();
            }

            _panels.Remove(panel);
            _panels.Add(panel);
            
            panel.Show();
            
            return panel;
        }

        public bool Back()
        {
            Panel panel = _panels.Last();
            panel.Hide();
            _panels.Remove(panel);
            if (_panels.Count == 0)
            {
                return false;
            }
            _panels.Last().Show();
            return true;
        }
    }
}