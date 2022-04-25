using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DocumentHandler : MonoBehaviour
{
    public Document thisDocument;
    public Image documentImage;
    [SerializeField]
    private bool isRead;
    // Start is called before the first frame update
    void Start()
    {
        //make document invisible on load
        documentImage.transform.localScale = new Vector3(0,0,0);
    }

    public void viewDocument()
    {
        documentImage.transform.localScale = new Vector3(1,1,1);
        isRead = true;
    }
    public void closeDocument()
    {
        //hide UI
        documentImage.transform.localScale = new Vector3(0,0,0);
        //hide gameobject 
        this.gameObject.transform.localScale = new Vector3(0,0,0);
    }
}
