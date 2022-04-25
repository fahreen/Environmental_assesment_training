using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Fahreen Bushra: Implement player movement and interaction with game world objects in 3D office space

public class PlayerContoller : MonoBehaviour
{
    public CharacterController playerCharContoller;

    public float playerSpeed = 5f;
    public float gravity = -9.81f;
    public float interactRange = 1.5f;

    //UI elements
    public Text interactPrompt;
    public Text exitPrompt;
    public Canvas playerCanvas;

    Vector3 playerVelocity;

    public Camera main;

    private bool canMove = true;
    private CameraMovement camMovement;
    [HideInInspector]
    public bool isTalking;



    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("Scenes/Map", LoadSceneMode.Additive);
        SceneManager.LoadScene("Scenes/EnergyUsage", LoadSceneMode.Additive);
        playerVelocity.y = gravity;
        //hide interact prompts
        interactPrompt.transform.localScale = new Vector3(0,0,0);
        exitPrompt.transform.localScale = new Vector3(0,0,0);
        camMovement = main.GetComponent<CameraMovement>();
        isTalking = false;
        
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.U)){

            //  SceneManager.UnloadSceneAsync("Scenes/Map");
            // insert a boole check here
            main.enabled = true;
            playerCanvas.enabled = true;
            GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>().MapUI.GetComponent<CanvasGroup>().alpha = 0f;
            //disable energyUI if active
            GameObject.FindGameObjectWithTag("EnergyManager").GetComponent<EnergyUsageManager>().energyCam.enabled = false;
            GameObject.FindGameObjectWithTag("EnergyManager").GetComponent<EnergyUsageManager>().EnergyUI.enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }

        if (!isTalking && canMove)
        {
            float xInput = Input.GetAxis("Horizontal");
            float zInput = Input.GetAxis("Vertical");

            Vector3 movementVector = transform.right * xInput + transform.forward * zInput;
            playerCharContoller.Move(movementVector * playerSpeed * Time.deltaTime);

            playerCharContoller.Move(playerVelocity * Time.deltaTime);
        }

        RaycastHit hit;
        //raycast to see if player can interact with object
        Debug.DrawRay(main.transform.position, main.transform.TransformDirection(Vector3.forward), Color.yellow);
        if (Physics.Raycast(main.transform.position, main.transform.TransformDirection(Vector3.forward), out hit, interactRange, LayerMask.GetMask("Interactable")))
        {
            //prompt player to interact with document
            if (canMove)
            {
                interactPrompt.transform.localScale = new Vector3(1,1,1);
            }
            
            DocumentHandler doc = hit.collider.GetComponent<DocumentHandler>();

            if (doc != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //lock player and camera movement
                    canMove = false;
                    camMovement.lockCamera();
                    //hide UI prompt
                    interactPrompt.transform.localScale = new Vector3(0,0,0);
                    //show exit prompt
                    exitPrompt.transform.localScale = new Vector3(1,1,1);
                    doc.viewDocument();
                }
                
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    canMove = true;
                    camMovement.unlockCamera();
                    exitPrompt.transform.localScale = new Vector3(0,0,0);
                    doc.closeDocument();
                }

            }
            
        }
        else if (Physics.Raycast(main.transform.position, main.transform.TransformDirection(Vector3.forward), out hit, interactRange, LayerMask.GetMask("Employee")))
        {
            Debug.Log("target hit");
            if (canMove)
            {
                interactPrompt.transform.localScale = new Vector3(1,1,1);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                //lock player and camera movement
                canMove = false;
                camMovement.lockCamera();
                //hide UI prompt
                interactPrompt.transform.localScale = new Vector3(0,0,0);
                //show exit prompt
                exitPrompt.transform.localScale = new Vector3(1,1,1);
            }
            
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                canMove = true;
                camMovement.unlockCamera();
                exitPrompt.transform.localScale = new Vector3(0,0,0);
            }

        }
        else
        {
            //disable interact prompt
            interactPrompt.transform.localScale = new Vector3(0,0,0);
        }


    }
    


}
