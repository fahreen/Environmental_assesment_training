using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode; 
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class NodeParser : MonoBehaviour
{
    public DialogueGraph graph;
    public TextMeshProUGUI name;
    public List<Button> options;
    public TMP_Dropdown dropDown;
    public TextMeshProUGUI question;
    private readonly string OPTION_KEYS = "OptionKeys";

    // Call on awake because for some reason the dropdown calls changelang before start
    //private void Awake()
    //{
    //    if (graph == null)
    //    {
    //        Debug.LogError($"Missing dialouge graph for {transform.parent.name}");
    //        enabled = false;
    //        return;
    //    }
    //    // Set current node to starting node
    //    foreach (BaseNode b in graph.nodes)
    //    {
    //        if (b is StartNode)
    //        {
    //            graph.current = b;
    //            graph.current = graph.current.GetPort("exit").Connection.node as BaseNode;
    //            break;
    //        }
    //    }
    //}

    private void OnEnable()
    {
        if (graph == null)
        {
            Debug.LogError($"Missing dialouge graph for {transform.parent.name}");
            enabled = false;
            return;
        }
        // Set current node to starting node
        foreach (BaseNode b in graph.nodes)
        {
            if (b is StartNode)
            {
                graph.current = b;
                graph.current = graph.current.GetPort("exit").Connection.node as BaseNode;
                break;
            }
        }
        // fill in dropdown for change lang
        string[] langs = LocalizationValues.db.langStringArray();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (string lang in langs)
        {
            options.Add(new TMP_Dropdown.OptionData(lang));
        }
        dropDown.options = options;
        dropDown.value = LocalizationValues.currentLang;
        FillOptions();
        name.text = graph.name.Split(' ')[0];
    }

    public void ChangeLang()
    {
        LocalizationValues.currentLang = dropDown.value;
        FillOptions();
    }

    public void FillOptions()
    {
        // reactivate the buttons later if they have a dialouge option
        foreach (Button button in options)
        {
            button.gameObject.SetActive(false);
        }
        bool skipFirst = true;
        int buttonIndex = 0;
        // get all dialouge options
        foreach (NodePort port in graph.current.Outputs)
        {
            if (port.fieldName.StartsWith(OPTION_KEYS))
            {
                if (skipFirst)
                {
                    skipFirst = false;
                    continue;
                }
                try
                {
                    // try to set them to the buttons
                    int index = int.Parse(port.fieldName.Split(' ')[1]);
                    options[buttonIndex].gameObject.SetActive(true);
                    options[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text = 
                        LocalizationValues.db.getValue((graph.current as DialogueNode).OptionKeys[index]);
                    buttonIndex++;
                } catch
                {
                    break;
                }
            }
        }
        if (graph.current is DialogueNode)
        {
            question.text = LocalizationValues.db.getValue((graph.current as DialogueNode).question);
        }
    }

    public void NextNode()
    {
        // gets index of clicked button
        int index = options.FindIndex(item => item == EventSystem.current.currentSelectedGameObject.GetComponent<Button>());
        // create port for next node
        string nextPort = $"{OPTION_KEYS} {index}";
        // set next node
        graph.current = graph.current.GetPort(nextPort).Connection.node as BaseNode;
        // check if end node
        if (graph.current is EndNode)
        {
            //#if UNITY_EDITOR
            //            UnityEditor.EditorApplication.isPlaying = false;
            //#else
            //            Application.Quit();
            //#endif
            GetComponentInParent<EmployeeController2>().EndTalking((graph.current as EndNode).success);
           
            
        }
        FillOptions();
    }

}
