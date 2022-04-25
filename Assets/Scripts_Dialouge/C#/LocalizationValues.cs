using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System;

public static class LocalizationValues
{
    // just a static class for getting and editing the key/values
    public static KeyToLangValue db = null;
    public static readonly string translateLocation = "Assets/Resources/translate.asset";
    public static int currentLang = 0;
    static LocalizationValues()
    {
#if UNITY_EDITOR
        if (AssetDatabase.LoadAssetAtPath(translateLocation, typeof(KeyToLangValue)) == null)
        {
            KeyToLangValue asset = ScriptableObject.CreateInstance<KeyToLangValue>();
            string name = AssetDatabase.GenerateUniqueAssetPath(translateLocation);
            AssetDatabase.CreateAsset(asset, name);
            AssetDatabase.SaveAssets();
            db = asset;
        }
        else
        {
            db = (KeyToLangValue)AssetDatabase.LoadAssetAtPath(translateLocation, typeof(KeyToLangValue));
        }
        EditorUtility.SetDirty(db);
#endif
#if UNITY_STANDALONE
        db = Resources.Load<KeyToLangValue>("translate");
#endif
        currentLang = db.lastEditorLang;
    }

#if UNITY_EDITOR
    public static void ForceSave()
    {
        EditorUtility.SetDirty(db);
        AssetDatabase.SaveAssets();
    }

    public static void reset()
    {
        if (AssetDatabase.LoadAssetAtPath(translateLocation, typeof(KeyToLangValue)) == null)
        {
            KeyToLangValue asset = ScriptableObject.CreateInstance<KeyToLangValue>();
            string name = AssetDatabase.GenerateUniqueAssetPath(translateLocation);
            AssetDatabase.CreateAsset(asset, name);
            AssetDatabase.SaveAssets();
            db = asset;
        }
        else
        {
            db = (KeyToLangValue)AssetDatabase.LoadAssetAtPath(translateLocation, typeof(KeyToLangValue));
        }
        EditorUtility.SetDirty(db);
    }
#endif
}
