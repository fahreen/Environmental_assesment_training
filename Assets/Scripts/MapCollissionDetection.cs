using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Fahreen Bushra: Detects when player has collided with map. Changes camera from 3D office scene to 2D map scene

public class MapCollissionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera main;
    public GameObject player;
    public Canvas playerCanvas;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
            //main.GetComponent<CameraMovement>().enabled = false;
            main.enabled = false;
            playerCanvas.enabled = false;

            GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>().MapUI.GetComponent<CanvasGroup>().alpha = 1f;
           
            Cursor.lockState = CursorLockMode.None;
        }
    }


}
