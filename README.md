- [Getting Started](#getting-started)
- [Modules](#modules)
  - [LifeTime](#lifetime) 
    - [Base API](#lifetime-api)
    - [LifeTime Extensions](#lifetime-extensions)
    - [LifeTime ScriptableObjects](#lifetime scriptableobjects)
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

### Base API

### LifeTime Extensions

### LifeTime ScriptableObjects

## Context

### Scene Context
### Context Data Sources
### Game Services

## Editor Tools

# Api References

# License
