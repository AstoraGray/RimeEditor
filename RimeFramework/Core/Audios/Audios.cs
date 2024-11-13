using System;
using System.Collections;
using System.Collections.Generic;
using RimeFramework.Tool;
using UnityEngine;

namespace RimeFramework.Core
{
    /// <summary>
    /// 霜 · 音效师 🔊
    /// </summary>
    /// <b> 支持多通道播放声音、BGM淡入淡出、3D音效、循环播放、提供最简单的调用接口
    /// <see cref="PlayS3D(MonoBehaviour,string,PlayingCall,bool)"/> 播放3D声音，需要一个拥有者
    /// <see cref="PlayS3D(string,PlayingCall,bool)"/> 播放2D声音
    /// <see cref="PlaySUI(string,PlayingCall,bool)"/> 播放UI声音
    /// <see cref="PlayS3D(string)"/> 播放BGM，淡入淡出
    /// <remarks>Author: AstoraGray</remarks>
    [RequireComponent(typeof(AudioSource))]
    public class Audios : Singleton<Audios>
    {
        private static AudioSource _audioMusic; // BGM音轨

        public static readonly float musicVolume = 1; // BGM音量

        public static readonly float s2dVolume = 1; // 2D音量
        
        public static readonly float s3dVolume = 1; // 3D音量
        
        public static readonly float suiVolume = 1; // UI音量

        private static readonly Dictionary<string, AudioClip> _dicClips = new(); // 缓存音频

        private static Coroutine _coroutineMusicFade; // 淡入淡出协程

        private static AudioClip _clipCurMusic; // 当前BGM
        
        private const float FADE_TIME = 2f; // 淡入淡出时间（真实时间）

        private const string AUDIO_PATH = "Audios/"; // 音频文件路径
        
        private enum AudioType
        {
            S2D, // 2D声音（无距离、可多个）
            S3D, // 3D声音（有距离、可多个）
            SUI, // UI声音（无距离、可多个）
            Music // BGM（无距离、单一、淡入淡出）
        }

        private enum FadeType
        {
            In, // 淡入
            Out // 淡出
        }
        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _audioMusic = GetComponent<AudioSource>();
            _audioMusic.loop = true;
            _audioMusic.volume = 0f;
        }

        /// <summary>
        /// 播放3D声音
        /// </summary>
        public static void PlayS3D(MonoBehaviour own, string name,PlayingCall call = null,bool loop = false) => Play(own, name, AudioType.S3D,call,loop);
        /// <summary>
        /// 播放2D声音
        /// </summary>
        public static void PlayS2D(string name,PlayingCall call = null,bool loop = false) => Play(null, name, AudioType.S2D,call,loop);
        /// <summary>
        /// 播放UI声音
        /// </summary>
        public static void PlaySUI(string name,PlayingCall call = null,bool loop = false) => Play(null, name, AudioType.SUI,call,loop);
        /// <summary>
        /// 播放BGM
        /// </summary>
        public static void PlayMusic(string name) => Play(null, name, AudioType.Music,null,true);
        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="own">拥有者</param>
        /// <param name="name">音频名称</param>
        /// <param name="type">音频类型</param>
        /// <param name="call">播放回调</param>
        /// <param name="loop">是否循环</param>
        private static void Play(MonoBehaviour own, string name, AudioType type,PlayingCall call,bool loop)
        {
            AudioClip clip;
            if (!_dicClips.ContainsKey(name))
            {
                string path = $"{AUDIO_PATH}{name}";
                clip = Resources.Load<AudioClip>(path);
                if (clip == null)
                {
                    Consoles.Print(typeof(Audios),$"找不到音频文件{path}");
                    return;
                }
                _dicClips[name] = clip;
            }

            clip = _dicClips[name];
            
            AudioSource audio = null;
            switch (type)
            {
                case AudioType.S2D:
                    audio = Pools.Take<AudioSource>();
                    audio.clip = clip;
                    audio.volume = s2dVolume;
                    audio.spatialBlend = 0;
                    audio.loop = loop;
                    Instance.StartCoroutine(PlaySound(null, audio,call));
                    break;
                case AudioType.S3D:
                    if (own == null)
                    {
                        Consoles.Print(typeof(Audios),$"播放{name}声音失败，3D声音需要指定一个拥有者");
                        return;
                    }
                    audio = Pools.Take<AudioSource>();
                    audio.clip = clip;
                    audio.volume = s3dVolume;
                    audio.spatialBlend = 1;
                    audio.loop = loop;
                    Instance.StartCoroutine(PlaySound(own, audio,call));
                    break;
                case AudioType.SUI:
                    audio = Pools.Take<AudioSource>();
                    audio.clip = clip;
                    audio.volume = suiVolume;
                    audio.spatialBlend = 0;
                    audio.loop = loop;
                    Instance.StartCoroutine(PlaySound(null, audio,call));
                    break;
                case AudioType.Music:
                    if (clip == _clipCurMusic)
                    {
                        return;
                    }

                    _clipCurMusic = clip;
                    
                    if (_audioMusic.isPlaying)
                    {
                        Consoles.Print(typeof(Audios),$"开始淡出BGM {_audioMusic.name}");
                        _coroutineMusicFade = Instance.StartCoroutine(FadeMusic(_audioMusic.volume,0,FADE_TIME,FadeType.Out, () =>
                        {
                            _audioMusic.clip = clip;
                            Consoles.Print(typeof(Audios),$"开始淡入BGM {name}");
                            _coroutineMusicFade = Instance.StartCoroutine(FadeMusic(_audioMusic.volume,musicVolume,FADE_TIME,FadeType.In));
                        }));
                    }
                    else
                    {
                        _audioMusic.clip = clip;
                        Consoles.Print(typeof(Audios),$"开始淡入BGM {name}");
                        _coroutineMusicFade = Instance.StartCoroutine(FadeMusic(_audioMusic.volume,musicVolume,FADE_TIME,FadeType.In));
                    }
                    break;
            }
        }
        /// <summary>
        /// 淡入淡出BGM
        /// </summary>
        /// <param name="startVolume">开始音量</param>
        /// <param name="endVolume">结束音量</param>
        /// <param name="duration">持续时间</param>
        /// <param name="fadeType">淡入/淡出</param>
        /// <param name="onComplete">结束回调</param>
        /// <returns></returns>
        private static IEnumerator FadeMusic(float startVolume, float endVolume, float duration,FadeType fadeType, Action onComplete = null)
        {
            yield return null;
            Coroutine coroutine = _coroutineMusicFade;
            float startTime = Time.time;
            _audioMusic.volume = startVolume;
            
            if (fadeType == FadeType.In)
            {
                _audioMusic.Play();
            }

            while (Time.time < startTime + duration)
            {
                if (coroutine != _coroutineMusicFade)
                {
                    yield break;
                }
                _audioMusic.volume = Mathf.Lerp(startVolume, endVolume, 1 - (startTime + duration - Time.time) / duration);
                yield return null;
            }

            _audioMusic.volume = endVolume;

            if (fadeType == FadeType.Out)
            {
                _audioMusic.Stop();
            }
            
            onComplete?.Invoke();
        }
        /// <summary>
        /// 播放条件（可空）
        /// </summary>
        public delegate bool PlayingCall(MonoBehaviour own, string name);
        
        /// <summary>
        /// 播放声音协程
        /// </summary>
        /// <param name="own">拥有者</param>
        /// <param name="audio">音频</param>
        /// <param name="call">播放条件回调</param>
        /// <returns></returns>
        private static IEnumerator PlaySound(MonoBehaviour own, AudioSource audio, PlayingCall call)
        {
            audio.Play();
            float clipLength = audio.clip.length;
            float endTime = Time.time + clipLength;
            
            while (call?.Invoke(own,audio.clip.name) ?? (audio.loop || Time.time < endTime))
            {
                if (own != null)
                {
                    audio.transform.position = own.transform.position;
                }

                if (!audio.isPlaying)
                {
                    break;
                }

                yield return null;
            }

            audio.Stop();
            Pools.Put(audio);
        }
    }
}