using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using UnityEngine.UI;

//Fahreen Bushra: animate + control talking vs not talking state.  Animation has not been implemented correctly yet

public class EmployeeController2 : MonoBehaviour
{
    public Employee data;
    Animator animator;
    public Canvas dialogCanvus;
    private bool talking;
    Quaternion savedRotation;
    private bool HasBeenAdded;

    PlayerContoller playerContoller;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        HasBeenAdded = false;
        talking = false;

        //dialogCanvus.gameObject.SetActive(true);
    }



    //display ui talk instruction for 5 seconds
    private void OnTriggerEnter(Collider other)
    {
        playerContoller = other.GetComponent<PlayerContoller>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerContoller>() != null)
        {
            playerContoller = null;
        }
    }

    private void Update()
    {
        if (playerContoller != null)
        {
            if (Input.GetKey("t") && talking == false)
            {
                BeginTalk();
            }
        }
    }




    public void BeginTalk()
    {

        Debug.Log("enabling canvus");
        // show dialog
        dialogCanvus.gameObject.SetActive(true);
        playerContoller.isTalking = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        animator.SetBool("isTalking", true);
        // save old position 
        savedRotation = this.transform.rotation;

        // turn employee toward the player
        Vector3 direction = playerContoller.transform.position - this.transform.position;
        Quaternion r = Quaternion.LookRotation(direction);
        r.x = this.transform.rotation.x;
        r.z = this.transform.rotation.z;
        this.transform.rotation = r;

        talking = true;
    }

    public void EndTalking(bool successful)
    {
        playerContoller.isTalking = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator.SetBool("isTalking", false);

        // close dialog box
        dialogCanvus.gameObject.SetActive(false);

        // go back to previous position
        this.transform.rotation = savedRotation;
        talking = false;
        Debug.Log(data.employeeName);
        // Collect current employee information
        if (successful)
        {
            //NEVER PRINTS
            Debug.Log(data.employeeName);

            data.hasTalked = true; 
            Manager.EmployeesSpokenTo.Add(this.data);
            HasBeenAdded = true;
        }
    }
}


