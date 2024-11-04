using System;
using UnityEngine;

namespace RimeFramework.Core
{
    /// <summary>
    /// 霜 · 状态类
    /// <b> WakeUp(): 请将初始化状态要做的事写入这个方法
    /// <b> On(): 将进入状态，或者说激活状态要做的事写入这个方法
    /// <b> Off(): 将离开状态，或者说取消激活状态要做的事写入这个方法
    /// <b> 生命周期：WakeUp->On->Update->Off
    /// <see cref="own.value"/> 建议使用直接访问的方式监听连续输入,eg: InputDir
    /// <see cref="Action += Method"/> 建议使用Action来监听离散输入，eg: Jump
    /// </summary>
    public abstract class State : MonoBehaviour
    {
        public MonoBehaviour own;
        
        /// <summary>
        /// 首次唤醒,代替Awake
        /// </summary>
        public virtual void WakeUp(){}

        /// <summary>
        /// 进入此状态,代替OnEnable
        /// </summary>
        public virtual void On(){}
        
        /// <summary>
        /// 离开此状态,代替OnDisAble
        /// </summary>
        public virtual void Off(){}
        
        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="exit">是否离开本状态</param>
        /// <param name="enabled">是否激活目标状态</param>
        /// <typeparam name="T">状态类型</typeparam>
        /// <returns></returns>
        protected virtual T Enter<T>(bool exit = true,bool enabled = true) where T : State
        {
            if (exit)
            {
                Exit();
            }
            return States.Register<T>(own, enabled);
        }
        /// <summary>
        /// 离开状态
        /// </summary>
        protected virtual void Exit()
        {
            enabled = false;
        }
        /// <summary>
        /// 隐藏Awake
        /// </summary>
        private void Awake(){}

        /// <summary>
        /// 隐藏Start
        /// </summary>
        private void Start(){}
        
        /// <summary>
        /// 隐藏OnEnable
        /// </summary>
        private void OnEnable() { }

        /// <summary>
        /// 取消激活
        /// </summary>
        private void OnDisable()
        {
            Off();
        }
        /// <summary>
        /// 销毁时取消注册
        /// </summary>
        protected virtual void OnDestroy()
        {
            States.UnRegister(this);
        }
    }
}