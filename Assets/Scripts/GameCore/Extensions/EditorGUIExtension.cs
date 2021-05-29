#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace GameCore.Extensions
{
    public static class EditorGUIExtension
    {
        private static bool mEndHorizontal = false;

        public static void BeginContents(bool minimalistic = false)
        {
            if (!minimalistic)
            {
                mEndHorizontal = true;
                GUILayout.BeginHorizontal();
                EditorGUILayout.BeginHorizontal("TextArea", GUILayout.MinHeight(10f));
            }
            else
            {
                mEndHorizontal = false;
                EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
                GUILayout.Space(10f);
            }

            GUILayout.BeginVertical();
            GUILayout.Space(2f);
        }

        public static void EndContents()
        {
            GUILayout.Space(3f);
            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            if (mEndHorizontal)
            {
                GUILayout.Space(3f);
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(3f);
        }

        public static bool DrawHeader(string text, string key, bool forceOn = false, bool minimalistic = false, params GUILayoutOption[] options)
        {
            bool state = EditorPrefs.GetBool(key, true);

            if (!minimalistic) GUILayout.Space(3f);
            if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
            GUILayout.BeginHorizontal(options);
            GUI.changed = false;

            if (minimalistic)
            {
                if (state) text = "\u25BC" + (char) 0x200a + text;
                else text = "\u25BA" + (char) 0x200a + text;

                GUILayout.BeginHorizontal();
                GUI.contentColor = EditorGUIUtility.isProSkin
                    ? new Color(1f, 1f, 1f, 0.7f)
                    : new Color(0f, 0f, 0f, 0.7f);
                if (!GUILayout.Toggle(true, text, "PreToolbar2", GUILayout.MinWidth(20f))) state = !state;
                GUI.contentColor = Color.white;
                GUILayout.EndHorizontal();
            }
            else
            {
                text = "<b><size=11>" + text + "</size></b>";
                if (state) text = "\u25BC " + text;
                else text = "\u25BA " + text;
                if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;
            }

            if (GUI.changed) EditorPrefs.SetBool(key, state);

            if (!minimalistic) GUILayout.Space(2f);
            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
            if (!forceOn && !state) GUILayout.Space(3f);
            return state;
        }

        public static bool ColorButton(string text, Color btnColor)
        {
            Color oldColor = GUI.color;
            GUI.color = btnColor;
            bool state = GUILayout.Button(text);
            GUI.color = oldColor;
            return state;
        }

        public static bool ColorButton(string text, Color btnColor, params GUILayoutOption[] options)
        {
            Color oldColor = GUI.color;
            GUI.color = btnColor;
            bool state = GUILayout.Button(text, options);
            GUI.color = oldColor;
            return state;
        }
        
        public static string SearchField(string value, int maxLength, bool expandWidth, float width = 0)
        {
            GUILayout.BeginHorizontal();
            value = GUILayout.TextField(value, maxLength, EditorStyles.toolbarSearchField, GUILayout.Width(width), GUILayout.ExpandWidth(expandWidth));
            if (GUILayout.Button( string.Empty, string.IsNullOrEmpty(value) ? "ToolbarSeachCancelButtonEmpty" : "ToolbarSeachCancelButton", GUILayout.ExpandWidth(false)))
            {
                value = string.Empty;
                GUI.FocusControl(string.Empty);
            }
            GUILayout.EndHorizontal();
            return value;
        }

        public static string TextField(string value, int maxLength, float width)
        {
            value = GUILayout.TextField(value, maxLength, EditorStyles.textField, GUILayout.Width(width));
            return value;
        }

        public static string TextArea(string value, int maxLength, float width)
        {
            value = GUILayout.TextArea(value, maxLength, EditorStyles.textArea, GUILayout.Width(width));
            return value;
        }

        public static bool ToolbarButton(GUIContent content, float width = 50)
        {
            return GUILayout.Button(content, EditorStyles.toolbarButton, GUILayout.Width(width));
        }

        public static bool ToolbarDropDown(GUIContent content, float width = 50)
        {
            return GUILayout.Button(content, EditorStyles.toolbarDropDown, GUILayout.Width(width));
        }
        
        public static void Headlines(GUIContent[] contents)
        {
            Headlines(contents, EditorStyles.whiteLabel);
        }
        
        public static void Headlines(string[] contents)
        {
            Headlines(contents, EditorStyles.whiteLabel);
        }

        public static void Headlines(GUIContent[] contents, GUIStyle style)
        {
            EditorGUILayout.BeginHorizontal();
            foreach (var content in contents)
            {
                GUILayout.Label(content, style, GUILayout.ExpandWidth(true));
            }
            EditorGUILayout.EndHorizontal();
        }

        public static void Headlines(string[] contents, GUIStyle style)
        {
            EditorGUILayout.BeginHorizontal();
            foreach (var content in contents)
            {
                GUILayout.Label(content, style,GUILayout.ExpandWidth(true));
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif
