using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RimeFramework.Tool;
using UnityEngine;

namespace RimeFramework.Core
{
    public class Animators : Singleton<Animators>
    {
        private static Dictionary<Animator, Coroutine> _dicPlays = new ();
        
        public static void Play(Animator animator,float fadeTime,Action cbDone,Action cbFail, params string[] newAnimations)
        {
            _dicPlays[animator] = Instance.StartCoroutine(Play(animator, newAnimations[0], fadeTime, () =>
            {
                if (newAnimations.Length == 1)
                {
                    cbDone?.Invoke();
                    return;
                }

                newAnimations = newAnimations.Skip(1).ToArray();
                Play(animator,fadeTime,cbDone,cbFail, newAnimations);
            },cbFail));
        }

        private static IEnumerator Play(Animator animator,string name,float fadeTime = 0,Action cbDone = null,Action cbFail = null)
        {
            yield return null;
            Coroutine coroutine = _dicPlays[animator];
            animator.CrossFade(name,fadeTime);
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
        }
    }
}