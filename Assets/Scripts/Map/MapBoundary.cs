using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Fahreen Bushra: not implemented at this time

public class MapBoundary : MonoBehaviour, IDropHandler
{


    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("ON DROP!");
    }

}
