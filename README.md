- [Getting Started](#getting-started)
- [Modules](#modules)
  - [LifeTime](#lifetime) 
    - [Base API](#lifetime-api)
    - [LifeTime Extensions](#lifetime-extensions)
    - [LifeTime ScriptableObjects](#lifetime-scriptableobjects)
  - [Context](#context)
    - [Game Services](#scene-context)
    - [Context Data Source](#context-data-sources)
    - [Game Services](#game-services)
  - [Addressables Tools](#addressables-tools)
  - [Editor Tools](#editor-tools)
- [API References](#api-references)
  - [UniRx](#unirx)
  - [UniTask](#unitask)
  - [ZString](#zstring)
- [License](#license)

# Getting Started

## How to Install

Add to your project manifiest by path [%UnityProject%]/Packages/manifiest.json new Scope:

```json
{
  "scopedRegistries": [
    {
      "name": "UniGame",
      "url": "http://packages.unigame.pro:4873/",
      "scopes": [
        "com.unigame",
        "com.littlebigfun",
        "com.alelievr"
      ]
    },
    
    "__comment":"another scoped registers",
    
  ],
}

```

Now install via Package Manager

![](https://github.com/UniGameTeam/UniGame.CoreModules/blob/master/Readme/Assets/package_install.png)

### Additional helpful registers

- Open UPM Register

```json
    {
      "name": "package.openupm.com",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.coffee",
        "com.coffee.ui-particle",
        "com.coffee.ui-effect",
        "com.littlebigfun.addressable-importer",
        "com.yasirkula",
        "com.yasirkula.assetusagedetector",
        "com.openupm"
      ]
    },
 
```
- Google Packages

```json
    {
      "name": "Game Package Registry by Google",
      "url": "https://unityregistry-pa.googleapis.com",
      "scopes": [
        "com.google"
      ]
    },
    
```


# Modules

## LifeTime


Main idea based on JetBrains LifeTime pattern.

- https://www.jetbrains.com/help/resharper/sdk/Platform/Lifetime.html

Any shared resource/service MUST have single owner.  main rule for fluent resources managment: One resource - One owner
That rule make make all resource management pretty straightforward and allow to construct hierarchical dependencies.

![](https://github.com/UniGameTeam/UniGame.CoreModules/blob/master/Readme/Assets/lifetime_terminated.png)

### Base API

```cs

    public interface ILifeTime
    {
        /// <summary>
        /// cleanup action, call when life time terminated
        /// </summary>
        ILifeTime AddCleanUpAction(Action cleanAction);

        /// <summary>
        /// add child disposable object
        /// </summary>
        ILifeTime AddDispose(IDisposable item);

        /// <summary>
        /// save object from GC
        /// </summary>
        ILifeTime AddRef(object o);

        /// <summary>
        /// is lifetime terminated
        /// </summary>
        bool IsTerminated { get; }
    }
    
```

#### LifeTime Creation

you can't create instance of LifeTime class directly, but there is easy way to achieve it

```cs

  LifeTimeDefinition lifeTime = new LifeTimeDefinition();

```

```cs

  LifeTime lifeTime = LifeTime.Create();

```

every instance of LifeTime has unique runtime Id.

you must always share only interface **"LifeTime"** where it needed for resources management and never allow direct access to LifeTime instance. That's guarantee that only owner can Terminate your resources

**LifeTime.Terminate()** cleanup all registered IDisposable, Actions, Object in inverted registration order. After that lifeTime.IsTerminated will return TRUE.

```cs

  LifeTime lifeTime = LifeTime.Create();
  lifeTime.Terminate();

```

All registered actions, disposables on Terminated LifeTime will execute immediately

```cs

  LifeTime lifeTime = LifeTime.Create();
  lifeTime.Terminate();

```

**LifeTime.Release()** cleanup all registered objects and mark

```cs

  LifeTime lifeTime = LifeTime.Create();
  lifeTime.Terminate();
  
  IDisposable disposable1 = new Disposable1();
  lifeTime.AddDisposable(disposable1);// Dispose method call immediately

```

#### LifeTime Methods

- IDisposable cleanup

```cs

  public class Foo : IDisposable
  {
  
    private LifeTime lifeTime = LifeTime.Create();

    public Foo(){
      
      var disposable1 = new Disposable1().AddTo(lifeTime);
      var disposable2 = new Disposable2().AddTo(lifeTime);
      var disposable3 = new Disposable3().AddTo(lifeTime);
      var disposable4 = new Disposable4();
      
      lifeTime.AddDisposable(disposable4);
      
    }
    
    public void Dispose() => lifeTime.Terminate();
  
  }
  
```

- Cleanup Action

```cs

  public class Foo : IDisposable
  {
  
    private LifeTime lifeTime = LifeTime.Create();

    public Foo(){
           
      lifeTime.AddCleanUpAction(CleanUp1);
      lifeTime.AddCleanUpAction(() => CleanUp2());
      
    }
    
    public void Dispose() => lifeTime.Terminate();
  
    public void CleanUp1(){
    
    }
    
    pulic void CleanUp2(){
    
    }
    
  }
  
```

### LifeTime Extensions

### LifeTime ScriptableObjects

## Context

Context is Reactive Container of strong typed data

### Base Context API

```cs

  public interface IContext
  {
        /// <summary>
        /// Subscribe typed message.
        /// </summary>
        IObservable<T> Receive<T>();
        
        /// <summary>
        /// Send Message to all receiver.
        /// </summary>
        void Publish<T>(T message);
        
        /// Try to remove data of TData type
        bool Remove<TData>();
        
        /// Get registered data by TData Type or return default
        TData Get<TData>();
        
        /// Is Data wuth TData type registered
        bool Contains<TData>();
  }

```

### Scene Context

Additional API for Content that allow you register data into Scene Scoped Context container

### Context Data Sources

Async Data Sources of data with registration into target Context

### Game Services



## Editor Tools

# Api References

# License
