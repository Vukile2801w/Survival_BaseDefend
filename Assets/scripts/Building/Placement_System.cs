using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Placement_System : MonoBehaviour
{

    
    [SerializeField] private Build_Input_Manager Input_Manager;

    [SerializeField] public Grid grid;

    [SerializeField] public Objects_DataBase_SO database;
    [SerializeField] private ShopMenagment shopMenagment;
    public int selected_Object_index { get; private set; } = -1;
    public int Shop_ID  = -1;


    [Header("UI")]
    [SerializeField] private GameObject grid_visualisation;
    [SerializeField] private GameObject Build_Menu;

    [SerializeField] private Transform Parent_for_new_building;

    [SerializeField] private Preview_System preview;
    

    public Grid_Data grid_data;

    public List<GameObject> placed_GameObject = new();

    private Vector3Int last_detected_pos = Vector3Int.zero;


    private void Awake()
    {
        Stop_Placement();
        grid_data = new Grid_Data();
        
    }

    public int Get_Selected_Object_Index(GameObject prefab)
    {
        for (int i = 0; i < database.objectData.Count; i++)
        {
            int ID = database.objectData[i].ID;
            GameObject gameObject = database.objectData[i].prefab;

            if (prefab == gameObject)
            {
                return ID;
            }
        }

        return -1;
    }

    public void Place_Object(Vector3 grid_pos, int obj_index)
    {
        GameObject new_Object = Instantiate(database.objectData[obj_index].prefab);


        Vector3Int grid_pos_int = grid.WorldToCell(grid_pos);
        Vector3 offset = grid_pos - grid_pos_int;

        new_Object.transform.position = grid.CellToWorld(grid_pos_int) + offset;
        new_Object.transform.position += new Vector3(0, 0.14f, 0);
        new_Object.transform.parent = Parent_for_new_building;

        Building new_Object_Building = new_Object.GetComponentInChildren<Building>() ?? new_Object.GetComponent<Building>();

        if (new_Object_Building != null)
        {
            Debug.LogAssertion($"{new_Object.name} have Building component: {new_Object_Building != null}");
            new_Object_Building.PlaceBuilding();
        }

        placed_GameObject.Add(new_Object);

        grid_data.Add_Object_At(
            grid_pos_int,
            database.objectData[selected_Object_index].Size,
            database.objectData[selected_Object_index].ID,
            Mathf.Max(placed_GameObject.Count - 1, 0)
            );

        

    }


    public void Start_Placement(int ID)
    {
        Stop_Placement();

        selected_Object_index = database.objectData.FindIndex(data => data.ID == ID);



        if (selected_Object_index < 0)
        {
            Debug.LogError($"No ID found: {ID}");
            return;
        }

        grid_visualisation.SetActive(true);
        preview.Start_Preview(
            database.objectData[selected_Object_index].prefab,
            database.objectData[selected_Object_index].Size
            );
        Input_Manager.onClicked += Place_Structure;
        Input_Manager.onExit += Stop_Placement;


        
        Build_Menu.SetActive(false);
        

    }

    private void Place_Structure()
    {

        if (Input_Manager.Is_Pointer_Over_UI())
        {
            return;
        }

        Vector3 mouse_position = Input_Manager.Get_Selected_Map_Position();
        Vector3Int grid_pos = grid.WorldToCell(mouse_position);

        bool placement_is_valid = Check_Placement_Validity(grid_pos, selected_Object_index);

        if (placement_is_valid == false)
            return;
        GameObject new_Object = Instantiate(database.objectData[selected_Object_index].prefab);

        new_Object.transform.position = grid.CellToWorld(grid_pos);
        new_Object.transform.position += new Vector3(0, 0.14f, 0);
        new_Object.transform.parent = Parent_for_new_building;

        Building new_Object_Building = new_Object.GetComponentInChildren<Building>() ?? new_Object.GetComponent<Building>();

        Debug.LogAssertion($"{new_Object.name} have Building component: {new_Object_Building != null}");
        if (new_Object_Building != null)
        {
            new_Object_Building.PlaceBuilding();
        }

        placed_GameObject.Add(new_Object);

        grid_data.Add_Object_At(
            grid_pos,
            database.objectData[selected_Object_index].Size,
            database.objectData[selected_Object_index].ID,
            placed_GameObject.Count - 1
            );

        preview.UpdatePosition(grid.CellToWorld(grid_pos), false);

        shopMenagment.Remove_Mat(Shop_ID);

        if (!shopMenagment.CanIBuildIt(shopMenagment.ShopItems[Shop_ID]))
        {
            Stop_Placement();
        }
    }

    private bool Check_Placement_Validity(Vector3Int grid_pos, int selected_Object_index)
    {
        return grid_data.Can_Place_Obj_At(grid_pos, database.objectData[selected_Object_index].Size);
    }

    private void Stop_Placement()
    {


        selected_Object_index = -1;
        Shop_ID = -1;

        grid_visualisation.SetActive(false);
        preview.Stop_Preview();

        Input_Manager.onClicked -= Place_Structure;
        Input_Manager.onExit -= Stop_Placement;

        last_detected_pos = Vector3Int.zero;

    }


    void Update()
    {
        if (selected_Object_index == -1) return;

        Vector3 mouse_position = Input_Manager.Get_Selected_Map_Position();
        Vector3Int grid_pos = grid.WorldToCell(mouse_position);

        if (last_detected_pos != grid_pos)
        { 
            bool placement_is_valid = Check_Placement_Validity(grid_pos, selected_Object_index);

            preview.UpdatePosition(grid.CellToWorld(grid_pos), placement_is_valid);
        }

    }
}
