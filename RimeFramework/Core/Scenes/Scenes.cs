using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RimeFramework.Tool;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RimeFramework.Core
{
    /// <summary>
    /// 霜 · 布景员 🎬
    /// </summary>
    /// <b> Note: 负责加载和卸载场景，支持完成回调、取消回调
    /// <remarks>Author: AstoraGray</remarks>
    public class Scenes : Singleton<Scenes>
    {
        private static readonly List<string> _scenes = new (); // 已加载场景
        
        private static readonly Dictionary<string,Coroutine> _dicCoroutines = new (); // 进行中协程
        
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="name">场景名</param>
        /// <param name="mode">加载模式</param>
        /// <param name="onUpdate">更新回调</param>
        /// <param name="onComplete">完成回调</param>
        public static void Load(string name,LoadSceneMode mode,Action<float> onUpdate = null,Action onComplete = null)
        {
            if (_scenes.Contains(name))
            {
                Consoles.Print(typeof(Scenes),$"场景列表中已包含场景 {name}，不可再次读取");
                return;
            }
            if (mode == LoadSceneMode.Single)
            {
                _scenes.Clear();
            }
            _scenes.Add(name);
            
            _dicCoroutines[name] = Instance.StartCoroutine(Loading(name,mode,onUpdate,onComplete));
        }
        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="name">场景名</param>
        /// <param name="onUpdate">更新回调</param>
        /// <param name="onComplete">完成回调</param>
        public static void Unload(string name = null,Action<float> onUpdate = null,Action onComplete = null)
        {
            if (!_scenes.Contains(name))
            {
                Consoles.Print(typeof(Scenes),$"场景列表中不包含场景 {name}，不可卸载");
                return;
            }
            if (name == null)
            {
                name = _scenes.Last();
                _scenes.RemoveAt(_scenes.Count - 1);
            }
            else
            {
                _scenes.Remove(name);
            }
            _dicCoroutines[name] = Instance.StartCoroutine(Unloading(name, onUpdate, onComplete));
        }
        /// <summary>
        /// 加载协程
        /// </summary>
        /// <param name="name">场景名</param>
        /// <param name="mode">加载模式</param>
        /// <param name="onUpdate">更新回调</param>
        /// <param name="onComplete">完成回调</param>
        /// <returns></returns>
        private static IEnumerator Loading(string name,LoadSceneMode mode,Action<float> onUpdate,Action onComplete)
        {
            yield return null;
            Coroutine coroutine = _dicCoroutines[name];
            AsyncOperation scene = SceneManager.LoadSceneAsync(name, mode);
            
            while (!scene.isDone)
            {
                if (_dicCoroutines[name] != coroutine)
                {
                    yield break;
                }
                onUpdate?.Invoke(scene.progress);
                yield return null;
            }
            
            if (_dicCoroutines[name] == coroutine)
            {
                _dicCoroutines[name] = null;
            }
            
            onComplete?.Invoke();
        }
        /// <summary>
        /// 卸载协程
        /// </summary>
        /// <param name="name">场景名</param>
        /// <param name="onUpdate">更新回调</param>
        /// <param name="onComplete">完成回调</param>
        /// <returns></returns>
        private static IEnumerator Unloading(string name,Action<float> onUpdate,Action onComplete)
        {
            yield return null;
            Coroutine coroutine = _dicCoroutines[name];
            AsyncOperation scene = SceneManager.UnloadSceneAsync(name);
            
            while (!scene.isDone)
            {
                if (_dicCoroutines[name] != coroutine)
                {
                    yield break;
                }
                onUpdate?.Invoke(scene.progress);
                yield return null;
            }

            if (_dicCoroutines[name] == coroutine)
            {
                _dicCoroutines[name] = null;
            }
            onComplete?.Invoke();
        }
    }
}