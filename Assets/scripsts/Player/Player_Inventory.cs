using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player_Inventory : MonoBehaviour
{
    public enum Item
    {
        Wood
    }

    [SerializeField] private GameObject Wood;
    
    public void Update_Item(Item item,int ammount)
    {
        Debug.Log("Updating");

        if (item == Item.Wood)
        {
            Debug.Log("Drvo++");
            Text txt = Wood.transform.GetChild(1).gameObject.GetComponent<Text>();
            int txt_to_int = 0;
            if(int.TryParse(txt.text, out txt_to_int))
            {
                txt_to_int += ammount;

            }
            else
            {
                txt_to_int = ammount;
            }

            txt.text = txt_to_int.ToString();
        }
    }
}
