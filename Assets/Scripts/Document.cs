using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Document", menuName = "Document")]
public class Document : ScriptableObject
{
    public string documentName;
    public int energyUsage;
    public enum docType
    {
        Electricity,
        Gas
    }
    public docType documentType;
}
