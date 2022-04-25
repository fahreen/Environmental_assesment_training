using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Results : MonoBehaviour
{
    public Text evaluationScoreText;
    public Text playerCarbonEmissionScore;
    // Start is called before the first frame update
    void Start()
    {
        //TODO: GET PLAYER EVALUATION SCORE TO USE AS PART OF TEXT SEGMENT
        evaluationScoreText.text = "Evaluation Score: " + FinalQuestions.questionList.Count.ToString() + "/" + FinalQuestions.questionList.Count.ToString();
        //TODO GET PLAYER EMISSION SCORE
        playerCarbonEmissionScore.text = "Player Emission Rating: " + "GOOD";
        Debug.Log(FinalQuestions.questionList.Count);
    }
}
