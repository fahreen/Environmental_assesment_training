using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Fahreen Bushra:keep track of carpool groups

public class Manager : MonoBehaviour
{

    //singleton
    
    private static Manager instance;


    [Header("Dialogue UI")]


    public static List<Employee> AllEmployees;
    public static List<Employee> EmployeesSpokenTo;

    public static Dictionary<string, List<Employee>> carpoolGroups;



    // carpool groups
    public List<Employee> yellowCarpoolGroup;
    public List<Employee> greenCarpoolGroup;
    public List<Employee> blueCarpoolGroup;


    //carbool Bools
    public bool yellowCarpoolChanged;
    public bool greenCarpoolChanged;
    public bool blueCarpoolChanged;



    // carpool paths
    public  List<List<Vector2Int>> yellowCarpoolPath;
 
    public List<List<Vector2Int>> greenCarpoolPath;
 
    public List<List<Vector2Int>> blueCarpoolPath;

    // Start is called before the first frame update



    //singleton pattern
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one  Manager!");
        }


        instance = this;
    }

    public static Manager GetInstance()
    {
        return instance;
    }




    void Start()
    {

        // pick the scriptable object map to base the level off of

        // instantiate the employees in the scene based on the map chosen randomly

        
        // for all players in the scene, add them to : AllEmployees
        EmployeesSpokenTo = new List<Employee>();
        AllEmployees = new List<Employee>();

        carpoolGroups = new Dictionary<string, List<Employee>>();
        carpoolGroups.Add("yellow", new List<Employee>());
        carpoolGroups.Add("blue", new List<Employee>());
        carpoolGroups.Add("green", new List<Employee>());

    }

}
