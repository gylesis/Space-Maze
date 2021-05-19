﻿using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace MalbersAnimations
{
    public abstract class IDs : ScriptableObject
    {
        [Tooltip("ID Value for the Animator transitions in order to Execute the wanted animation clip")]
        public int ID;

        public static implicit operator int(IDs reference)
        {
            return reference.ID;
        }
    }


#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(IDs), true)]
    public class IDDrawer : PropertyDrawer
    {
        /// <summary> Cached style to use to draw the popup button. </summary>
        private GUIStyle popupStyle;

        List<IDs> Instances;
        List<string> popupOptions;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (popupStyle == null)
            {
                popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                popupStyle.imagePosition = ImagePosition.ImageOnly;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            // Calculate rect for configuration button
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += popupStyle.margin.top;
            buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
            buttonRect.x -= 20;

            position.xMin = buttonRect.xMax;

            // Store old indent level and set it to 0, the PrefixLabel takes care of it
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;


            if (Instances == null || Instances.Count == 0)
            {
                var NameOfType = GetPropertyType(property);
                string[] guids = AssetDatabase.FindAssets("t:" + NameOfType);  //FindAssets uses tags check documentation for more info
                                                                               //  Debug.Log("ONNCE");
                Instances = new List<IDs>();
                popupOptions = new List<string>();
                popupOptions.Add("None");

                for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                    var inst = AssetDatabase.LoadAssetAtPath<IDs>(path);
                    Instances.Add(inst);
                    popupOptions.Add(inst.name);
                }

            }
            var PropertyValue = property.objectReferenceValue;

            //  Debug.Log(PropertyValue);
            int result = 0;

            if (PropertyValue != null && Instances.Count > 0)
            {
                result = Instances.FindIndex(i => i.name == PropertyValue.name) + 1; //Plus 1 because 0 is None
            }


            result = EditorGUI.Popup(buttonRect, result, popupOptions.ToArray(), popupStyle);

            if (result == 0)
            {
                property.objectReferenceValue = null;
            }
            else
            {
                var NewSelection = Instances[result - 1];
                property.objectReferenceValue = NewSelection;
            }

            //Select the Instance of the selection

            // position.x += 20;
            // position.width -= 20;

            EditorGUI.PropertyField(position, property, GUIContent.none, false);

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }




        public static string GetPropertyType(SerializedProperty property)
        {
            var type = property.type;
            var match = Regex.Match(type, @"PPtr<\$(.*?)>");
            if (match.Success)
                type = match.Groups[1].Value;
            return type;
        }
    }
#endif
}