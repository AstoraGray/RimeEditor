using UnityEngine;

namespace RimeFramework.Core
{
    /// <summary>
    /// 绑定接口
    /// </summary>
    /// <remarks>Author: AstoraGray</remarks>
    public interface IBind
    {
        
    }
    /// <summary>
    /// Unity · Awake
    /// </summary>
    public interface IAwake : IBind
    {
        public void Awake();
    }
    /// <summary>
    /// Unity · OnEnable
    /// </summary>
    public interface IOnEnable : IBind
    {
        public void OnEnable(GameObject obj);
    }
    /// <summary>
    /// Unity · Start
    /// </summary>
    public interface IStart : IBind
    {
        public void Start();
    }
    /// <summary>
    /// Unity · Update
    /// </summary>
    public interface IUpdate : IBind
    {
        public void Update();
    }
    /// <summary>
    /// Unity · FixedUpdate
    /// </summary>
    public interface IFixedUpdate : IBind
    {
        public void FixedUpdate();
    }
    /// <summary>
    /// Unity · LateUpdate
    /// </summary>
    public interface ILateUpdate : IBind
    {
        public void LateUpdate();
    }
    /// <summary>
    /// Unity · OnDisable
    /// </summary>
    public interface IOnDisable : IBind
    {
        public void OnDisable();
    }
    /// <summary>
    /// Unity · OnDestroy
    /// </summary>
    public interface IOnDestroy : IBind
    {
        public void OnDestroy();
    }
}