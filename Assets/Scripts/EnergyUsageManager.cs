using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUsageManager : MonoBehaviour
{
    public Canvas EnergyUI;
    public Camera energyCam;
    // Start is called before the first frame update
    void Start()
    {
        //disable on start
        EnergyUI.enabled = false;

        energyCam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
