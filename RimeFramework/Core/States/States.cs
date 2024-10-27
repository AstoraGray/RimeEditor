using System;
using System.Collections.Generic;
using RimeFramework.Tool;
using UnityEngine;

namespace RimeFramework.Core
{
    /// <summary>
    /// 霜 · 状态机 📐
    /// <b>Note: 为什么使用静态方法，但又是单例？
    /// 这样可以在融入Unity生命周期的同时，保持一些配置不受生命周期的影响，比如后续可以给States设置一些Awake()初始化方法</b>
    /// <see cref="Register(MonoBehaviour, enabled)"/> 注册状态
    /// </summary>
    /// <remarks>Author: AstoraGray</remarks>
    public class States : Singleton<States>
    {
        private static Dictionary<MonoBehaviour, Dictionary<Type, State>> _dicStates = new (); // 字典

        private const string STATES_NAME = "States"; // 状态节点名字
        
        public Dictionary<Type, State> this[MonoBehaviour monoBehaviour]
        {
            get
            {
                if (_dicStates.TryGetValue(monoBehaviour, out var states))
                {
                    return states;
                }
                Consoles.Print(typeof(States),"状态不存在");
                return null; // 或者抛出异常，根据需求
            }
        }
        
        /// <summary>
        /// 注册状态行为
        /// </summary>
        /// <param name="own">持有者</param>
        /// <param name="enable">激活状态</param>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public static T Register<T>(MonoBehaviour own,bool enabled = true) where T : State
        {
            Transform actions = null;
            // 查找是否存在key
            if (!_dicStates.ContainsKey(own))
            {
                // 查找是否已有节点
                if (!own.transform.Find(STATES_NAME))
                {
                    actions = new GameObject(STATES_NAME).transform;
                    actions.SetParent(own.transform);
                }
                _dicStates.Add(own,new Dictionary<Type, State>());
            }
            State newState = null;
            bool isWake = false;
            if (!_dicStates[own].ContainsKey(typeof(T)))
            {
                actions ??= own.transform.Find(STATES_NAME);
                newState = actions.GetComponent<T>() ?? actions.gameObject.AddComponent<T>();
                _dicStates[own].Add(typeof(T),newState);
                isWake = true;
            }
            newState ??= _dicStates[own][typeof(T)];
            newState.enabled = enabled;
            newState.own = own;
            if (isWake)
            {
                newState.WakeUp();
            }
            newState.On();
            return newState as T;
        }
        
        /// <summary>
        /// 注销状态行为
        /// </summary>
        /// <param name="state">状态行为</param>
        public static void UnRegister(State state)
        {
            if (state == null)
            {
                Consoles.Print(typeof(States),"注销目标不存在");
                return;
            }
            _dicStates[state.own].Remove(state.GetType());
            if (_dicStates[state.own].Count <= 0)
            {
                _dicStates.Remove(state.own);
            }
            Destroy(state);
        }
    }
}