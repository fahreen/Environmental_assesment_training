using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalQuestions : MonoBehaviour
{
    public List<Toggle> toggleList = new List<Toggle>(); 
    public static List<bool> questionList = new List<bool>();

    void Start()
    {
        for (int i = 0; i < toggleList.Count; i++)
        {
            int x = i;
            toggleList[i].onValueChanged.AddListener(delegate{toggle(x);});
            questionList.Add(false);
        }
    }

    public void toggle(int index)
    {
        questionList[index] = !questionList[index];
        Debug.Log(index + " " + questionList[index]);
    }

    public void loadNextScene()
    {
        SceneManager.LoadScene("FinalResults");
    }

    
}
