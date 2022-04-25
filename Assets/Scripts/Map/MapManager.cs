using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public class MapManager : MonoBehaviour
{
    // set in inspector
    public Tile pathTile;
    public Tile carpoolTile;
    public Tile replacementTile;

    public List<GameObject> allMaps;
    public GameObject[] employeePrefabs;
    public GameObject fueleffeciencyGraph;


    // constant values
    private const int ROW = 74;
    private const int COL = 42;
    private int[,] binaryMap = new int[ROW, COL];
    private int[,] tempBinaryMap = new int[ROW, COL];


    private GameObject map;
    private Tilemap layer2;
    private Tilemap layer1;
    private Tilemap pathLayer;
    private Vector3Int endPos;


    // Text UI elements
    public Canvas MapUI;
    // public GameObject ProfileUI;
    public Text NameUI;
    public Text AddressUI;
    public Text DistanceUI;
    public Text CarUI;
    public Text mpgUI;
    public Image EmployeeImageUI;
    public Sprite annonomousImage;



    //Carpool UI elements
    public GameObject CarpoolUI;
    public List<Image> CarpoolImages;
    public Text carpoolTitle;
    public Text CarpoolDriver;
    public Text carpoolDistance;
    public Text carpoolCarUI;
    public Text carpoolmpgUI;

    public Image carpoolBorder;

    Color yellow;
    Color blue;
    Color green;





    void Start()
    {
        this.yellow = new Color32(231, 208, 0, 255);
        this.blue = new Color32(11, 110, 255, 255);
        this.green = new Color32(34, 177, 76, 255);
        //randomly pick a level
        int k = 0;

        //set variables for this game
        Map script = allMaps[k].GetComponent<Map>();
        this.map = allMaps[k];
        this.layer2 = script.layer2;
        this.pathLayer = script.path;
        this.endPos = script.endPos;

        // enable map
        map.SetActive(true);
        //map.SetTile(new Vector3Int(-16, 24, 0), tile);
        // Convert map to binary map 
        //layer1.enabled = false;
        //layer1.GetComponent<TilemapRenderer>().enabled = false;




        // iterate through all employees, instantiate them and set their data

        for (int numOfEmployees = 0; numOfEmployees < script.employeeData.Count; numOfEmployees++)
        {
            // get name
            string[] employee = script.employeeData[numOfEmployees].Split(char.Parse("/"));
            Debug.Log(employee[0]);

            // get position for prefab instantiation
            string[] pos = employee[1].Split(char.Parse(","));
            Vector3 InstantiatePos = new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));

            // find emplyee prefab by name, instantiate them and set their data(through their scritable object
            for (int i = 0; i < employeePrefabs.Length; i++)
            {
                if (employeePrefabs[i].name == employee[0])
                {
                    //Debug.Log(employee[i]);
                    // instantiate prefab and get access to scriptable object of the prefab
                    Employee data = Instantiate(employeePrefabs[i], InstantiatePos, employeePrefabs[i].transform.rotation).GetComponent<EmployeeController2>().data;

                    data.instantiatePos = InstantiatePos;

                    // set position of the start of the path to work
                    pos = employee[2].Split(char.Parse(","));
                    data.startPos = new Vector3Int(int.Parse(pos[0]), int.Parse(pos[1]), int.Parse(pos[2]));


                    // set position home (top left square)
                    pos = employee[3].Split(char.Parse(","));
                    data.locationPos = new Vector3Int(int.Parse(pos[0]), int.Parse(pos[1]), int.Parse(pos[2]));

                    // set the rest of the variables
                    data.address = employee[4];
                    data.carType = employee[5];
                    data.distance = int.Parse(employee[6]);
                    data.distanceTemp = data.distance;
                    data.mpg = int.Parse(employee[7]);
                    data.pathCalculated = false;
                    data.hasTalked = false;
                    data.carpool = false;
                    data.HasBusPass = false;

                    // add the employee sriptable object to static list
                    Manager.AllEmployees.Add(data);

                    this.MapUI.GetComponent<CanvasGroup>().alpha = 0f;
                    this.CarpoolUI.gameObject.SetActive(false);

                }
            }


        }









        // create binary map
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                int x = i - 34;
                int y = 26 - j;

                Tile current = (Tile)pathLayer.GetTile(new Vector3Int(x, y, 0));
                if (current == pathTile)
                {
                    //map.SetTile(new Vector3Int(x, y, 0), replacementTile);
                    binaryMap[i, j] = 0;
                }
                else
                {
                    binaryMap[i, j] = 1;
                }
            }
        }



        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                sb.Append(binaryMap[i, j]);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());


        // Draw graph
        List<float> valueList = new List<float>();
        float t = 0;
        for (int point = 0; point < Manager.AllEmployees.Count; point++)
        {
            float num = Manager.AllEmployees[point].distance / Manager.AllEmployees[point].mpg;
            valueList.Add(num);
            t = t + num;
        }
        this.fueleffeciencyGraph.GetComponent<FEGraph>().ShowGraph(valueList);
        this.fueleffeciencyGraph.GetComponent<FEGraph>().total.text = "Total Gas Mileage: " + t;

        //Total Gas Mileage:


    }





    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = pathLayer.WorldToCell(mousePosition);
            Debug.Log("Current pos = " + gridPos);


            for (int i = 0; i < Manager.AllEmployees.Count; i++)
            {
                if (IsInRectangle(gridPos, Manager.AllEmployees[i].locationPos))
                {
                    Debug.Log("YES");



                    //DrawPath(new Vector2Int(Manager.AllEmployees[i].startPos.x, Manager.AllEmployees[i].startPos.y), new Vector2Int(this.endPos.x, this.endPos.y));
                    if (Manager.AllEmployees[i].pathCalculated == false)
                    {
                        //DrawPath(Manager.AllEmployees[i].startPos, this.endPos);
                        Manager.AllEmployees[i].path = CalculatePath(Manager.AllEmployees[i].startPos, this.endPos);
                        Manager.AllEmployees[i].pathCalculated = true;

                    }
                    if (Manager.AllEmployees[i].showPath == false)
                    {
                        //Check for carpool
                        //if curent employee is set to carpool show carpool path
                        // draw path on screen
                        if (Manager.AllEmployees[i].carpoolGroupCircle != null)
                        {
                            Debug.Log(Manager.AllEmployees[i].carpoolGroupCircle.tag);

                            CalculateCarpoolGroupPath(Manager.AllEmployees[i].carpoolGroupCircle.tag);


                            //DrawPath(Manager.GetInstance().yellowCarpoolPath);
                        }

                        else
                        {



                            DrawPath(Manager.AllEmployees[i].path);


                        }

                        Manager.AllEmployees[i].showPath = true;



                        // Display UI ELEMENTS

                        if (Manager.AllEmployees[i].hasTalked)
                        {
                            // if employee iis part of the carpool group
                            if (Manager.AllEmployees[i].carpoolGroupCircle != null)
                            {
                                this.CarpoolUI.gameObject.SetActive(true);
                                //.ProfileUI.gameObject.SetActive(false);






                                if (Manager.AllEmployees[i].carpoolGroupCircle.tag == "yellow")
                                {
                                    //update text ui
                                    this.carpoolBorder.color = this.yellow;
                                    this.carpoolTitle.text = "CARPOOL GROUP: A";
                                    this.CarpoolDriver.text =  Manager.GetInstance().yellowCarpoolGroup[0].name +" is driving for this Group!";
                                    this.carpoolDistance.text = "Total Distance: " + Manager.GetInstance().yellowCarpoolGroup[0].distanceTemp.ToString();
                                    this.carpoolmpgUI.text ="Total MPG: "+  Manager.GetInstance().yellowCarpoolGroup[0].mpg.ToString();
                                    this.carpoolCarUI.text = "CAR: " + Manager.GetInstance().yellowCarpoolGroup[0].carType;

                                    //Update image ui
                                    int x = 0;
                                    for (int ci = 0; ci < Manager.GetInstance().yellowCarpoolGroup.Count; ci++)
                                    {
                                        this.CarpoolImages[ci].gameObject.SetActive(true);
                                        this.CarpoolImages[ci].overrideSprite = Manager.GetInstance().yellowCarpoolGroup[ci].Headshot;
                                        

                                        x = ci + 1;
                                    }

                                    for (int noCi = x; noCi < this.CarpoolImages.Count; noCi++)
                                    {
                                        this.CarpoolImages[noCi].gameObject.SetActive(false);
                                    }


                                }


                                else if (Manager.AllEmployees[i].carpoolGroupCircle.tag == "blue")
                                {
                                    this.carpoolBorder.color = this.blue;
                                    this.carpoolTitle.text = "CARPOOL GROUP: B";
                                    this.CarpoolDriver.text = Manager.GetInstance().blueCarpoolGroup[0].name + " is driving for this Group!";
                                    this.carpoolDistance.text = "Total Distance: " + Manager.GetInstance().blueCarpoolGroup[0].distanceTemp.ToString();
                                    this.carpoolmpgUI.text = "Total MPG: " + Manager.GetInstance().blueCarpoolGroup[0].mpg.ToString();
                                    this.carpoolCarUI.text = "CAR: " + Manager.GetInstance().blueCarpoolGroup[0].carType;
                                    int x = 0;
                                    for (int ci = 0; ci < Manager.GetInstance().blueCarpoolGroup.Count; ci++)
                                    {
                                        this.CarpoolImages[ci].gameObject.SetActive(true);
                                        this.CarpoolImages[ci].overrideSprite = Manager.GetInstance().blueCarpoolGroup[ci].Headshot;

                                        x = ci + 1;
                                    }

                                    for (int noCi = x; noCi < this.CarpoolImages.Count; noCi++)
                                    {
                                        this.CarpoolImages[noCi].gameObject.SetActive(false);
                                    }


                                }

                                else if (Manager.AllEmployees[i].carpoolGroupCircle.tag == "green")
                                {
                                    this.carpoolBorder.color = this.green;
                                    this.carpoolTitle.text = "CARPOOL GROUP: C";
                                    this.CarpoolDriver.text = Manager.GetInstance().greenCarpoolGroup[0].name + " is driving for this Group!";
                                    this.carpoolDistance.text = "Total Distance: " + Manager.GetInstance().greenCarpoolGroup[0].distanceTemp.ToString();
                                    this.carpoolmpgUI.text = "Total MPG: " + Manager.GetInstance().greenCarpoolGroup[0].mpg.ToString();
                                    this.carpoolCarUI.text = "CAR: " + Manager.GetInstance().greenCarpoolGroup[0].carType;
                                    int x = 0;
                                    for (int ci = 0; ci < Manager.GetInstance().greenCarpoolGroup.Count; ci++)
                                    {
                                        this.CarpoolImages[ci].gameObject.SetActive(true);
                                        this.CarpoolImages[ci].overrideSprite = Manager.GetInstance().greenCarpoolGroup[ci].Headshot;

                                        x = ci + 1;
                                    }

                                    for (int noCi = x; noCi < this.CarpoolImages.Count; noCi++)
                                    {
                                        this.CarpoolImages[noCi].gameObject.SetActive(false);
                                    }


                                }





                            }
                            else
                            {
                                this.CarpoolUI.gameObject.SetActive(false);
                                
                            }

                            // show employee profile
                            this.NameUI.text = "Name: " + Manager.AllEmployees[i].name;
                            this.AddressUI.text = "Address: " + Manager.AllEmployees[i].address;
                            this.DistanceUI.text = "Distance: " + Manager.AllEmployees[i].distance;
                            this.CarUI.text = "Car: " + Manager.AllEmployees[i].carType;
                            this.mpgUI.text = "mpg: " + Manager.AllEmployees[i].mpg;
                            this.EmployeeImageUI.overrideSprite = Manager.AllEmployees[i].Headshot;
                        }

                        else
                        {
                            this.CarpoolUI.gameObject.SetActive(false);
                            // show employee profile
                            this.NameUI.text = "Name: ?";
                            this.AddressUI.text = "Address: ?";
                            this.DistanceUI.text = "Distance: ? ";
                            this.CarUI.text = "Car: ?";
                            this.mpgUI.text = "mpg: ?";
                            this.EmployeeImageUI.overrideSprite = this.annonomousImage;



                        }




                    }
                    else
                    {
                        if (Manager.AllEmployees[i].carpoolGroupCircle == null)
                        {
                            ErasePath(Manager.AllEmployees[i].path);
                        }
                        else
                        {
                            ErasePathCarpool(Manager.AllEmployees[i].carpoolGroupCircle.tag);
                        }

                        Manager.AllEmployees[i].showPath = false;
                    }
                }


            }



        }
    }








    public void CalculateCarpoolGroupPath(string groupColor)
    {

        if (groupColor == "yellow")
        {
            if (!Manager.GetInstance().yellowCarpoolChanged)
            {
                //return 
                Debug.Log("Path Has not changed " + groupColor);
            }
            else
            {
                // calculate the path is something has changed
                Debug.Log("Recalculating path for " + groupColor);
                Manager.GetInstance().yellowCarpoolPath = new List<List<Vector2Int>>();

                int x = 0;
                for (int i = 0; i < Manager.GetInstance().yellowCarpoolGroup.Count - 1; i++)
                {
                    Vector3Int e1 = Manager.GetInstance().yellowCarpoolGroup[i].startPos;
                    Vector3Int e2 = Manager.GetInstance().yellowCarpoolGroup[i + 1].startPos;
                    Manager.GetInstance().yellowCarpoolPath.Add(CalculatePath(e1, e2));
                    x = i + 1;
                }

                Vector3Int lastEmp = Manager.GetInstance().yellowCarpoolGroup[x].startPos;
                Manager.GetInstance().yellowCarpoolPath.Add(CalculatePath(lastEmp, this.endPos));
                
                // update the distances for the employees
                
                // For Everyone who is not the driver, set the distance to 0
                for (int yc = 1; yc < Manager.GetInstance().yellowCarpoolGroup.Count; yc++)
                {
                    Manager.GetInstance().yellowCarpoolGroup[yc].distanceTemp = 0;
                }
                // Set the distance for the driver to the size of the carpool path
                int totalDistance = 0;
                for(int q = 0; q < Manager.GetInstance().yellowCarpoolPath.Count; q++)
                {
                    totalDistance += Manager.GetInstance().yellowCarpoolPath[q].Count;
                }

                Manager.GetInstance().yellowCarpoolGroup[0].distanceTemp = totalDistance;


            }
            // draw the path
            for (int i = 0; i < Manager.GetInstance().yellowCarpoolPath.Count; i++)
            {
                DrawPathCarpool(Manager.GetInstance().yellowCarpoolPath[i]);
            }


            Manager.GetInstance().yellowCarpoolChanged = false;

           





        }


        else if (groupColor == "blue")
        {
            if (!Manager.GetInstance().blueCarpoolChanged)
            {
                //return 
                Debug.Log("Path Has not changed " + groupColor);
            }
            else
            {
                // calculate the path is something has changed
                Debug.Log("Recalculating path for " + groupColor);
                Manager.GetInstance().blueCarpoolPath = new List<List<Vector2Int>>();
                // sort carpool group by mpg
                // Manager.GetInstance().yellowCarpoolGroup.Sort(SortBympg);
                int x = 0;
                for (int i = 0; i < Manager.GetInstance().blueCarpoolGroup.Count - 1; i++)
                {
                    Vector3Int e1 = Manager.GetInstance().blueCarpoolGroup[i].startPos;
                    Vector3Int e2 = Manager.GetInstance().blueCarpoolGroup[i + 1].startPos;
                    Manager.GetInstance().blueCarpoolPath.Add(CalculatePath(e1, e2));
                    x = i + 1;
                }

                Vector3Int lastEmp = Manager.GetInstance().blueCarpoolGroup[x].startPos;
                Manager.GetInstance().blueCarpoolPath.Add(CalculatePath(lastEmp, this.endPos));
            }
            // draw the path
            for (int i = 0; i < Manager.GetInstance().blueCarpoolPath.Count; i++)
            {
                DrawPathCarpool(Manager.GetInstance().blueCarpoolPath[i]);
            }


            Manager.GetInstance().blueCarpoolChanged = false;

        }



        else if (groupColor == "green")
        {
            if (!Manager.GetInstance().greenCarpoolChanged)
            {
                //return 
                Debug.Log("Path Has not changed " + groupColor);
            }
            else
            {
                // calculate the path is something has changed
                Debug.Log("Recalculating path for " + groupColor);
                Manager.GetInstance().greenCarpoolPath = new List<List<Vector2Int>>();

                // sort carpool group
                // sort carpool group by mpg
                //  Manager.GetInstance().yellowCarpoolGroup.Sort(SortBympg); 

                int x = 0;
                for (int i = 0; i < Manager.GetInstance().greenCarpoolGroup.Count - 1; i++)
                {
                    Vector3Int e1 = Manager.GetInstance().greenCarpoolGroup[i].startPos;
                    Vector3Int e2 = Manager.GetInstance().greenCarpoolGroup[i + 1].startPos;
                    Manager.GetInstance().greenCarpoolPath.Add(CalculatePath(e1, e2));
                    x = i + 1;
                }



                Vector3Int lastEmp = Manager.GetInstance().greenCarpoolGroup[x].startPos;
                Manager.GetInstance().greenCarpoolPath.Add(CalculatePath(lastEmp, this.endPos));
            }
            // draw the path
            for (int i = 0; i < Manager.GetInstance().greenCarpoolPath.Count; i++)
            {
                DrawPathCarpool(Manager.GetInstance().greenCarpoolPath[i]);
            }


            Manager.GetInstance().greenCarpoolChanged = false;

        }





    }




    void DrawPathCarpool(List<Vector2Int> path)
    {

        for (int p = 0; p < path.Count; p++)
        {
            // pathLayer.SetTile(new Vector3Int(path[p].x - 34, 26 - path[p].y, 0), replacementTile);
            layer2.SetTile(new Vector3Int(path[p].x - 34, 26 - path[p].y, 0), carpoolTile);
        }


    }



    bool IsInRectangle(Vector3Int pointClicked, Vector3Int topLeft)
    {

        int x1 = topLeft.x;
        int y1 = topLeft.y;

        int x2 = topLeft.x + 5;
        int y2 = topLeft.y - 5;

        int x = pointClicked.x;
        int y = pointClicked.y;

        if (x >= x1 & x <= x2 & y <= y1 & y >= y2)
        {
            return true;
        }


        else
            return false;
    }



    void DrawPath(List<Vector2Int> path)
    {

        for (int p = 0; p < path.Count; p++)
        {
            // pathLayer.SetTile(new Vector3Int(path[p].x - 34, 26 - path[p].y, 0), replacementTile);
            layer2.SetTile(new Vector3Int(path[p].x - 34, 26 - path[p].y, 0), pathTile);
        }


    }







    void ErasePath(List<Vector2Int> path)
    {



        for (int p = 0; p < path.Count; p++)
        {
            // pathLayer.SetTile(new Vector3Int(path[p].x - 34, 26 - path[p].y, 0), this.pathTile);

            layer2.SetTile(new Vector3Int(path[p].x - 34, 26 - path[p].y, 0), null);
        }


    }


    void ErasePathCarpool(string cg)

    {
        List<List<Vector2Int>> path;


        if (cg == "yellow")
        {
            path = Manager.GetInstance().yellowCarpoolPath;
        }
        else if (cg == "green")
        {
            path = Manager.GetInstance().greenCarpoolPath;
        }
        else
        {
            path = Manager.GetInstance().blueCarpoolPath;
        }


        for (int i = 0; i < path.Count; i++)
        {
            for (int p = 0; p < path[i].Count; p++)
            {
                // pathLayer.SetTile(new Vector3Int(path[p].x - 34, 26 - path[p].y, 0), this.pathTile);

                layer2.SetTile(new Vector3Int(path[i][p].x - 34, 26 - path[i][p].y, 0), null);
            }

        }



    }





    List<Vector2Int> CalculatePath(Vector3Int start, Vector3Int end)
    {

        // translate to binary graph coordinates
        start.x = start.x + 34;
        start.y = 26 - start.y;

        end.x = end.x + 34;
        end.y = 26 - end.y;



        // create a matrix of zeroes

        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                tempBinaryMap[i, j] = 0;
            }
        }

        tempBinaryMap[start.x, start.y] = 1;
        //Debug.Log("HERE");

        // find paths
        int k = 0;
        int a = end.x;
        int b = end.y;


        while (tempBinaryMap[a, b] == 0)
        {

            k++;
            make_step(k);
        }



        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                sb.Append(tempBinaryMap[i, j]);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        //Debug.Log(sb.ToString());

        // retrace and identify path
        k = tempBinaryMap[end.x, end.y]; // go to endpoint
        List<Vector2Int> path = new List<Vector2Int>(); // declare list for path

        while (k > 1)
        {
            //Debug.Log("HERE" + k);

            if (a > 0 && tempBinaryMap[a - 1, b] == k - 1)
            {
                a = a - 1;
                //b = b;
                path.Add(new Vector2Int(a, b));
                k--;
            }

            else if (b > 0 && tempBinaryMap[a, b - 1] == k - 1)
            {
                //a = a;
                b = b - 1;
                path.Add(new Vector2Int(a, b));
                k--;
            }

            else if (a < ROW - 1 && tempBinaryMap[a + 1, b] == k - 1)
            {
                a = a + 1;
                path.Add(new Vector2Int(a, b));
                k--;
            }
            else if (b < COL - 1 && tempBinaryMap[a, b + 1] == k - 1)
            {
                b = b + 1;
                path.Add(new Vector2Int(a, b));
                k--;
            }
        }

        return path;

    }




    // helper method
    void make_step(int k)
    {


        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                if (tempBinaryMap[i, j] == k)
                {

                    if (i > 0 && tempBinaryMap[i - 1, j] == 0 && binaryMap[i - 1, j] == 0)
                    {
                        tempBinaryMap[i - 1, j] = k + 1;
                    }
                    if (j > 0 && tempBinaryMap[i, j - 1] == 0 && binaryMap[i, j - 1] == 0)
                    {
                        tempBinaryMap[i, j - 1] = k + 1;
                    }
                    if (i < ROW - 1 && tempBinaryMap[i + 1, j] == 0 && binaryMap[i + 1, j] == 0)
                    {
                        tempBinaryMap[i + 1, j] = k + 1;
                    }
                    if (j < COL - 1 && tempBinaryMap[i, j + 1] == 0 && binaryMap[i, j + 1] == 0)
                    {
                        tempBinaryMap[i, j + 1] = k + 1;
                    }

                }
            }
        }

    }





}
