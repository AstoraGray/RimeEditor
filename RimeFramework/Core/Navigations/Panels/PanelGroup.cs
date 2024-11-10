using System.Collections.Generic;
using System.Linq;

namespace RimeFramework.Core
{
    /// <summary>
    /// 导航组
    /// </summary>
    public class PanelGroup
    {
        public string Name { get; private set; } // 导航组名

        private readonly List<Panel> _panels = new (); // 面板列表
        /// <summary>
        /// 初始化导航组
        /// </summary>
        /// <param name="name">组名</param>
        public PanelGroup(string name) => Name = name;
        /// <summary>
        /// 打开当前面板
        /// </summary>
        /// <returns></returns>
        public Panel Open()
        {
            return Open(_panels.LastOrDefault());
        }
        /// <summary>
        /// 打开面板
        /// </summary>
        /// <param name="panel">面板实例</param>
        /// <returns></returns>
        public Panel Open(Panel panel)
        {
            if (panel == _panels.LastOrDefault())
            {
                panel?.Show();
                return panel;
            }
            _panels.LastOrDefault()?.Hide();

            _panels.Remove(panel);
            _panels.Add(panel);
            
            panel.Show();
            
            return panel;
        }
        /// <summary>
        /// 返回上一级
        /// </summary>
        /// <returns></returns>
        public bool Back()
        {
            Panel panel = _panels.LastOrDefault();
            panel?.Hide();
            _panels.Remove(panel);
            if (_panels.Count == 0)
            {
                return false;
            }
            _panels.LastOrDefault()?.Show();
            return true;
        }
        /// <summary>
        /// 关闭当前面板
        /// </summary>
        public void Close()
        {
            _panels.LastOrDefault()?.Hide();
        }
        /// <summary>
        /// 清理面板列表（不是销毁）
        /// </summary>
        public void Clear()
        {
            _panels.Clear();
        }
        /// <summary>
        /// 打印面板Debug信息
        /// </summary>
        /// <returns></returns>
        public string DebugPanels()
        {
            string log = "";
            for (var i = 0; i < _panels.Count; i++)
            {
                log += _panels[i].GetType().Name;
                if (i < _panels.Count - 1)
                {
                    log += "-->";
                }
            }

            return log;
        }
    }
}