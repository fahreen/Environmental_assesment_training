using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity  = 200f;

    public Transform playerCharacter;
    private PlayerContoller playerContoller;

    private float xRotation = 0f;
    private bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        playerContoller = GetComponentInParent<PlayerContoller>();
        //hide cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerContoller.isTalking && canMove)
        {
            float xInput = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float yInput = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;


            xRotation -= yInput;
            //stop the player from looking too far up or down
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerCharacter.Rotate(Vector3.up * xInput);
        }
        
    }

    public void lockCamera()
    {
        canMove = false;
    }
    public void unlockCamera()
    {
        canMove = true;
    }
}
