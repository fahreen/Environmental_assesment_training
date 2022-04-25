using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
#if UNITY_EDITOR
public class LocalizationGUI : EditorWindow
{
    // for displaying the UI for editing the key/values

    private string newLang = "";
    private string edittingLang = null;
    private int currentLang = 0;
    private int firstVisableLang = 1;

    private string newKey = "";
    private string newValue = "";
    private Color oldColor;

    private void OnEnable()
    {
        edittingLang = null;
        oldColor = GUI.backgroundColor;
    }

    [MenuItem("Tools/Localization Settings")]
    public static void OpenTheWindow()
    {
        GetWindow<LocalizationGUI>();
    }
    public Vector2 scrollPosition = Vector2.zero;
    private void OnGUI()
    {
        // constants to use, can change while running
        float width = position.width - 20;
        scrollPosition = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), scrollPosition, new Rect(0, 0, position.width, 10000), false, true);

        // title
        var titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 40;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        EditorGUI.LabelField(new Rect(0, 0, width, 40), "Localization Tool", titleStyle);
        EditorGUILayout.Space(40);

        // reset everything (for testing)
        if (GUILayout.Button("Reset", GUILayout.Width(width)))
        {
            if (EditorUtility.DisplayDialog("RESET EVERYTHING", $"ARE YOU SURE YOU WANT TO RESET EVERYTHING???", "Yes", "No"))
            {
                LocalizationValues.reset();
                LocalizationValues.db.reset();
            }
        }

        // reset everything (for testing)
        if (GUILayout.Button("Force Save", GUILayout.Width(width)))
        {
            LocalizationValues.ForceSave();
        }

        // select current lang
        using (new GUILayout.HorizontalScope(GUILayout.Width(width)))
        {
            EditorGUILayout.LabelField("Select current laguage: ", widthOfText("Select current laguage: ", 10, GUI.skin.label));
            currentLang = EditorGUILayout.Popup(currentLang, LocalizationValues.db.langStringArray());
            LocalizationValues.db.lastEditorLang = currentLang;
        }

        // add new lang
        using (new GUILayout.HorizontalScope(GUILayout.Width(width)))
        {
            EditorGUILayout.LabelField("Add A Language: ", widthOfText("Add A Language: ", 10, GUI.skin.textField));
            newLang = GUILayout.TextField(newLang, 25);
            if (GUILayout.Button("Add", widthOfText("Add", 30, GUI.skin.button)))
            {
                try
                {
                    LocalizationValues.db.addLanguage(newLang);
                    edittingLang = newLang;
                    newLang = "";
                }
                catch (Exception e)
                {
                    EditorUtility.DisplayDialog("Error", e.Message, "Sorry");
                }
            }
        }

        // warning box for when lang already exists
        if (LocalizationValues.db.langExists(newLang))
        {
            EditorGUILayout.HelpBox("This Lang already exists", MessageType.Warning);
        }

        // choose language to edit
        using (new GUILayout.HorizontalScope())
        {
            // different UI for selecting less than 4 lang
            if (LocalizationValues.db.langValues.Count < 4)
            {
                foreach (var entry in LocalizationValues.db.langValues)
                {
                    // highlighting the current lang
                    if (edittingLang == entry.lang)
                    {
                        GUI.backgroundColor = Color.magenta;
                    } else
                    {
                        GUI.backgroundColor = oldColor;
                    }
                    // display lang buttons
                    if (GUILayout.Button(entry.lang, GUILayout.Width(width / LocalizationValues.db.langValues.Count)))
                    {
                        edittingLang = entry.lang;
                    }
                }
                firstVisableLang = 1;
                GUI.backgroundColor = oldColor;
            } else
            {
                // only display 3 lang at a time, but can scroll left and right
                if (GUILayout.Button("<", widthOfText("<", 0, GUI.skin.button)))
                {
                    firstVisableLang = Mathf.Clamp(firstVisableLang - 1, 0, LocalizationValues.db.langValues.Count - 3);
                }
                for (int i = firstVisableLang; i < firstVisableLang + 3; i++)
                {
                    if (edittingLang == LocalizationValues.db.langValues[i].lang)
                    {
                        GUI.backgroundColor = Color.magenta;
                    }
                    else
                    {
                        GUI.backgroundColor = oldColor;
                    }
                    // display lang buttons
                    if (GUILayout.Button(LocalizationValues.db.langValues[i].lang, GUILayout.Width(width / 3 - 19.5f)))
                    {
                        edittingLang = LocalizationValues.db.langValues[i].lang;
                    }
                }
                GUI.backgroundColor = oldColor;
                // right scroll
                if (GUILayout.Button(">", widthOfText(">", 0, GUI.skin.button)))
                {
                    firstVisableLang = Mathf.Clamp(firstVisableLang + 1, 0, LocalizationValues.db.langValues.Count - 3);
                }
            }
        }

        // display key/values for the editing langauge
        if (edittingLang != null)
        {
            // calculate spacing
            float keySpace = 50;
            float valueSpace = 0;
            float deleteSpace = new GUIStyle(GUI.skin.button).CalcSize(new GUIContent("Delete Key")).x;
            foreach(string key in LocalizationValues.db.keys)
            {
                if (keySpace < new GUIStyle(GUI.skin.label).CalcSize(new GUIContent(key)).x)
                {
                    keySpace = new GUIStyle(GUI.skin.label).CalcSize(new GUIContent(key)).x;
                }
            }
            valueSpace = width - deleteSpace - keySpace - 10;
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Keys", GUILayout.Width(keySpace));
                GUILayout.Label("Values", GUILayout.Width(valueSpace));
            }
            // list out Key Values
            int langIndex = LocalizationValues.db.getLangIndex(edittingLang);
            for (int i = 0; i < LocalizationValues.db.keys.Count; i++)
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label(LocalizationValues.db.keys[i], GUILayout.Width(keySpace));
                    LocalizationValues.db.langValues[langIndex].values[i] = GUILayout.TextField(LocalizationValues.db.langValues[langIndex].values[i], GUILayout.Width(valueSpace));
                    if (GUILayout.Button("Delete Key", GUILayout.Width(deleteSpace)))
                    {
                        LocalizationValues.db.deleteKey(LocalizationValues.db.keys[i]);
                    }
                }
            }
            // add new key value pair
            using (new GUILayout.HorizontalScope())
            {
                newKey = GUILayout.TextField(newKey, GUILayout.Width(keySpace + 2));
                newValue = GUILayout.TextField(newValue, GUILayout.Width(valueSpace));
                if (GUILayout.Button("Add", GUILayout.Width(deleteSpace)))
                {
                    try
                    {
                        LocalizationValues.db.addNewKeyValue(newKey, newValue, edittingLang);
                        newKey = "";
                        newValue = "";
                    } catch (Exception e) 
                    {
                        EditorUtility.DisplayDialog("Error", e.Message, "Sorry");
                    }
                }
            }
            if (LocalizationValues.db.keyExists(newKey))
            {
                EditorGUILayout.HelpBox("This key already exists", MessageType.Warning);
            }
            EditorGUILayout.Space(20);
            var removeLangStyle = new GUIStyle(GUI.skin.button);
            removeLangStyle.normal.textColor = Color.red;
            if (GUILayout.Button("Remove Language", removeLangStyle, GUILayout.Width(width)))
            {
                if (EditorUtility.DisplayDialog("Remove Language",
                $"Are you sure you want to remove {edittingLang}", "Yes", "No"))
                {
                    LocalizationValues.db.removeLanguage(edittingLang);
                    edittingLang = null;
                }
            }
        }
        GUI.EndScrollView();
    }

    private GUILayoutOption[] widthOfText(string text, float extraSpace, GUIStyle style)
    {
        return new GUILayoutOption[] { GUILayout.ExpandWidth(false), GUILayout.MaxWidth((new GUIStyle(style)).CalcSize(new GUIContent(text)).x + extraSpace) };
    }
}
#endif