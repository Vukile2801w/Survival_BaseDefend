using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Debuging : MonoBehaviour
{
    
    public void Log(string message)
    {
        Debug.Log(message);
    }
}
