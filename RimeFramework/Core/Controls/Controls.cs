using System;
using System.Collections;
using System.Linq;
using RimeFramework.Tool;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RimeFramework.Core
{
    /// <summary>
    /// 霜 · 控制器 🎮
    /// </summary>
    /// <b> Note: 后续将在Enable中初始化，读取保存的按键配置文件，以及管理协程
    /// <see cref="Register(MonoBehaviour,InputAction, Action{InputAction.CallbackContext})"/> 注册输入事件
    /// <remarks>Author: AstoraGray</remarks>
    public class Controls : Singleton<Controls>
    {
        private static InputSettings _bindings;

        public static InputSettings Bindings
        {
            get
            {
                if (_bindings == null)
                {
                    _bindings = new InputSettings();
                    _bindings.Enable();
                }
                return _bindings;
            }
        }

        public static Action onControlSchemeChanged; // 输入切换事件

        public static Action<BindingBehaviour> onBehaviorUnregistered; // 输入注销事件，用来结束长按协程
        public static string CurrentControlScheme { get; private set; } = CONTROL_SCHEME_KEYBOARD_MOUSE;


        public const string CONTROL_SCHEME_KEYBOARD_MOUSE = "Keyboard Mouse"; // 键鼠
        public const string CONTROL_SCHEME_CONTROLLER = "Controller"; // 手柄


        public static bool UsingController()
        {
            return CurrentControlScheme == CONTROL_SCHEME_CONTROLLER;
        }

        public static bool UsingKeyboardMouse()
        {
            return CurrentControlScheme == CONTROL_SCHEME_KEYBOARD_MOUSE;
        }
    
        public static void OnLastInputDeviceChanged(string lastInputDevice)
        {
            UpdateCurrentControlScheme(lastInputDevice);
        }

        private static void UpdateCurrentControlScheme(string lastInputDevice)
        {
            switch (lastInputDevice)
            {
                case "Keyboard Mouse":
                    SetCurrentControlScheme(CONTROL_SCHEME_KEYBOARD_MOUSE);
                    break;
                case "Controller":
                    SetCurrentControlScheme(CONTROL_SCHEME_CONTROLLER);
                    break;
                default:
                    Debug.LogWarning("上一个输入设备是未知的控制方案" + lastInputDevice);
                    break;
            }
        }
        /// <summary>
        /// 输入设备切换
        /// </summary>
        /// <param name="newControlScheme">输入设备/风格</param>
        private static void SetCurrentControlScheme(string newControlScheme)
        {
            if (CurrentControlScheme == newControlScheme) return;

            Consoles.Print(nameof(Controls),"[Input] 输入设备切换 [FROM: " + CurrentControlScheme + "] [TO: " + newControlScheme +
                                       "]");

            CurrentControlScheme = newControlScheme;

            onControlSchemeChanged?.Invoke();
        }
        
        private void OnEnable()
        {
            Bindings.Enable();
        }

        private void OnDisable()
        {
            Bindings.Disable();
        }

        public static BindingBehaviour Register(MonoBehaviour own,InputAction binding,Action<InputAction.CallbackContext> behaviour)
        {
            BindingMonoBehaviour newBehaviour = new BindingMonoBehaviour(own, binding, behaviour);
            InputAction action = Bindings.FindAction(binding.name);
            action.performed += newBehaviour.Invoke;
            return newBehaviour;
        }
        
        public static BindingBehaviour Register(BindingBehaviour bindingBehavior)
        {
            InputAction action = Bindings.FindAction(bindingBehavior.binding.name);
            action.performed += bindingBehavior.Invoke;
            return bindingBehavior;
        }

        public static void UnRegister(BindingBehaviour behaviour)
        {
            InputAction action = Bindings.FindAction(behaviour.binding.name);
            if (action == null)
            {
                Consoles.Print(nameof(Controls),"找不到状态，是退出播放模式了吗?");
                return;
            }
            action.performed -= behaviour.Invoke;
            onBehaviorUnregistered?.Invoke(behaviour);
        }
        
        public static Coroutine StartCoroutineOnInstance(IEnumerator coroutine)
        {
            return Instance.StartCoroutine(coroutine);
        }

        public static void StopCoroutineOnInstance(Coroutine coroutine)
        {
            if (coroutine != null && Instance != null)
                Instance.StopCoroutine(coroutine);
        }
    }

    /// <summary>
    /// 基础行为
    /// </summary>
    public class BindingBehaviour
    {
        public InputAction binding;
        public Action<InputAction.CallbackContext> behaviour;
        public bool isEnabled;

        public BindingBehaviour(InputAction binding,Action<InputAction.CallbackContext> behaviour,bool isEnabled = true)
        {
            this.binding = binding;
            this.behaviour = behaviour;
            this.isEnabled = isEnabled;
        }

        public virtual void Invoke(InputAction.CallbackContext context)
        {
            if (isEnabled)
            {
                behaviour?.Invoke(context);
            }
        }
    }
    /// <summary>
    /// 按键行为，绑定生命周期
    /// </summary>
    public class BindingMonoBehaviour : BindingBehaviour
    {
        public MonoBehaviour own;
        
        public BindingMonoBehaviour(MonoBehaviour own,InputAction binding,
            Action<InputAction.CallbackContext> behaviour,
            bool isEnabled = true)
            : base(binding, behaviour, isEnabled)
        {
            this.own = own;
        }

        public override void Invoke(InputAction.CallbackContext context)
        {
            if (own == null || !own.isActiveAndEnabled)
            {
                Controls.UnRegister(this);
                return;
            }
            base.Invoke(context);
        }
    }
    /// <summary>
    /// 长按行为
    /// </summary>
    public class BindingHeldBehavior : BindingMonoBehaviour
    {
        public float holdDuration { get; private set; } // 长按时间
        public event Action<InputAction.CallbackContext> onHoldStarted; // 开始长按
        public event Action<InputAction.CallbackContext> onHoldCompleted; // 长按结束
        public event Action<float> onButtonReleasedEarly; // 提前结束
        public event Action<float> onEachFrameWhileButtonHeld;  // 逐帧更新 

        private Coroutine _activeBindingHeldCoroutine; 

        public BindingHeldBehavior(MonoBehaviour own,InputAction binding,
            float holdDuration,
            Action<InputAction.CallbackContext> onHoldStarted = null,
            Action<InputAction.CallbackContext> onHoldCompleted = null,
            Action<float> onButtonReleasedEarly = null,
            Action<float> onEachFrameWhileButtonHeld = null,
            bool isEnabled = true)
            : base(own,binding, onHoldCompleted, isEnabled)
        {
            this.holdDuration = holdDuration;
            this.onHoldStarted = onHoldStarted;
            this.onHoldCompleted = onHoldCompleted;
            this.onButtonReleasedEarly = onButtonReleasedEarly;
            this.onEachFrameWhileButtonHeld = onEachFrameWhileButtonHeld;

            // 提前取消时，结束协程
            Controls.onBehaviorUnregistered += (BindingBehaviour behaviorThatWasUnregistered) =>
            {
                if (behaviorThatWasUnregistered == this)
                {
                    Controls.StopCoroutineOnInstance(_activeBindingHeldCoroutine);
                }
            };
        }

        public override void Invoke(InputAction.CallbackContext context)
        {
            if (own == null || !own.isActiveAndEnabled)
            {
                Controls.UnRegister(this);
                return;
            }

            if (_activeBindingHeldCoroutine != null)
            {
                return;
            }
            _activeBindingHeldCoroutine = Controls.StartCoroutineOnInstance(ICheckForBindingHeld(context));
        }

        private IEnumerator ICheckForBindingHeld(InputAction.CallbackContext context)
        {
            if (binding.controls[0] == null)
            {
                Consoles.Print(this,"绑定输入的按键为空");
                yield return null;
            }

            onHoldStarted?.Invoke(context);

            float timeSinceButtonPressed = 0;
            while (AnyBindingControlIsHeld() || timeSinceButtonPressed < holdDuration)
            {
                timeSinceButtonPressed += Time.deltaTime;
                onEachFrameWhileButtonHeld?.Invoke(timeSinceButtonPressed);
                if (!AnyBindingControlIsHeld())
                {
                    onButtonReleasedEarly?.Invoke(timeSinceButtonPressed);
                    _activeBindingHeldCoroutine = null;
                    break;
                }

                if (timeSinceButtonPressed >= holdDuration)
                {
                    onHoldCompleted?.Invoke(context);
                    _activeBindingHeldCoroutine = null;
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        private bool AnyBindingControlIsHeld()
        {
            return binding.controls.ToList().Any(control => control.IsPressed());
        }
    }
}