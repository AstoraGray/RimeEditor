using UnityEngine;

namespace RimeFramework.Core
{
    public interface IBind
    {
        
    }
    public interface IAwake : IBind
    {
        public void Awake();
    }
    
    public interface IOnEnable : IBind
    {
        public void OnEnable(GameObject obj);
    }

    public interface IStart : IBind
    {
        public void Start();
    }
    
    public interface IUpdate : IBind
    {
        public void Update();
    }
    
    public interface IFixedUpdate : IBind
    {
        public void FixedUpdate();
    }

    public interface ILateUpdate : IBind
    {
        public void LateUpdate();
    }

    public interface IOnDisable : IBind
    {
        public void OnDisable();
    }

    public interface IOnDestroy : IBind
    {
        public void OnDestroy();
    }
}