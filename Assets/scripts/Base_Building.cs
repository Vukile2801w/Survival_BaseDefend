using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Base_Building : MonoBehaviour
{

    [SerializeField] Placement_System placement_system;
    public bool placed = false;
    void Update()
    {
        if (placed) return;
        
        placed = true;
        placement_system.placed_GameObject.Add(gameObject);
        placement_system.grid_data.Add_Object_At(placement_system.grid.WorldToCell(transform.parent.position), placement_system.database.objectData[1].Size, 1, placement_system.placed_GameObject.Count - 1);
        Debug.Log("Placed");
        
    }
    
}
