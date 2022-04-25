using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EmployeeController : MonoBehaviour
{
    
    public float wanderRange = 3.0f;
    public float walkTimer = 5.0f;
    public Employee employeeData;
    public Canvas talkingUI;

    private NavMeshAgent employeeAgent;
    private Animator animator;
    private float currentTimer;
    private GameObject playerCharacter;
    private bool notTalking = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        employeeAgent = GetComponent<NavMeshAgent>();
        currentTimer = walkTimer;
        playerCharacter = GameObject.Find("Player");
        talkingUI.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {  
        if(notTalking)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= walkTimer)
            {
                Vector3 newPosition = RandomPosition(transform.position, wanderRange, -1);
                employeeAgent.SetDestination(newPosition);
                currentTimer = 0;
                
            }
            if(employeeAgent.velocity.magnitude > 0)
            {
                Debug.Log("moving");
                animator.SetBool("isMoving", true);
            }
            else
            {
                Debug.Log("not moving");
                animator.SetBool("isMoving", false);
            }
        }
    }

    //Pick a random position for the employee to walk to
    //Position will be within wanderDistance from origin and on the same layer as layermask
    public static Vector3 RandomPosition(Vector3 origin, float wanderDistance, int layermask)
    {
        Vector3 randomVector3 = Random.insideUnitSphere * wanderDistance; 

        randomVector3 += origin;

        UnityEngine.AI.NavMeshHit newNavMesh;

        UnityEngine.AI.NavMesh.SamplePosition (randomVector3, out newNavMesh, wanderDistance, layermask);
 
        return newNavMesh.position; 
    }

    //To be called when an employee is being talked to 
    public void DisplayDialog()
    {
        transform.LookAt(playerCharacter.transform);
        animator.SetBool("isMoving", false);
        notTalking = false;
        talkingUI.enabled = true;
    }

    public void RemoveDialog()
    {
        notTalking = true;
        talkingUI.enabled = false;
    }
}
