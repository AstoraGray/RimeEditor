using System;
using System.Collections.Generic;
using System.Linq;
using RimeFramework.Tool;
using RimeFramework.Utility;
using UnityEngine;
using Object = System.Object;

namespace RimeFramework.Core
{
    /// <summary>
    /// 霜 · 控制台 💻
    /// </summary>
    /// <b> Note: 类打印传Type，实例传Object,目前可以配置特定的颜色
    /// <see cref="Print(Type, string)"/> 打印类
    /// <see cref="Print(Object, string)"/> 打印实例
    /// <remarks>Author: AstoraGray</remarks>
    public class Consoles : Singleton<Consoles>
    {
        private static readonly Dictionary<string, Color> _dicMapping = new()
        {
            { nameof(RimeManager), Color.cyan },
            { nameof(States), Color.cyan },
            { nameof(Controls), Color.cyan },
            { nameof(Cycles), Color.cyan },
            { nameof(Pools), Color.cyan },
            { nameof(Navigations),Color.cyan},
            { nameof(Scenes), Color.cyan },
            { nameof(Animators),Color.cyan},
            { nameof(Observers),Color.cyan}
        };
        
        public static void Print(string name,string content)
        {
            if (_dicMapping.ContainsKey(name))
            {
                name = $"<color={_dicMapping[name].ToHex()}>{name}</color>";
            }
            Debug.Log($"{name}: {content}");
        }

        public static void Print(Object obj, string content)
        {
            string objName;
            MonoBehaviour own = obj as MonoBehaviour;
            if (own != null)
            {
                objName = own.name;
            }
            else
            {
                objName = obj.GetType().Name;
            }
            Debug.Log($"{objName}: {content}");
        }
    }
}