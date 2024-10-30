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
        private static readonly Dictionary<Type, Color> _dicMapping = new()
        {
            { typeof(RimeManager), Color.cyan },
            { typeof(States), Color.cyan },
            { typeof(Controls), Color.cyan },
            { typeof(Cycles), Color.cyan },
            { typeof(Pools), Color.cyan },
            { typeof(Navigations),Color.cyan},
            { typeof(Scenes), Color.cyan },
            { typeof(Animators),Color.cyan}
        };
        
        public static void Print(Type type,string content)
        {
            string typeName = type.ToString().Split('.').Last();
            if (_dicMapping.ContainsKey(type))
            {
                typeName = $"<color={_dicMapping[type].ToHex()}>{typeName}</color>";
            }
            Debug.Log($"{typeName}: {content}");
        }

        public static void Print(Object obj, string content)
        {
            string objName = obj.ToString().Split('.').Last();
            Debug.Log($"{objName}: {content}");
        }
    }
}