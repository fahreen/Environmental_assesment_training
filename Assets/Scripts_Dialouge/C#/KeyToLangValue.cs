using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[System.Serializable]
public struct LangValues
{
    // wrapper and struct to hold values
    public LangValues(string lang, List<string> values)
    {
        this.lang = lang;
        this.values = values;
    }
    public string lang;
    public List<string> values;
}

[CreateAssetMenu]
public class KeyToLangValue : ScriptableObject
{
    // scriptableobject to hold EVERYTHING
    public List<string> keys;
    public List<LangValues> langValues;
    public int lastEditorLang;

    public void reset()
    {
        keys.Clear();
        langValues.Clear();
        lastEditorLang = 0;
    }

    public string getValue(string key)
    {
        return langValues[LocalizationValues.currentLang].values[keys.FindIndex(item => item == key)];
    }

    public bool langExists(string newLang)
    {
        int index = langValues.ConvertAll<string>(item => item.lang).FindIndex(item => item == newLang);
        return index != -1;
    }

    public bool keyExists(string newKey)
    {
        return keys.Contains(newKey);
    }

    public string[] langStringArray()
    {
        return langValues.ConvertAll<string>(item => item.lang).ToArray();
    }

    public int getLangIndex(string lang)
    {
        return langValues.FindIndex(item => item.lang == lang);
    }

    public void addLanguage(string newLang)
    {
        if (langExists(newLang))
        {
            throw new Exception("Language already exists");
        }
        else if (newLang == "")
        {
            throw new Exception("Empty language");
        }
        LangValues lang = new LangValues(newLang, Enumerable.Repeat("", keys.Count).ToList());
        langValues.Add(lang);
    }

    public void removeLanguage(string oldLang)
    {
        langValues.RemoveAll(item => item.lang == oldLang);
    }

    public void addNewKeyValue(string newKey, string newValue, string currentLang)
    {
        if (keyExists(newKey))
        {
            throw new Exception("Key already exists");
        }
        else if (newKey == "")
        {
            throw new Exception("Empty Key");
        }
        foreach (LangValues entry in langValues)
        {
            entry.values.Add("");
        }
        int langIndex = getLangIndex(currentLang);
        langValues[langIndex].values[langValues[langIndex].values.Count - 1] = newValue;
        keys.Add(newKey);
    }

    public void deleteKey(string key)
    {
        int index = keys.IndexOf(key);
        keys.RemoveAt(index);
        foreach (LangValues entry in langValues)
        {
            entry.values.RemoveAt(index);
        }
    }
}
