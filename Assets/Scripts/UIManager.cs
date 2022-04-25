using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public void loadEnergyUsage()
    {
        //enable UI of page
        Debug.Log("pressed");
        GameObject.FindGameObjectWithTag("EnergyManager").GetComponent<EnergyUsageManager>().energyCam.enabled = true;
        GameObject.FindGameObjectWithTag("EnergyManager").GetComponent<EnergyUsageManager>().EnergyUI.enabled = true;
        //disable ui of other page(s)
        GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>().MapUI.GetComponent<CanvasGroup>().alpha = 0f;
    }
    public void loadCarpoolMap()
    {
        //disable UI of other page(s)
        GameObject.FindGameObjectWithTag("EnergyManager").GetComponent<EnergyUsageManager>().energyCam.enabled = false;
        GameObject.FindGameObjectWithTag("EnergyManager").GetComponent<EnergyUsageManager>().EnergyUI.enabled = false;

        //enable UI of page
        GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>().MapUI.GetComponent<CanvasGroup>().alpha = 1f;

    }
}
