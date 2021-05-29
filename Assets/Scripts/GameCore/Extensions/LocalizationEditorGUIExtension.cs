#if UNITY_EDITOR
using System;
using I2.Loc;
using UnityEditor;
using UnityEngine;

namespace GameCore.Extensions
{
    public static class LocalizationEditorGUIExtension
    {
        public static string DrawStringReference(string text, string value, string key, Vector2 scrollPosition, params GUILayoutOption[] options)
        {
            var isExpanded = false;
            var tempValue = string.Empty;
            var toggleKey = key + "_" + isExpanded.GetType().Name;
            var tempValueKey = key + "_" + tempValue.GetType().Name;
            isExpanded = EditorPrefs.GetBool(toggleKey);
            tempValue = EditorPrefs.GetString(tempValueKey);

            GUILayout.BeginHorizontal(options);
            isExpanded = GUILayout.Toggle(isExpanded, GUIContent.none, EditorStyles.foldout, GUILayout.Width(10));
            EditorGUILayout.LabelField(text, GUILayout.MaxWidth(135));
            value = EditorGUILayout.TextArea(value ?? string.Empty);
            GUILayout.EndHorizontal();

            EditorPrefs.SetBool(toggleKey, isExpanded);

            if (!isExpanded)
            {
                return value;
            }

            EditorGUILayout.Space();
            tempValue = EditorGUIExtension.SearchField(tempValue, 255, true);

            EditorPrefs.SetString(tempValueKey, tempValue);

            if (string.IsNullOrEmpty(tempValue))
            {
                return value;
            }

            var termsList = LocalizationManager.GetTermsList();

            var filter = tempValue.ToUpper();
            for (var i = termsList.Count - 1; i >= 0; --i)
            {
                if (!termsList[i].ToUpper().Contains(filter))
                {
                    termsList.RemoveAt(i);
                }
            }

            termsList.Sort(StringComparer.OrdinalIgnoreCase);

            if (termsList.Count <= 0)
            {
                return value;
            }

            var previousColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.gray;
            GUILayout.BeginVertical(EditorStyles.textArea, GUILayout.ExpandWidth(false), GUILayout.Height(200));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            foreach (var term in termsList)
            {
                if (GUILayout.Button(term, EditorStyles.miniLabel, GUILayout.MaxWidth(Screen.width - 70)))
                {
                    value = term;
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUI.backgroundColor = previousColor;
            return value;
        }
    }
}
#endif