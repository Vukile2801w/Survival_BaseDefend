using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grid_Data
{
    Dictionary<Vector3Int, Placement_Data> placed_objects = new();

    public void Add_Object_At(Vector3Int grid_pos, Vector2Int obj_size, int id, int obj_index)
    {
        List<Vector3Int> pos_to_occupy = Calculate_Positions(grid_pos, obj_size);

        Placement_Data placement_Data = new Placement_Data(pos_to_occupy, id, obj_index);

        foreach (var pos in pos_to_occupy)
        {
            if (placed_objects.ContainsKey(pos))
            {
                throw new Exception($"Dict alredy contains this cell pos {pos}");
            }
            else
            {
                placed_objects[pos] = placement_Data;
            }
        }
    }

    private List<Vector3Int> Calculate_Positions(Vector3Int grid_pos, Vector2Int obj_size)
    {
        List<Vector3Int> return_val = new();

        for (int x = 0; x < obj_size.x; x++)
        {
            for (int y = 0; y < obj_size.y; y++)
            {
                return_val.Add(grid_pos + new Vector3Int(x, 0, y));
            }
        }            

        return return_val;
    }

    public bool Can_Place_Obj_At(Vector3Int grid_pos, Vector2Int obj_size)
    {
        List<Vector3Int> pos_to_occupy = Calculate_Positions(grid_pos, obj_size);

        foreach (var pos in pos_to_occupy)
        {
            if (placed_objects.ContainsKey(pos))
                return false;
        }

        return true;
    }
}


public class Placement_Data
{
    public List<Vector3Int> occupied_pos;

    public int ID { get; private set; }

    public int Placed_Object_Idex { get; private set; }


    public Placement_Data(List<Vector3Int> occupied_pos, int id, int placed_object_idex)
    {
        this.occupied_pos = occupied_pos;
        this.ID = id;
        this.Placed_Object_Idex = placed_object_idex;
    }
}