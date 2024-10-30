using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RimeFramework.Tool;
using UnityEngine;

namespace RimeFramework.Core
{
    /// <summary>
    /// 霜 · 动画师 ✍️
    /// </summary>
    /// <b> Note: 注册播放动画以及动画组、支持成功失败回调，可以脱离Animator连线
    /// <remarks>Author: AstoraGray</remarks>
    public class Animators : Singleton<Animators>
    {
        private static readonly Dictionary<string, string[]> _dicGroups = new(); // 动画组
        
        private static readonly Dictionary<Animator, Coroutine> _dicPlays = new (); // 播放组

        private const string GROUP = "group|"; // 烙印

        /// <summary>
        /// 注册组
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <param name="animations">序列动画名</param>
        public static void Register(string groupName,params string[] animations)
        {
            _dicGroups[$"{GROUP}{groupName}"] = animations;
        }

        /// <summary>
        /// 播放 动画/动画组,组优先级 > 动画
        /// </summary>
        /// <param name="animator">动画</param>
        /// <param name="name">动画组名 / 动画名</param>
        /// <param name="fadeTime"> 渐变时间</param>
        /// <param name="cbDone">成功回调</param>
        /// <param name="cbFail">失败回调</param>
        public static void Play(Animator animator,string name,float fadeTime = 0.3f,Action cbDone = null,Action cbFail = null)
        {
            if (!_dicGroups.TryGetValue($"{GROUP}{name}", out string[] animations))
            {
                _dicPlays[animator] = Instance.StartCoroutine(Playing(animator,name,fadeTime, cbDone, cbFail));
                return;
            }
            Play(animator,animations,fadeTime,cbDone,cbFail);
        }
        /// <summary>
        /// 播放动画组
        /// </summary>
        /// <param name="animator">动画</param>
        /// <param name="animations">动画序列</param>
        /// <param name="fadeTime">渐变时间</param>
        /// <param name="cbDone">成功回调</param>
        /// <param name="cbFail">失败回调</param>
        private static void Play(Animator animator,string[] animations,float fadeTime = 0.3f,Action cbDone = null,Action cbFail = null)
        {
            _dicPlays[animator] = Instance.StartCoroutine(Playing(animator,animations[0], fadeTime, () =>
            {
                if (animations.Length == 1)
                {
                    cbDone?.Invoke();
                    return;
                }

                animations = animations.Skip(1).ToArray();
                Play(animator,animations,fadeTime,cbDone,cbFail);
            },cbFail));
        }
        /// <summary>
        /// 播放协程
        /// </summary>
        /// <param name="animator">动画</param>
        /// <param name="animations">动画序列</param>
        /// <param name="fadeTime">渐变时间</param>
        /// <param name="cbDone">成功回调</param>
        /// <param name="cbFail">失败回调</param>
        /// <returns></returns>
        private static IEnumerator Playing(Animator animator,string name,float fadeTime,Action cbDone,Action cbFail)
        {
            yield return null;
            Coroutine coroutine = _dicPlays[animator];
            animator.CrossFadeInFixedTime(name,fadeTime);
            float length = animator.GetCurrentAnimatorClipInfo(0).Length - fadeTime;
            float startTime = Time.time;
            while (Time.time < startTime + length)
            {
                if (_dicPlays[animator] != coroutine)
                {
                    cbFail?.Invoke();
                    yield break;
                }
                yield return Time.deltaTime;
            }
            cbDone?.Invoke();
            if (_dicPlays[animator] == coroutine)
            {
                _dicPlays[animator] = null;
            }
        }
    }
}