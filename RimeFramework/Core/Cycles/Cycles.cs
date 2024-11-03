using System.Collections.Generic;
using JetBrains.Annotations;
using RimeFramework.Tool;
using RimeFramework.Utility;
using UnityEngine;
using Object = System.Object;

namespace RimeFramework.Core
{
    /// <summary>
    /// 霜 · 周期 🕙
    /// </summary>
    /// <b> Note: 控制所有非Mono类的生命进程
    /// <see cref="Register(Object)"/> 挂载对象
    /// <remarks>Author: AstoraGray</remarks>
    public class Cycles : Singleton<Cycles>
    {
        private static readonly Dictionary<string,Cell> _dicCells = new (); // 工作的细胞们

        private static readonly Dictionary<string, Cell> _dicCellsReady = new(); // 就绪的细胞们

        private static readonly Queue<string> _queueRecycle = new (); // 要回收的细胞列表
        
        private static readonly Queue<string> _queueRecycleReady = new (); // 准备回收的细胞列表
        /// <summary>
        /// 细胞（ps：就像细胞一般的周期）
        /// </summary>
        private struct Cell
        {
            [CanBeNull] public IAwake awake;

            [CanBeNull] public IOnEnable onEnable;

            [CanBeNull] public IStart start;

            [CanBeNull] public IUpdate update;

            [CanBeNull] public IFixedUpdate fixedUpdate;

            [CanBeNull] public ILateUpdate lateUpdate;

            [CanBeNull] public IOnDisable onDisable;

            [CanBeNull] public IOnDestroy onDestroy;
        }
        /// <summary>
        /// 注册周期
        /// </summary>
        /// <param name="obj">绑定目标</param>
        /// <typeparam name="T">周期类类型</typeparam>
        /// <returns></returns>
        public static T Register<T>(GameObject obj) where T : class,IBind, new()
        {
             return Register(Pools.OnTake<T>(),obj);
        }
        /// <summary>
        /// 注册周期
        /// </summary>
        /// <param name="own">自身实例</param>
        /// <param name="obj">绑定目标</param>
        /// <typeparam name="T">周期类类型</typeparam>
        /// <returns></returns>
        private static T Register<T>(T own,GameObject obj) where T : class,IBind, new()
        {
            if (own as MonoBehaviour != null)
            {
                Consoles.Print(typeof(Cycles),$"不允许Mono派生类加入周期");
                return null;
            }

            Cell cell = new Cell();
            
            if (own is IAwake awake)
            {
                cell.awake = awake;
            }

            if (own is IOnEnable onEnable)
            {
                cell.onEnable = onEnable;
            }
            
            if (own is IStart start)
            {
                cell.start = start;
            }

            if (own is IUpdate update)
            {
                cell.update = update;
            }

            if (own is IFixedUpdate fixedUpdate)
            {
                cell.fixedUpdate = fixedUpdate;
            }

            if (own is ILateUpdate lateUpdate)
            {
                cell.lateUpdate = lateUpdate;
            }

            if (own is IOnDisable onDisable)
            {
                cell.onDisable = onDisable;
            }

            if (own is IOnDestroy onDestroy)
            {
                cell.onDestroy = onDestroy;
            }
            
            _dicCellsReady.Add(own.GetKey(),cell);

            cell.onEnable?.OnEnable(obj);

            return own;
        }
        /// <summary>
        /// 注销周期
        /// </summary>
        /// <param name="own">自身实例</param>
        /// <typeparam name="T">周期类类型</typeparam>
        /// <returns></returns>
        public static bool UnRegister<T>(T own) where T : class,IBind, new()
        {
            if (own == null)
            {
                Consoles.Print(typeof(Cycles),$"注销物体为空");
                return false;
            }

            string key = own.GetKey();
            
            if (_dicCells.ContainsKey(key))
            {
                _queueRecycle.Enqueue(key);
                return Pools.OnPut(own);
            }

            if (_dicCellsReady.ContainsKey(key))
            {
                _queueRecycleReady.Enqueue(key);
                return Pools.OnPut(own);;
            }

            return false;
        }
        /// <summary>
        /// Unity - Update()
        /// </summary>
        private void Update()
        {
            Recycle();
            
            foreach (var dicCell in _dicCells)
            {
                dicCell.Value.update?.Update();
            }
        }
        /// <summary>
        /// Unity - FixedUpdate()
        /// </summary>
        private void FixedUpdate()
        {
            Recycle();
            
            foreach (var dicCell in _dicCells)
            {
                dicCell.Value.fixedUpdate?.FixedUpdate();
            }
        }
        /// <summary>
        /// Unity - LateUpdate()
        /// </summary>
        private void LateUpdate()
        {
            Recycle();
            
            foreach (var dicCell in _dicCells)
            {
                dicCell.Value.lateUpdate?.LateUpdate();
            }
            
            foreach (var pair in _dicCellsReady)
            {
                _dicCells[pair.Key] = pair.Value;
            }
            
            _dicCellsReady.Clear();
        }
        /// <summary>
        /// 回收函数
        /// </summary>
        private void Recycle()
        {
            while (_queueRecycle.Count > 0)
            {
                string key = _queueRecycle.Dequeue();
                _dicCells[key].onDisable?.OnDisable();
                _dicCells.Remove(key);
            }
            
            while (_queueRecycleReady.Count > 0)
            {
                string key = _queueRecycleReady.Dequeue();
                _dicCellsReady[key].onDisable?.OnDisable();
                _dicCellsReady.Remove(key);
            }
        }
    }
}