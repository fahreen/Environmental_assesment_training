using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Script not used at this time

public class CameraControllerMAP : MonoBehaviour
{

    public Canvas MapUI;
    public EventSystem mapEventSystem;

    private void OnEnable()
    {

       // MapUI.GetComponent<CanvasGroup>().alpha = 1f;
       // MapUI.GetComponent<CanvasGroup>().blocksRaycasts = true; 
       // mapEventSystem.enabled = true;
        
    }

    private void OnDisable()
    {
       // MapUI.GetComponent<CanvasGroup>().alpha = 0f;
       // MapUI.GetComponent<CanvasGroup>().blocksRaycasts = false; //this prevents the UI element to receive input events
       // mapEventSystem.enabled = false;
    }
}
