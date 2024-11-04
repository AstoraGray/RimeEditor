using System;
using System.Collections.Generic;
using System.Linq;
using RimeFramework.Tool;

namespace RimeFramework.Core
{
    /// <summary>
    /// 霜 · 导航组 ➡️
    /// </summary>
    public class Navigations : Singleton<Navigations>
    {
        private static readonly List<PanelGroup> _groups = new();

        private static readonly Dictionary<Type,Panel> _dicPanels = new();

        // public Panel Open<T>() where T : Panel,new()
        // {
        //     if(!_dicPanels.ContainsKey())
        // }

        public void Back()
        {
            
        }
    }
}