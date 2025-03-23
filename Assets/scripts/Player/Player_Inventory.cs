using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Materilals;

public class Player_Inventory : MonoBehaviour
{
    [SerializeField] private GameObject[] ui_items;

    Dictionary<Materials, int> inventory = new Dictionary<Materials, int>();

    private void Start()
    {
        // Inicijalizacija inventara
        foreach (Materials material in Enum.GetValues(typeof(Materials)))
        {
            inventory.Add(material, 0);
        }

        Update_UI();
    }

    public void Add_Item(Materials item, int amount)
    {
        Debug.Log("Updating inventory");

        if (inventory.ContainsKey(item))
        {
            inventory[item] += amount;
        }
        else
        {
            Debug.LogWarning($"Item {item} not found in inventory.");
        }

        Update_UI();
    }

    public bool Have(Materials mat, int amount)
    {
        return inventory.ContainsKey(mat) && inventory[mat] >= amount;
    }

    private void Update_UI()
    {
        for (int i = 0; i < ui_items.Length; i++)
        {
            if (i >= inventory.Count)
                break;

            GameObject game_object = ui_items[i];
            Text txt = game_object.GetComponent<Text>();

            if (txt != null)
            {
                Materials material = inventory.ElementAt(i).Key;
                txt.text = $"{material}: {inventory[material]}";
            }
            else
            {
                Debug.LogError($"UI element at index {i} does not have a Text component.");
            }
        }
    }
}
