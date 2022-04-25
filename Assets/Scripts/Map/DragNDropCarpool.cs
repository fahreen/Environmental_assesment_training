using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

//Fahreen Bushra: control carpool group positioning on the map.  Enable user to assign employees a carpool group.


public class DragNDropCarpool : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private RectTransform rectTransform;
    //[SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;

    public Tilemap pathLayer;

    private Vector3Int initialPosition;
    //  private Employee lastEmployeeAttached = null;
    private Employee lastEmployeeAttached = null;

    private bool attached;

 //   public Text DistanceUI;


    private void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
        canvasGroup = this.GetComponent<CanvasGroup>();

        attached = false;
    initialPosition = pathLayer.WorldToCell(this.transform.position);


    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("OnBeginDrag");
        // canvasGroup.alpha = 0f;
        // canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("Ondrag");
        rectTransform.anchoredPosition += (eventData.delta / 48);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
     
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPos = pathLayer.WorldToCell(mousePosition);

        for (int i = 0; i < Manager.AllEmployees.Count; i++)
        {
            if (IsInRectangle(gridPos, Manager.AllEmployees[i].locationPos) && Manager.AllEmployees[i].carpoolGroupCircle == null && Manager.AllEmployees[i].HasBusPass == false && attached == false && Manager.AllEmployees[i].hasTalked ==true)
            {

                this.transform.position = pathLayer.CellToWorld(new Vector3Int(Manager.AllEmployees[i].locationPos.x , Manager.AllEmployees[i].locationPos.y , Manager.AllEmployees[i].locationPos.z));
                attached = true;
                // get list, and add current emplyee to it
                // Manager.carpoolGroups[this.tag].Add(Manager.AllEmployees[i]);


                if (this.tag == "yellow")
                {
                    if (!Manager.GetInstance().yellowCarpoolGroup.Contains(Manager.AllEmployees[i]))
                    {
                        Manager.GetInstance().yellowCarpoolGroup.Add(Manager.AllEmployees[i]);
                        Manager.GetInstance().yellowCarpoolGroup.Sort(SortBympg);

                        //Alert that the carpool path has changed so that the path can be recalculated
                        Manager.GetInstance().yellowCarpoolChanged = true;
                    }
                }
                else if (this.tag == "green")
                {
                    if (!Manager.GetInstance().greenCarpoolGroup.Contains(Manager.AllEmployees[i]))
                    {
                        Manager.GetInstance().greenCarpoolGroup.Add(Manager.AllEmployees[i]);
                        Manager.GetInstance().greenCarpoolGroup.Sort(SortBympg);
                        Manager.GetInstance().greenCarpoolChanged = true;
                    }
                }
                else if (this.tag == "blue")
                {
                    if (!Manager.GetInstance().blueCarpoolGroup.Contains(Manager.AllEmployees[i]))
                    {
                        Manager.GetInstance().blueCarpoolGroup.Add(Manager.AllEmployees[i]);
                        Manager.GetInstance().blueCarpoolGroup.Sort(SortBympg);


                        Manager.GetInstance().blueCarpoolChanged = true;

                    }

                    
                }
              
               Manager.AllEmployees[i].carpoolGroupCircle = this.gameObject;

                lastEmployeeAttached = Manager.AllEmployees[i];

           
                return;

            }




        }


        if (this.lastEmployeeAttached != null && lastEmployeeAttached.carpoolGroupCircle != null )
        {

          
            lastEmployeeAttached.carpoolGroupCircle = null;
            lastEmployeeAttached.distanceTemp = lastEmployeeAttached.distance;

            if (this.tag == "yellow")
            {
                Manager.GetInstance().yellowCarpoolGroup.Remove(lastEmployeeAttached);
                
                Manager.GetInstance().yellowCarpoolChanged = true;
            }
            else if (this.tag == "green")
            {
                Manager.GetInstance().greenCarpoolGroup.Remove(lastEmployeeAttached);
                Manager.GetInstance().greenCarpoolChanged = true;
            }
            else if (this.tag == "blue")
            {
                Manager.GetInstance().blueCarpoolGroup.Remove(lastEmployeeAttached);
                Manager.GetInstance().blueCarpoolChanged = true;
            }

        }

        // if not attached to a house
        this.transform.position = pathLayer.CellToWorld(initialPosition);
        attached = false;


        //  canvasGroup.alpha = 1f;
        // canvasGroup.blocksRaycasts = true;
    }


    static int SortBympg(Employee e1, Employee e2)
    {
        return e1.mpg.CompareTo(e2.mpg);

        //.score.CompareTo(p2.score);
    }





    public bool IsInRectangle(Vector3Int pointClicked, Vector3Int topLeft)
    {

        int x1 = topLeft.x;
        int y1 = topLeft.y;

        int x2 = topLeft.x + 8;
        int y2 = topLeft.y - 8;

        int x = pointClicked.x;
        int y = pointClicked.y;

        if (x >= x1 & x <= x2 & y <= y1 & y >= y2)
        {
            return true;
        }


        else
            return false;
    }









}
