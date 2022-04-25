using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MapBoundary : MonoBehaviour, IDropHandler
{


    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("ON DROP!");
    }

}
