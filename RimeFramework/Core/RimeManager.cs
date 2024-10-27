using System;
using RimeFramework.Tool;

namespace RimeFramework.Core
{
    /// <summary>
    /// 最上层，游戏管理器
    /// </summary>
    public class RimeManager : Singleton<RimeManager>
    {
        public bool consoles = true;
        public bool controls = true;
        public bool states = true;
        public bool cycles = true;
        public bool pools = true;
        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            Consoles.Print(GetType(),$"{Environment.UserName}, 欢迎回来!");
            if(consoles) Consoles.Instance.transform.SetParent(transform);
            if(controls) Controls.Instance.transform.SetParent(transform);
            if(states) States.Instance.transform.SetParent(transform);
            if(cycles) Cycles.Instance.transform.SetParent(transform);
            if(pools) Pools.Instance.transform.SetParent(transform);
        }
    }
}