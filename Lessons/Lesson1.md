# Lesson 1

## Goals
1. Introduce the structure of this tutorial series
2. Introduce the basics of game development and game engines
3. Introduce the basic UI of Unity
4. Introduce the organization of Unity and the Unity API
5. Get Mario moving

## Material
[Lesson 1 Material](https://github.com/brholtkamp/UHGameDev-Fall2016/archive/v1.0.zip)

# Introduction
This tutorial series will cover the basics of developing a game using Unity as our main game engine.

The main scripting langauge will be C# and we will lightly cover on some of the basic systems that all game engines share.  Knowledge of object-oriented programming is necessary, but most of the code will be highly documented and should be pretty quick to pick up.

A game engine is an application that contains many subsystems to handle different pieces that are necessary for a game to run.  Some examples of the subsystems the game engine provide are: rendering graphics, simulating physics, handling online connections to a server, inputs from controllers and other devices, and artifical intelligence.  Typically you use an engine for the convenience to not have to implement your own subsystems.

A critical part of a game engine is the idea of the "game loop".  A game can be generally distilled down into a while loop:

```
    while(game.isRunning()) {
        updateGameState();      // Update what's going on in the game: move characters, shoot bullets, etc.

        updateAI();             // Allow the AI to adapt to what has changed

        render();               // Render the graphics onto the screen

        updateEngine();         // Keep track of what all changed in the last update
    }
```

The game loop is generally done per "frame", an image that is put onto the monitor, though the physics and game logic will generally be done on a "fixed" game loop.  This allows the computer to run at lower frames per second when something extremely graphically intense is being rendered on the screen, without the game logic and physics being slowed down and thus behave incorrectly.  A good rule of thumb is, if it's a game mechanic, try to avoid making it frame reliant.  The main exception would be a fighting game.

# Unity (Unity3D)

Unity is a free game engine that is designed to build 2D and 3D games.  Unity is designed around a hierarchy of [GameObjects](https://docs.unity3d.com/ScriptReference/GameObject.html).  A GameObject is just an object in the game that is goverened by scripts.  It typically comes with a [Transform](https://docs.unity3d.com/ScriptReference/Transform.html) which is the information of where the object exists in the game world.  A transform is typically used to keep track of where a GameObject exists in the game world (it's X, Y, and Z coordinates) and how it's oriented (rotation and scaling).

GameObjects can be organized to have children and parent GameObjects, which allow you to build more complex characters and objects in your game.  Additionally a GameObject can be extended with [Components](https://docs.unity3d.com/ScriptReference/Component.html).  A Component is anything that adds functionality to a GameObject.  Some examples of Components are: renderers, colliders, particle systems, etc.  The main power of Unity is that you can easily combine Components to create GameObjects that do many complex things, and you can also create your own Components to do things necessary for your game.

We create Components by making scripts that use the [MonoBehaviour](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html) class (It's named MonoBehaviour because Unity uses the [Mono Project](https://mono-project.com/) in order to run C# code).  You'll see many scripts use the MonoBehaviour class in order to do the game's logic and many of your MonoBehaviours will interact with other MonoBehaviours in your game.  One of the best features of Unity is that you can use public members of classes that derive MonoBehaviour in order to change settings in the middle of your game.  This allows you to test different configurations in your game, in real-time, and without having to recompile the game and waste more time.

If you're curious to see what else Unity has to offer, check it out [here](https://docs.unity3d.com/Manual/index.html).

## Lesson
1. Introduction
    1. Show off the final product of the series [TODO: Link to final build](https://google.com)
