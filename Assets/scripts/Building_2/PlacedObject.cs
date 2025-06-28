using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class PlacedObjectManager : MonoBehaviour
{
    public Dictionary<Vector3Int[], int> placedObjects;
    [SerializeField] private ObjectDataBase objectDataBase;

    void PlaceObjectAt(Vector3 pos, int objID)
    {
        
        Vector2Int obj_size = objectDataBase.GetObjectByID(objID).size;


        if (IsObjectPlacedAt(pos))
        {
            new Log(true, "PlacedObjectMenager ").error($"Object already placed at {pos}. Cannot place another object with ID {objID} here.");

            return;
        }

        Vector3Int[] occupi = new Vector3Int[obj_size.x * obj_size.y];

        for (int i = 0; i < obj_size.x; i++)
        {
            for (int j = 0; j < obj_size.y; j++)
            {
                occupi[i * obj_size.y + j] = new Vector3Int(pos.x + i, pos.y, pos.z + j);
            }
        }


        placedObjects.Add(occupi, objID);

    }

    public bool IsObjectPlacedAt(Vector3 pos, out int objID)
    {

        // Evo vidis


        return placedObjects.TryGetValue(pos, out objID);
    }

    public bool IsObjectPlacedAt(Vector3 pos)
    {
        return IsObjectPlacedAt(pos, out int _);
    }


}