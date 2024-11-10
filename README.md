# Rime Framework
「Rime Framework」很轻，就像雾凇上的白霜一般，覆之于上，将其包裹着的物体装饰的冰洁、神圣。

「Rime Framework」是Unity的一个超轻量级框架，目的是提供简洁的接口，使用起来不会破坏原代码的结构，就像没有一样。目前目前主要有八个模块：Consoles、Controls、States、Pools、Cycles、Animators、Scenes、Navigations。RimeManager负责启动模块。

Consoles 💻

Rime Framework的控制台，负责打印RimeFramework的Log，支持打印实例和类型

Controls 🎮

Rime Framework的控制器，基于InputSystem开发，用事件驱动控制输入事件，支持设备热插拔，蓄力长按，并且能很好的扩展改建

States  🗡️

Rime Framework的状态机，非常简洁易用，可以和Controls配合完成角色操纵的开发，并且能很好的完成状态切换、动画切换的操作

Pools 💧

Rime Framework的对象池，支持Object、Monobehaviour、GameObject的三重管理，并且能够配合Cycles实现非Mono完整的Unity周期

Cycles  🕙

Rime Framework的生命周期控制，配合Pools优化内存，能够很简单地实现非Mono类加入到Unity循环的操作

Animators ✍️

Rime Framework的动画师，注册播放动画，动画融合以及动画组、支持成功失败回调，可以脱离Animator连线

Scenes 🎬

Rime Framework的布景员，负责异步加载和卸载场景，支持完成回调、取消回调

Navigations ➡️

Rime Framework的导航组，对游戏中的所有面板进行导航，并且有防循环机制

Audios 🔊

即将到来，敬请期待！

------

The "Rime Framework" is lightweight, akin to the white frost on rime ice, enveloping objects in an ice-like purity and sanctity.

The "Rime Framework" is an ultra-lightweight framework for Unity, designed to provide a concise interface that won't disrupt the original code structure—it's as if it's not even there. Currently, there are eight main modules: Consoles、Controls、States、Pools、Cycles、Animators、Scenes and Navigations. The RimeManager is responsible for launching these modules.

**Consoles 💻**

The console of the Rime Framework is responsible for printing logs from the Rime Framework, supporting the printing of instances and types.

**Controls 🎮**

The controller of the Rime Framework is developed based on the InputSystem. It uses event-driven mechanisms to control input events, supports hot-swapping of devices, long press charging, and is easily extensible and adaptable.

**States 🗡️**

The state machine of the Rime Framework is very simple and easy to use, allowing collaboration with Controls to facilitate character control development. It also effectively handles state transitions and animation switches.

**Pools 💧**

The object pool of the Rime Framework supports threefold management of Objects, Monobehaviours, and GameObjects, and works with Cycles to achieve a complete Unity lifecycle for non-Mono classes.

**Cycles 🕙**

The lifecycle management of the Rime Framework, in conjunction with Pools, optimizes memory usage and allows for straightforward integration of non-Mono classes into the Unity loop.

**Animators ✍️**

The animator of the Rime Framework registers animations for playback, facilitates animation blending and groups, and supports success and failure callbacks, allowing independence from Animator connections.

**Scenes 🎬**

The scenographer of the Rime Framework is responsible for the asynchronous loading and unloading of scenes, supporting completion callbacks and cancellation callbacks.

**Navigations ➡️**

The navigation group of the Rime Framework manages navigation for all panels in the game, featuring a loop prevention mechanism.

**Audios 🔊**

Coming soon, stay tuned!
