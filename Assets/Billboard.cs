using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform Target_Camera;

    void LateUpdate()
    {
        // Calculate the direction to the camera and invert it
        Vector3 Direction = transform.position - Target_Camera.position;

        // Optional: Ignore vertical rotation if needed
        Direction.y = 0;

        // Apply the corrected rotation
        transform.rotation = Quaternion.LookRotation(Direction);
    }
}
