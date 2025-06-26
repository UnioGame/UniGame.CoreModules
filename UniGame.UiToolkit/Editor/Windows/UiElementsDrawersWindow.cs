#if ODIN_INSPECTOR

using Sirenix.OdinInspector.Editor;

namespace UniGame.UiElements.Editor
{
    using System;
    using System.Collections.Generic;
    using R3;
    using UniModules.Editor;
    using Sirenix.OdinInspector;
    using TypeDrawers;
    using UniGame.Runtime.DataFlow;
    using UniModules.UniGame.UiElements.Editor.TypeDrawers;
    using UnityEditor;
    using UnityEngine;

    [Serializable]
    public class DrawerItemInfo
    {
        public string name;
        public Type type;
        public MonoScript source;

        [Button]
        public void OpenScript()
        {
            type?.OpenEditorScript();
        }
    }
    
    public class UiElementsDrawersWindow : OdinEditorWindow
    {
        [MenuItem("UniGame/UiElements/Drawers")]
        private static void OpenWindow()
        {
            var window = GetWindow<UiElementsDrawersWindow>();
            window.InitializeWindow();
            window.Show();
        }
        
        private LifeTime _lifeTimeDefinition = new LifeTime();
        
        [SerializeField]
        private List<DrawerItemInfo> _drawers = new List<DrawerItemInfo>();
        [Space]
        [SerializeField]
        private List<DrawerItemInfo> _fieldDrawers = new List<DrawerItemInfo>();
        
        public void InitializeWindow()
        {
            _lifeTimeDefinition.Release();
            UiElementFactory.
                Ready.
                Subscribe(UpdateView).
                AddTo(_lifeTimeDefinition);
        }

        private void UpdateView(bool ready)
        {
            foreach (var drawer in UiElementFactory.Drawers) {
                var type = drawer.GetType();
                _drawers.Add(new DrawerItemInfo() {
                    type = type,
                    name = type.Name,
                    source = AssetEditorTools.GetScriptAsset(type),
                });
            }
            
            foreach (var drawer in UiElementFactory.FieldDrawers) {
                var type = drawer.GetType();
                _drawers.Add(new DrawerItemInfo() {
                    type   = type,
                    name   = type.Name,
                    source = AssetEditorTools.GetScriptAsset(type),
                });
            }
        }

        protected override void OnDestroy()
        {
            _lifeTimeDefinition.Terminate();
            base.OnDestroy();
        }
    }
}

#endif