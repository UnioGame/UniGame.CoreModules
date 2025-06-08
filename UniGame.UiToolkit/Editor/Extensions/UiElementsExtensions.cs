using System;
using System.Collections.Generic;
using System.Linq;
using UniModules.Editor;
using UniGame.Runtime.ReflectionUtils;
using UniGame.Core.Runtime.SerializableType.Editor.SerializableTypeEditor;
using UniGame.CoreModules.Editor.SerializableTypeEditor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace UniModules.UniGame.CoreModules.UniGame.Core.Editor.UiElements
{
    public static class UiElementsExtensions
    {

        public static VisualElement DrawAllChildren(this SerializedObject target, bool drawChildren = true)
        {
            var visualContainer = new VisualElement();

            foreach (var child in target.GetVisibleChildren(drawChildren))
            {
                var view = new PropertyField(child);
                visualContainer.Add(view);
            }

            return visualContainer;
        }

#if ENABLE_UI_TOOLKIT
        

        public static VisualElement CreateSerializedTypeDropDown(Type baseType,SerializedProperty property)
        {
            var type = property.GetSerializedType();
            return CreateTypePopup(property.displayName, baseType,type , property.SetSerializedType);
        }
        
        public static DropdownField CreateDropDownValue(this List<string> items,string selection, Func<string,int,string> selectionFunction)
        {
            if (items == null || items.Count <= 0)
                return new DropdownField();
            
            var selectionIndex = items.IndexOf(selection);
            selectionIndex = selectionIndex < 0 ? 0 : selectionIndex;
            
            var menu = new DropdownField(items,selectionIndex,
                newValue => selectionFunction(newValue,items.IndexOf(newValue)));
           
            return menu;
        }
        
         
        public static DropdownField CreateTypePopup(string label, Type baseType, Type selectedType,Action<Type> typeSelected)
        {
            //all assignable types
            var types = baseType.GetAssignableTypes();
            var typeNames = types.Select(x => x.Name).ToList();
            var selectedTypeItem = types.FirstOrDefault(x => x == selectedType) ?? types.FirstOrDefault();
            var selectedName = selectedTypeItem == null ? string.Empty : selectedTypeItem.Name;
            
            var field = typeNames.CreateDropDownValue(selectedName, (x, index) =>
            {
                typeSelected?.Invoke(types[index]);
                return x;
            });

            field.label = label;
            
            return field;
        }
      
#endif
        
    }
}
