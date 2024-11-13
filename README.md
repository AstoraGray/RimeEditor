# Rime Framework v0.0.1
![original](https://github.com/user-attachments/assets/581aa995-2678-4c5a-90d8-cda9a440d1ba)

「Rime Framework」很轻，就像雾凇上的白霜一般，覆之于上，将其包裹着的物体装饰的冰洁、神圣。

https://github.com/AstoraGray/Unity-RimeFramework

「Rime Framework」是Unity的一个超轻量级框架，目的是提供简洁的接口，使用起来不会破坏原代码的结构，就像没有一样。目前目前主要有九个模块：Consoles、Controls、States、Pools、Cycles、Animators、Scenes、Navigations、Audios。RimeManager负责启动模块。

Consoles 💻

Rime Framework的控制台，负责打印RimeFramework的Log，支持打印实例和类型

Controls 🎮

Rime Framework的控制器，基于InputSystem开发，用事件驱动控制输入事件，支持设备热插拔，蓄力长按，并且能很好的扩展改建

States  🗡️

Rime Framework的状态机，非常简洁易用，可以和Controls配合完成角色操纵的开发，并且能很好的完成状态切换、动画切换的操作

Pools 💧

Rime Framework的对象池，支持Object、Component、GameObject的三重管理，并且能够配合Cycles实现非Mono完整的Unity周期

Cycles  🕙

Rime Framework的生命周期控制，配合Pools优化内存，能够很简单地实现非Mono类加入到Unity循环的操作

Animators ✍️

Rime Framework的动画师，注册播放动画，动画融合以及动画组、支持成功失败回调，可以脱离Animator连线

Scenes 🎬

Rime Framework的布景员，负责异步加载和卸载场景，支持完成回调、取消回调

Navigations ➡️

Rime Framework的导航组，对游戏中的所有面板进行导航，并且有防循环机制

Audios 🔊

Rime Framework的音效师，管理游戏中的所有声音，并且与Pools联动

------

The "Rime Framework" is lightweight, like the white frost on rime ice, enveloping objects and adorning them with purity and sanctity.

https://github.com/AstoraGray/Unity-RimeFramework

The "Rime Framework" is an ultra-lightweight framework for Unity, designed to provide a concise interface that does not disrupt the structure of the original code, making it feel almost invisible. Currently, it features nine main modules: Consoles, Controls, States, Pools, Cycles, Animators, Scenes, Navigations, and Audios. The RimeManager is responsible for initializing these modules.

**Consoles 💻**

The console of the Rime Framework, responsible for printing out the logs of RimeFramework. It supports logging for instances and types.

**Controls 🎮**

The controller of the Rime Framework, developed based on the InputSystem. It uses event-driven mechanisms to manage input events, supports hot-swapping of devices, and allows for long press actions. It is also easily extensible and customizable.

**States 🗡️**

The state machine of the Rime Framework, which is simple and easy to use, can work in conjunction with Controls to facilitate the development of character manipulation, and effectively handle state and animation transitions.

**Pools 💧**

The object pool of the Rime Framework, which supports triple management of Objects, Components, and GameObjects, and integrates well with Cycles to achieve a complete Unity lifecycle without using Mono.

**Cycles 🕙**

The lifecycle management of the Rime Framework, which optimizes memory in conjunction with Pools and makes it easy to add non-Mono classes into the Unity loop.

**Animators ✍️**

The animator of the Rime Framework, which registers animations for playback, blending animations, and supports callback mechanisms for success and failure, allowing it to operate independently of Animator connections.

**Scenes 🎬**

The scene manager of the Rime Framework, responsible for asynchronously loading and unloading scenes, supporting completion callbacks and cancellation callbacks.

**Navigations ➡️**

The navigation group of the Rime Framework, which manages navigation for all panels in the game and includes a mechanism to prevent loops.

**Audios 🔊**

The sound engineer of the Rime Framework, which manages all audio in the game and interacts with Pools.
