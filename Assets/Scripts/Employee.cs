using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Employee", menuName = "Employee")]
public class Employee : ScriptableObject
{
    public string employeeName;
    public Vector3Int startPos; // position of employee on map
    public Vector3 instantiatePos; // position of employee in 3D office
    public Vector3Int locationPos; // position of the employee house tile(tope left square)
    public string address;
    public string carType;
    public float mpg;
    public List<Vector2Int> path;
    public int distance;
    public int distanceTemp;
    public bool pathCalculated;
    public bool carpool;
    public bool showPath = false;
    public bool hasTalked;
    public Sprite Headshot;
    public bool HasBusPass;
    public GameObject carpoolGroupCircle;






    //distances the employee travels to work
    //public int xTravelDistance;
    //public int yTravelDistacne;C 

}
