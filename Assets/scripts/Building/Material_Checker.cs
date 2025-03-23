using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Material_Checker : MonoBehaviour
{
    [SerializeField] Objects_DataBase_SO database;
    [SerializeField] Player_Inventory inventory;

    bool Can_Build(int obj_db_index)
    {
        //List<Materilals.Materials> mats_needed = database.objectData[obj_db_index].material_cost;
        //List<int> ammount = database.objectData[obj_db_index].ammount_material;

        //for (int i = 0; i < mats_needed.Count; i++)
        //{
        //    if (inventory.Have(mats_needed[i], ammount[i]))
        //    {
        //        continue;
        //        
        //    }
        //    else
        //   {
        //        return false;
        //    }
        //}

        return true;
    }
}
