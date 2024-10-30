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
        /// <summary>
        /// 所有可能的动画播放报错
        /// </summary>
        public enum AnimationError
        {
            OK,
            ALREADY_PLAYING,
            DOES_NOT_EXIST
        }

        /// <summary>
        /// 在指定的层上播放具有给定名称的动画，并从给定的开始时间开始，
        /// 前提是动画尚未播放且在指定的层和动画控制器上确实存在。
        /// </summary>
        /// <param name="animator">希望在其上调用动画的 Animator。</param>
        /// <param name="newAnimation">希望播放的动画名称。</param>
        /// <param name="delayTime">想要开始动画的延迟时间。</param>
        /// <param name="startTime">我们想要从何处开始动画的时间。</param>
        /// <param name="layer">找到给定动画名称的层。</param>
        /// <returns>AnimationError，显示播放文件是否失败以及失败的原因。</returns>
        public static AnimationError Play(Animator animator, string newAnimation,Action cbDone = null, float delayTime = 0.0f,
            float startTime = 0.0f, int layer = 0)
        {
            AnimationError message = AnimationError.OK;

            // 获取基础层的当前动画剪辑信息。
            List<AnimatorClipInfo> currentClipInfoList = new List<AnimatorClipInfo>();
            animator.GetCurrentAnimatorClipInfo(layer, currentClipInfoList);
            
            if (!Exists(animator, newAnimation))
            {
                message = AnimationError.DOES_NOT_EXIST;
            }
            // 在给定的 startTime 播放 newAnimation。
            else
            {
                Instance.PlayCoroutine(animator, newAnimation, delayTime, startTime, layer,cbDone);
            }

            return message;
        }

        /// <summary>
        /// 检查给定的 animator 是否实际包含给定的 animationClip。
        /// </summary>
        /// <param name="animator">Animator 组件，用于获取所有动画剪辑。</param>
        /// <param name="newAnimation">我们想要检查的动画名称。</param>
        /// <returns>给定的 animator 中是否有任何剪辑具有给定的名称。</returns>
        private static bool Exists(Animator animator, string newAnimation)
        {
            // 从给定的 Animator 获取所有动画。
            AnimationClip[] allclips = animator.runtimeAnimatorController.animationClips;

            // 检查 Animator 中的任何动画是否具有给定的剪辑名称。 
            return allclips.Any(animation => string.Equals(animation.name, newAnimation));
        }

        /// <summary>
        /// 获取在给定层上当前正在播放的动画的长度。
        /// </summary>
        /// <param name="animator">Animator 组件，用于获取所有动画剪辑。</param>
        /// <param name="layer">我们找到当前正在播放的动画的层。</param>
        /// <returns>等于剪辑长度的浮点数。</returns>
        private static float GetCurAniLength(Animator animator, int layer = 0)
        {
            // 获取基础层的当前动画剪辑信息。
            AnimatorClipInfo[] currentClipInfo = animator.GetCurrentAnimatorClipInfo(layer);

            // 访问当前动画剪辑长度。
            return currentClipInfo[0].clip.length;
        }

        private void PlayCoroutine(Animator animator, string newAnimation, float delayTime, float startTime, int layer,Action cbDone = null)
        {
            StartCoroutine(Play(animator, newAnimation, delayTime, startTime, layer, cbDone));
        }
        
        private IEnumerator Play(Animator animator, string newAnimation, float delayTime, float startTime, int layer,Action cbDone = null)
        {
            yield return new WaitForSeconds(delayTime);
            animator.Play(newAnimation, layer, startTime);
            yield return new WaitForSeconds(GetCurAniLength(animator) - startTime);
            cbDone?.Invoke();
        }
    }
}