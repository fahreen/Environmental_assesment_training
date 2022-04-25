using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.Tilemaps;


public class Map : MonoBehaviour
{

    // ALL VALUES ASSIGNED IN INSPECTOR
    // tile map grid
    public GameObject MapGrid;
    
    // tile map layers
    public Tilemap layer0;
    public Tilemap layer2;
    public Tilemap path;

    // postion of workplace 
    public Vector3Int endPos;

    // List of employees and their information encoded as strings, later use this to instantiate employees and set th eir scriptable object values
   public List<string> employeeData = new List<string>(); //Encoding --> name/InstantiatePos/startPos/locationPos/Address/CarType/Distance/mpg


}

