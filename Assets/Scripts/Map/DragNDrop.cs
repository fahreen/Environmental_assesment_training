using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

//Fahreen Bushra: control bus pass positioning on the map.  Enable user to assign employees a bus pass.

public class DragNDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private RectTransform rectTransform;
    //[SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;

    public Tilemap pathLayer;

    private Vector3Int busHolderPosition;
    private Employee lastEmployeeAttached = null;


    public Text DistanceUI;


    private void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
        canvasGroup = this.GetComponent<CanvasGroup>();
  

        busHolderPosition = new Vector3Int(-29, -28, 0);


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
        rectTransform.anchoredPosition += (eventData.delta/48);
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag: ");

        // Debug.Log(eventData.position);

        // eventData.pressPosition
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPos = pathLayer.WorldToCell(mousePosition);

        for (int i = 0; i < Manager.AllEmployees.Count; i++)
        { 
            if (IsInRectangle(gridPos, Manager.AllEmployees[i].locationPos ) && Manager.AllEmployees[i].hasTalked == true )
            {


                this.transform.position =   pathLayer.CellToWorld(new Vector3Int(Manager.AllEmployees[i].locationPos.x +3, Manager.AllEmployees[i].locationPos.y + 3, Manager.AllEmployees[i].locationPos.z));

                if(this.lastEmployeeAttached != null)
                {
                    lastEmployeeAttached.HasBusPass = false;
                    lastEmployeeAttached.distance = lastEmployeeAttached.path.Count;
                    
                }
               

                Manager.AllEmployees[i].HasBusPass = true;
                Manager.AllEmployees[i].distance = 0;
                
                // update UI
                DistanceUI.text = "Distance: "+ Manager.AllEmployees[i].distance;



                lastEmployeeAttached = Manager.AllEmployees[i];

                return;

            }



        }

       

        this.transform.position = pathLayer.CellToWorld(busHolderPosition);
        if (this.lastEmployeeAttached != null)
        {
            lastEmployeeAttached.HasBusPass = false;
            lastEmployeeAttached.distance = lastEmployeeAttached.path.Count;
            if (this.lastEmployeeAttached.hasTalked)
            {
                DistanceUI.text = "Distance: " + lastEmployeeAttached.distance;


            }
        }




        //  canvasGroup.alpha = 1f;
        // canvasGroup.blocksRaycasts = true;
    }



    // is called when mouse is on this object and clicked down
    public void OnPointerDown(PointerEventData eventData)
    {





     //   Debug.Log("OnpointerDown");





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
