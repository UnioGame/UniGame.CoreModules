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

Context is Reactive Container of strong typed data and Resolving them dynamicaly with support async operations

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

#### Resolve dependencies with Context

```cs
    public ILifeTime LifeTime { get; }

    public async void Resolve(IContext context)
    {
        //async await of value from context with context lifetime
        var asyncFooValue = await context.ReceiveFirstAsync<IFoo>();

        //async await of value from context with direct lifetime
        var asyncLifeTimeFooValue = await context.ReceiveFirstAsync<IFoo>(LifeTime);

        //observable value
        context.Receive<IFoo>()
            .Subscribe(x => SomeAction(x))
            .AddTo(LifeTime);

        //sync value acquire
        var syncValue = context.Get<IFoo>();
    }
```

### Scene Context

Additional API for Content that allow you register data into Scene Scoped Context container

### Context Data Sources

Async Data Sources of data with registration into target Context

### Game Services

## Addressables Tools

Toolset for easy and painless usage of <a href="https://docs.unity3d.com/Packages/com.unity.addressables@latest">Unity Addressables Package</a>  

The main problem of unity package usage is no easy control on assets references lifetime. Where is no way to bind lifetime of references to game state or objects
Our toolset contains serveral helpful extensions that simplify your workflow

base extensions methods for Addressable System can be found at 
<a href="https://github.com/UniGameTeam/UniGame.AddressableTools/blob/main/Runtime/Extensions/AddressableExtensions.cs">AddressableExtensions</a>

### Load AssetReference with LifeTime

```cs

  ILifeTime LifeTime = new LifeTimeDefinition();

  private async UniTask LoadReferences()
  {
      _goldResource            = await _goldReference.LoadAssetTaskAsync<GameResourceData>(LifeTime);
      _starsResource           = await _starsReference.LoadAssetTaskAsync<GameResourceData>(LifeTime);
      _diamondsResource        = await _diamondsReference.LoadAssetTaskAsync<GameResourceData>(LifeTime);
      _energyResource          = await _energyReference.LoadAssetTaskAsync<GameResourceData>(LifeTime);
      _boostTimeHintResource   = await _boostTimeHintReference.LoadAssetTaskAsync<GameResourceData>(LifeTime);
      _highlightHintResource   = await _highlightHintReference.LoadAssetTaskAsync<GameResourceData>(LifeTime);
      _searchHintResource      = await _searchHintReference.LoadAssetTaskAsync<GameResourceData>(LifeTime);
      _realMoneyResource       = await _realMoneyReference.LoadAssetTaskAsync<GameResourceData>(LifeTime);
      _multiSearchHintResource = await _multiSearchHintReference.LoadAssetTaskAsync<GameResourceData>(LifeTime);
  }
```

### Load Same Reference

```cs

  LifeTimeDefinition LifeTime1 = new LifeTimeDefinition();
  LifeTimeDefinition LifeTime2 = new LifeTimeDefinition();

  private async UniTask LoadReferences()
  {
      _goldResource1           = await _goldReference.LoadAssetTaskAsync<GameResourceData>(LifeTime1);
      _goldResource2           = await _goldReference.LoadAssetTaskAsync<GameResourceData>(LifeTime2);

      LifeTime1.Release(); //  _goldReference still valid because of _goldResource2 has active reference and alive lifetime       

      LifeTime2.Release(); // all lifetime's terminated. _goldReference will be unloaded
  }
```

### Load Component from AssetReference

```cs

  AssetReference gameObjectReference;

  private async UniTask LoadReferences()
  {
      Transform objectTransform = await gameObjectReference.LoadAssetTaskAsync<Transform>(LifeTime);
  }

```

### Load Interface from AssetReference
  
- Let's load some interface of MonoBehaviour from GameObject

```cs

  AssetReference gameObjectReference;

  private async UniTask LoadReferences()
  {
      var assetResult = await gameObjectReference.LoadAssetTaskAsync<GameObject,IFoo>(LifeTime);
      IFoo = assetResult.result;
  }

```
  
```cs

AssetReference gameObjectReference;
AssetReference soReference;

private async UniTask LoadReferences()
{
    IFoo assetResult = await gameObjectReference.LoadAssetTaskApiAsync<GameObject,IFoo>(LifeTime);
    
    ISomeScriptableObject assetResult = await soReference.LoadAssetTaskApiAsync<ScriptableObject,ISomeScriptableObject>(LifeTime);
}

```
  
or you can use GameObjectAssetReference
  
```cs

AssetReferenceGameObject gameObjectReference;

private async UniTask LoadReferences()
{
    IFoo assetResult = await gameObjectReference.LoadAssetTaskAsync<IFoo>(LifeTime);
}
  
```

### Load Collections of References
  
```cs
private readonly IReadOnlyList<AssetReference> resources;
private readonly List<IFoo> sources = new List<IFoo>();

public async UniTask<IReadOnlyList<SomeScriptableObject>> Execute(ILifeTime lifeTime)
{
    return await resources.LoadAssetsTaskAsync<ScriptableObject, IFoo, AssetReference>(sources,lifeTime);
}
  
```
  
  
## Editor Tools

# Api References

- <a href="https://github.com/neuecc/UniRx">UniRx</a> - Reactive Extensions for Unity
- <a href="https://github.com/Cysharp/UniTask">UniTask</a> - Provides an efficient allocation free async/await integration for Unity 
- <a href="https://github.com/Cysharp/ZString">ZSting</a> - Zero Allocation StringBuilder for .NET Core and Unity. 
- <a href="https://github.com/favoyang/unity-addressable-importer">Addressables Importer</a> - A simple rule-based addressable asset importer. The importer marks assets as addressable, by applying to files having a path matching the rule pattern.

# License

MIT
