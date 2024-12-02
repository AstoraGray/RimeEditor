using System;
using System.Collections.Generic;
using RimeFramework.Tool;

namespace RimeFramework.Core
{
    /// <summary>
    /// 霜 · 观察者 📷
    /// </summary>
    /// <b> Note: 支持有参无参事件的管理
    /// <remarks>Author: AstoraGray</remarks>
    public class Observers : Singleton<Observers>
    {
        private static readonly Dictionary<string, Action<object[]>> _dicEventObserversParams = new(); // 事件观察者

        private static readonly Dictionary<string, Action> _dicEventObservers = new(); // 事件观察者（无参）
        
        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="name">事件名</param>
        /// <param name="onComplete">事件</param>
        public static void Register(string name,Action<object[]> onComplete)
        {
            if (!_dicEventObserversParams.ContainsKey(name))
            {
                _dicEventObserversParams[name] = onComplete;
                return;
            }
            _dicEventObserversParams[name] += onComplete;
        }
        
        /// <summary>
        /// 注册事件（无参）
        /// </summary>
        /// <param name="name">事件名</param>
        /// <param name="onComplete">事件</param>
        public static void Register(string name,Action onComplete)
        {
            if (!_dicEventObservers.ContainsKey(name))
            {
                _dicEventObservers[name] = onComplete;
                return;
            }
            _dicEventObservers[name] += onComplete;
        }

        /// <summary>
        /// 注销事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="onComplete"></param>
        public static void UnRegister(string name, Action<object[]> onComplete)
        {
            if (!_dicEventObserversParams.ContainsKey(name))
            {
                Consoles.Print(nameof(Observers),$"事件{name}不存在，因为它没有被注册");
                return;
            }
            
            _dicEventObserversParams[name] -= onComplete;
        }
        
        /// <summary>
        /// 注销事件（无参）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="onComplete"></param>
        public static void UnRegister(string name, Action onComplete)
        {
            if (!_dicEventObservers.ContainsKey(name))
            {
                Consoles.Print(nameof(Observers),$"事件{name}不存在，因为它没有被注册");
                return;
            }
            
            _dicEventObservers[name] -= onComplete;
        }

        /// <summary>
        /// 广播事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        public static void Dispatch(string name,params object[] parameters)
        {
            if (_dicEventObserversParams.TryGetValue(name, out Action<object[]> onComplete1))
            {
                onComplete1?.Invoke(parameters);
            }
            if (_dicEventObservers.TryGetValue(name, out Action onComplete2))
            {
                onComplete2?.Invoke();
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public static void Clear()
        {
            _dicEventObserversParams.Clear();
            _dicEventObservers.Clear();
        }
    }
}