using UnityEngine;

namespace RimeFramework.Tool
{
    /// <summary>
    /// 单例类
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <remarks>Author: AstoraGray</remarks>
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;

        protected virtual void Awake()
        {
            if (_instance != null && this != null)
            {
                Destroy(this);
                return;
            }

            _instance = this as T;
            DontDestroyOnLoad(_instance);
        }

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
    }
}