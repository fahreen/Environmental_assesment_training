using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

public class DialogueNode : BaseNode {

    [Input] public string entry;
    public string question;
    [Output(dynamicPortList = true)] public string[] OptionKeys;

    public override List<string> GetString()
    {
        return new List<string>(OptionKeys);
    }

    // will yell at user for inputing not a key in the node graph
    private void OnValidate()
    {
        foreach (string key in OptionKeys)
        {
            if (!LocalizationValues.db.keyExists(key))
            {
                Debug.LogError($"This key does not exist: {key}");
            }
        }
    }

}