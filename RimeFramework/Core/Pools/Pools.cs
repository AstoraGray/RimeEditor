using System;
using System.Collections.Generic;
using System.Linq;
using RimeFramework.Tool;
using RimeFramework.Utility;
using UnityEngine;

namespace RimeFramework.Core
{
    /// <summary>
    /// 霜 · 池 💧
    /// </summary>
    /// <b> Note: 存取所有堆类型数据，包括Mono派生类、以及提线木偶、纯GameObject
    /// <remarks>Author: AstoraGray</remarks>
    public class Pools : Singleton<Pools>
    {
        private static readonly Dictionary<Type, Queue<object>> _dicDrips = new (); // Type水滴

        private static readonly Dictionary<Type, HashSet<object>> _dicOuterDrips = new(); // Type外界水滴

        private static readonly Dictionary<Type, GameObject> _dicWells = new(); // Type井

        private static readonly Dictionary<Type, GameObject> _dicWares = new(); // Type仓库
        
        private static readonly Dictionary<string, Queue<GameObject>> _dicObjDrips = new(); // Name水滴

        private static readonly Dictionary<string, HashSet<GameObject>> _dicObjOuterDrips = new(); // Name外界水滴

        private static readonly Dictionary<string, GameObject> _dicObjWells = new(); // Name井

        private static readonly Dictionary<string, GameObject> _dicObjWares = new(); // Name仓库
        
        private static readonly Queue<GameObject> _queueDestroy = new(); // 清理队列

        private static GameObject _objWell; // Type井根结点

        private static GameObject _objObjWell; // Name井根结点

        private const string WELL = "Well"; // Type井名称

        private const string OBJ_WELL = "ObjWell"; // Name井名称

        private const string RIME = "|RIME|"; // ｜烙印｜

        /// <summary>
        /// 索要 - TYPE
        /// </summary>
        /// <typeparam name="T">水滴类型</typeparam>
        /// <returns>水滴实例</returns>
        public static T OnTake<T>() where T : class,new ()
        {
            Type type = typeof(T);
            if (!_dicDrips.ContainsKey(type))
            {
                _dicDrips[type] = new Queue<object>();
            }

            Queue<object> queue = _dicDrips[type];
            
            if (queue.Count == 0)
            {
                return TakeMono<T>(type);
            }

            return TakeMono((T)queue.Dequeue(),type);
        }
        /// <summary>
        /// 索要 - NAME
        /// </summary>
        /// <param name="name">水滴名称</param>
        /// <returns>水滴实例 - 特化GameObject</returns>
        public static GameObject OnTake(string name)
        {
            if (!_dicObjDrips.ContainsKey(name))
            {
                _dicObjDrips[name] = new Queue<GameObject>();
            }

            Queue<GameObject> queue = _dicObjDrips[name];

            if (queue.Count == 0)
            {
                return TakeObj(name);
            }

            return TakeObj(name,queue.Dequeue());
        }

        /// <summary>
        /// 归放 - TYPE
        /// </summary>
        /// <param name="drip">水滴实例</param>
        /// <typeparam name="T">水滴类型</typeparam>
        public static bool OnPut<T>(T drip) where T : class
        {
            Type type = typeof(T);
            if (!_dicDrips.ContainsKey(type))
            {
                Consoles.Print(typeof(Pools),$"归放过程未发现池 {type}");
                return false;
            }
            
            Queue<object> queue = _dicDrips[type];

            _dicOuterDrips[type].Remove(drip);
            queue.Enqueue(drip);

            return PutMono(drip,type);
        }
        /// <summary>
        /// 归放 - GAMEOBJECT
        /// </summary>
        /// <param name="obj">水滴实例 - 特化GameObject</param>
        /// <returns></returns>
        public static bool OnPut(GameObject obj)
        {
            string name = obj.name.Extract(RIME);
            if (!_dicObjDrips.ContainsKey(name))
            {
                Consoles.Print(typeof(Pools),$"归放过程未发现池 {name}");
                return false;
            }
            
            Queue<GameObject> queue = _dicObjDrips[name];

            _dicObjOuterDrips[name].Remove(obj);
            queue.Enqueue(obj);

            return OnPutObj(obj,name);
        }

        /// <summary>
        /// 清洗 - TYPE
        /// </summary>
        /// <typeparam name="T">水滴类型</typeparam>
        /// <returns>是否成功</returns>
        public static bool OnClear<T>() where T : class
        {
            Type type = typeof(T);
            int clearCount = 0;
            foreach (var key in _dicDrips.Keys.ToList())
            {
                if (key == typeof(T) || key.IsSubclassOf(typeof(T)))
                {
                    ClearMono<T>(key);
                    clearCount++;
                }
            }

            if (clearCount > 0)
            {
                return true;
            }
            Consoles.Print(typeof(Pools),$"清洗过程在{WELL}未发现池 {type}");
            return false;
        }
        
        /// <summary>
        /// 清洗 - NAME
        /// </summary>
        /// <param name="name">水滴名字 - 特化GameObject</param>
        /// <returns></returns>
        public static bool OnClear(string name)
        {
            if (!_dicObjDrips.ContainsKey(name))
            {
                Consoles.Print(typeof(Pools),$"清洗过程在{OBJ_WELL}中未发现池 {name}");
                return false;
            }

            return ClearObj(name);
        }
        /// <summary>
        /// 索要Mono
        /// </summary>
        /// <param name="type">水滴类型</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static T TakeMono<T>(Type type) where T : class,new ()
        {
            if (!type.IsSubclassOf(typeof(MonoBehaviour)))
            {
                T t = new T();
                (t as IAwake)?.Awake();
                (t as IStart)?.Start();
                return TakeMono(new T(), type);
            }

            bool inWares = _dicWares.ContainsKey(type);
            bool haveIPools = type.GetInterface(typeof(IPool).ToString()) != null;
            
            if (!inWares && !haveIPools)
            {
                GameObject obj1 = new GameObject(type.Name);
                T mono1 =  obj1.AddComponent(type) as T;
                return TakeMono(mono1,type);
            }

            if (!inWares && haveIPools)
            {
                _dicWares[type] = Resources.Load<GameObject>($"Prefabs/{WELL}/{type.Name}");
                if (_dicWares[type] == null)
                {
                    Consoles.Print(typeof(Pools),$"Prefabs/{WELL}中未发现预制体{type.Name}");
                    return null;
                }
            }

            GameObject obj2 = Instantiate(_dicWares[type]);
            T mono2 =  obj2.GetComponent(type) as T ?? obj2.AddComponent(type) as T;
            
            return TakeMono(mono2,type);
        }
        /// <summary>
        /// 索要Mono
        /// </summary>
        /// <param name="drip">水滴实例</param>
        /// <param name="type">水滴类型</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static T TakeMono<T>(T drip,Type type) where T : class,new ()
        {
            if (!_dicOuterDrips.ContainsKey(type))
            {
                _dicOuterDrips[type] = new HashSet<object>();
            }

            HashSet<object> hashSet = _dicOuterDrips[type];
            hashSet.Add(drip);
            
            MonoBehaviour mono = drip as MonoBehaviour;
            if (mono == null)
            {
                return drip;
            }

            if (_objWell == null)
            {
                _objWell = new GameObject(WELL);
                _objWell.transform.SetParent(Instance.transform);
            }

            if (!_dicWells.ContainsKey(type))
            {
                _dicWells[type] = new GameObject(type.ToString());
                _dicWells[type].transform.SetParent(_objWell.transform);
            }

            mono.gameObject.name = mono.GetKey();
            mono.transform.SetParent(_dicWells[type].transform);
            mono.gameObject.SetActive(true);
            return drip;
        }
        /// <summary>
        /// 索要Obj
        /// </summary>
        /// <param name="name">Obj名字</param>
        /// <returns></returns>
        private static GameObject TakeObj(string name)
        {
            _dicObjWares[name] = Resources.Load<GameObject>($"Prefabs/{OBJ_WELL}/{name}");
            if (_dicObjWares[name] == null)
            {
                Consoles.Print(typeof(Pools),$"Prefabs/{OBJ_WELL}中未发现预制体{name}");
                return null;
            }
            GameObject obj = Instantiate(_dicObjWares[name]);
            return TakeObj(name,obj);
        }
        /// <summary>
        /// 索要Obj
        /// </summary>
        /// <param name="name">Obj名字</param>
        /// <param name="obj">Obj实例</param>
        /// <returns></returns>
        private static GameObject TakeObj(string name,GameObject obj)
        {
            if (!_dicObjOuterDrips.ContainsKey(name))
            {
                _dicObjOuterDrips[name] = new HashSet<GameObject>();
            }

            HashSet<GameObject> hashSet = _dicObjOuterDrips[name];
            hashSet.Add(obj);

            if (_objObjWell == null)
            {
                _objObjWell = new GameObject(OBJ_WELL);
                _objObjWell.transform.SetParent(Instance.transform);
            }

            if (!_dicObjWells.ContainsKey(name))
            {
                _dicObjWells[name] = new GameObject(name);
                _dicObjWells[name].transform.SetParent(_objObjWell.transform);
            }

            obj.name = $"{obj.GetKey()}{RIME}{name}";
            obj.transform.SetParent(_dicObjWells[name].transform);
            obj.SetActive(true);
            return obj;
        }
        
        /// <summary>
        /// 归放Mono
        /// </summary>
        /// <param name="drip">水滴实例</param>
        /// <param name="type">水滴类型</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static bool PutMono<T>(T drip,Type type) where T : class
        {
            MonoBehaviour mono = drip as MonoBehaviour;
            if (mono == null)
            {
                return true;
            }

            if (mono.transform.parent != _dicWells[type].transform)
            {
                mono.transform.SetParent(_dicWells[type].transform);
            }
            
            mono.gameObject.SetActive(false);

            return true;
        }
        
        /// <summary>
        /// 归放Obj
        /// </summary>
        /// <param name="obj">Obj实例</param>
        /// <param name="name">Obj名字</param>
        /// <returns></returns>
        private static bool OnPutObj(GameObject obj, string name)
        {
            if (obj.transform.parent != _dicObjWells[name].transform)
            {
                obj.transform.SetParent(_dicObjWells[name].transform);
            }
            
            obj.gameObject.SetActive(false);

            return true;
        }

        /// <summary>
        /// 清洗Mono - TYPE
        /// </summary>
        /// <param name="type">水滴类型</param>
        /// <typeparam name="T">清洗</typeparam>
        /// <returns></returns>
        private static bool ClearMono<T>(Type type) where T : class
        {
            Consoles.Print(typeof(Pools),$"清洗类型 {type}");
            Queue<object> queueDrips = _dicDrips[type];
            HashSet<object> hashsetOuterDrips = _dicOuterDrips[type];

            foreach (var outerDrip in hashsetOuterDrips.ToList())
            {
                PutMono(outerDrip,type);
            }

            if (type.GetInterface(typeof(IOnDestroy).ToString()) != null)
            {
                while (queueDrips.Count > 0)
                {
                    (queueDrips.Dequeue() as IOnDestroy)?.OnDestroy();
                }
            }

            _dicOuterDrips[type] = null;
            _dicDrips[type] = null;
            
            if (!_dicWells.ContainsKey(type))
            {
                return true;
            }
            _queueDestroy.Enqueue(null);
            _queueDestroy.Enqueue(_dicWells[type]);
            _dicWells[type] = null;
            return true;
        }
        
        /// <summary>
        /// 清洗Obj - NAME
        /// </summary>
        /// <param name="name">水滴名字 - 特化GameObject</param>
        /// <returns></returns>
        private static bool ClearObj(string name)
        {
            Consoles.Print(typeof(Pools),$"清洗GameObject {name}");
            HashSet<GameObject> hashsetOuterDrips = _dicObjOuterDrips[name];
            
            foreach (var outerObjDrip in hashsetOuterDrips.ToList())
            {
                OnPut(outerObjDrip);
            }

            _queueDestroy.Enqueue(null);
            _queueDestroy.Enqueue(_dicObjWells[name]);
            _dicObjWells[name] = null;
            return true;
        }
        /// <summary>
        /// 延迟清洗水滴
        /// </summary>
        private void LateUpdate()
        {
            if (_queueDestroy.Count > 0)
            {
                GameObject obj = _queueDestroy.Dequeue();
                Destroy(obj);
            }
        }
    }
}