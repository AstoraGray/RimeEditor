# Rime Framework
「Rime Framework」很轻，就像雾凇上的白霜一般，覆之于上，将其包裹着的物体装饰的冰洁、神圣。

「Rime Framework」是Unity的一个超轻量级框架，目的是提供简洁的接口，使用起来不会破坏原代码的结构，就像没有一样。目前目前主要有五个模块：Consoles、Controls、States、Cycles、Pools。RimeManager负责启动模块。

Consoles 💻

Rime Framework的控制台，负责打印RimeFramework的Log，支持打印实例和类型

Controls 🎮

Rime Framework的控制器，基于InputSystem开发，用事件驱动控制输入事件，支持设备热插拔，蓄力长按，并且能很好的扩展改建

States  📐

Rime Framework的状态机，非常简洁易用，可以和Controls配合完成角色操纵的开发，并且能很好的完成状态切换、动画切换的操作

Pools 💧

Rime Framework的对象池，支持Object、Monobehaviour、GameObject的三重管理，并且能够配合Cycles实现非Mono完整的Unity周期

Cycles  🕙

Rime Framework的生命周期控制，配合Pools优化内存，能够很简单地实现非Mono类加入到Unity循环的操作

------

The "Rime Framework" is very lightweight, just like the white frost on rime ice, enveloping and decorating the objects it covers with a pure and sacred sheen.

The "Rime Framework" is an ultra-lightweight framework for Unity, designed to provide a concise interface that does not disrupt the structure of the original code, as if it weren't there at all. Currently, it mainly consists of five modules: Consoles, Controls, States, Cycles, and Pools. The RimeManager is responsible for initiating the modules.

#### Consoles 💻

The console of the Rime Framework, responsible for printing logs from Rime Framework, which supports printing instances and types.

#### Controls 🎮

The controller of the Rime Framework, developed based on the Input System. It uses event-driven control for input events, supports device hot-plugging, long-press charge actions, and is well extensible for modifications.

#### States 📐

A state machine in the Rime Framework that is very simple and easy to use. It can work in conjunction with Controls to facilitate character manipulation development and handle state transitions and animation switching effectively.

#### Pools 💧

The object pool of the Rime Framework, which supports triple management of Objects, Monobehaviours, and GameObjects, and can work with Cycles to achieve a complete Unity lifecycle without using Mono.

#### Cycles 🕙

Lifecycle control in the Rime Framework that optimizes memory in conjunction with Pools, allowing for easy integration of non-Mono classes into the Unity loop.
