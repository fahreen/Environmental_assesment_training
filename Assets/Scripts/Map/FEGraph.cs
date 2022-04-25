using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
public class FEGraph : MonoBehaviour
{
//Fahreen Bushra: show graph to display fuel effeciency per employee.  Must be updated, at this time graph is static

    private RectTransform graphContainer;
    [SerializeField] private Sprite circleSprite;
    public Text total;


    private void Start()
    {
        this.graphContainer = transform.Find("graph_container").GetComponent<RectTransform>();

        

    }

    private GameObject CreateCircle(Vector2 anchorPosition)
    {
        // create a new image object
        GameObject gameObject = new GameObject("circle", typeof(Image));
        // set the image's parent to the graph
        gameObject.transform.SetParent(this.graphContainer, false);
        // set the image's sprite to a circle 
        gameObject.GetComponent<Image>().sprite = circleSprite; 
        // set position of the sprite, given by function arguement
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchorPosition;
        rectTransform.sizeDelta = new Vector2(7, 7);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
        

    }

    public void ShowGraph(List<float> valueList)
    {
        float graphHeight = this.graphContainer.sizeDelta.y;
        float yMaximum = 3f; // top of graph
        float xSize = 25f; // size between each unit

        GameObject lastDotGameObject = null;
        for (int i = 0; i< valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            
            GameObject dotGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if(lastDotGameObject != null)
            {
                CreateDotConnection(lastDotGameObject.GetComponent<RectTransform>().anchoredPosition, dotGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastDotGameObject = dotGameObject;
            
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(this.graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        
        // get distance and direction between two dots
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);


        rectTransform.anchorMin = new Vector2(0,0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 1f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * 0.5f;

        
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir)); // take a vector, and calculate the rotation in terms of 0-360
    }


}
