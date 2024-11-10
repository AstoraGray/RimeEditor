using System;
using System.Collections.Generic;
using System.Linq;
using RimeFramework.Tool;
using UnityEngine;
using UnityEngine.UI;

namespace RimeFramework.Core
{
    /// <summary>
    /// 霜 · 导航组 ➡️
    /// </summary>
    /// <b> Note: 对游戏中的所有面板进行导航，并且有防循环机制
    /// <see cref="Open<T>"/> 打开某类型面板，调度导航组
    /// <see cref="Back()"/> 返回上一级面板，若当前组无面板则进入上个导航组
    /// <see cref="Clear()"/> 销毁所有的面板
    /// <remarks>Author: AstoraGray</remarks>
    [RequireComponent(typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster))]
    public class Navigations : Singleton<Navigations>
    {
        private static readonly List<PanelGroup> _groups = new();

        private static readonly Dictionary<Type,Panel> _dicPanels = new();

        public static string CurGroupName => _groups.LastOrDefault()?.Name;
        /// <summary>
        /// 初始化Canvas
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            gameObject.layer = LayerMask.NameToLayer("UI");
            Canvas canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.vertexColorAlwaysGammaSpace = true;
            CanvasScaler scale = GetComponent<CanvasScaler>();
            scale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scale.referenceResolution = new Vector2(Screen.width, Screen.height);
        }
        /// <summary>
        /// 打开面板
        /// </summary>
        /// <typeparam name="T">面板类型</typeparam>
        /// <returns></returns>
        public static Panel Open<T>() where T : Panel,new()
        {
            Type type = typeof(T);
            Panel panel;
            if (!_dicPanels.ContainsKey(type))
            {
                _dicPanels[type] = Pools.Take<T>();
                panel = _dicPanels[type];
                panel.transform.SetParent(Instance.transform);
                RectTransform rect = panel.transform as RectTransform;
                rect.offsetMax = Vector2.zero;
            }
            panel = _dicPanels[type];
            PanelGroup group = _groups.LastOrDefault();

            if (group?.Name == panel.Group)
            {
                group.Open(panel);
                Consoles.Print(typeof(Navigations),$"打开面板{group.DebugPanels()}，当前导航组为{DebugGroups()}");
                return panel;
            }
            
            _groups.LastOrDefault()?.Close();
            
            for (var i = 0; i < _groups.Count; i++)
            {
                if (_groups[i].Name == panel.Group)
                {
                    group = _groups[i];
                    break;
                }
            }

            if (group?.Name != panel.Group)
            {
                group = new PanelGroup(panel.Group);
            }

            group.Clear();
            
            _groups.Remove(group);
            _groups.Add(group);

            group.Open(panel);
            Consoles.Print(typeof(Navigations),$"打开面板{group.DebugPanels()}，当前导航组为{DebugGroups()}");
            return panel;
        }
        /// <summary>
        /// 返回上一级
        /// </summary>
        /// <returns></returns>
        public static Panel Back()
        {
            PanelGroup group = _groups.LastOrDefault();
            if (group == null)
            {
                Consoles.Print(typeof(Navigations),$"当前无导航组，无法返回");
                return null;
            }

            bool isBack = group.Back();
            Panel panel = null;
            
            if (!isBack)
            {
                _groups.Remove(_groups.LastOrDefault());
                group = _groups.LastOrDefault();
                panel = group?.Open();
                if (panel == null)
                {
                    Consoles.Print(typeof(Navigations),$"当前已无面板");
                    return panel;
                }
            }

            Consoles.Print(typeof(Navigations),$"打开面板{group?.DebugPanels()}，当前导航组为{DebugGroups()}");

            return panel;
        }
        
        public static void Clear()
        {
            Pools.Clear<Panel>();
            _groups.Clear();
            _dicPanels.Clear();
            Consoles.Print(typeof(Navigations),$"销毁所有面板");
        }
        /// <summary>
        /// 输出导航组Debug信息
        /// </summary>
        /// <returns></returns>
        private static string DebugGroups()
        {
            string groups = "";
            for (var i = _groups.Count - 1; i >= 0; i--)
            {
                groups += _groups[i].Name;
                if (i > 0)
                {
                    groups += "-->";
                }
            }

            return groups;
        }
    }
}