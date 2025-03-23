using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class ShopMenagment : MonoBehaviour
{
    [SerializeField] private ShopItem[] shopItems;
    public ShopItem[] ShopItems => shopItems; // Read-only pristup iz koda


    [SerializeField] Placement_System Placement_System;

    [SerializeField] private Player_Inventory playerInventory;

    [SerializeField] private Color green_txt;
    [SerializeField] private Color red_txt;

    private bool ammount_updated = false;

    public void Place(int index)
    {
        if (CanIBuildIt(shopItems[index]))
        {
            Placement_System.Start_Placement(shopItems[index].ID);
            Placement_System.Shop_ID = index;
        }
    }

    public void Remove_Mat(int index)
    {
        if (index == -1) return;
        playerInventory.Add_Item(shopItems[index].Materials[0], -shopItems[index].MaterialsAmmount[0]);
    }

    public void Refund(int index)
    {
        if (index == -1) return;
        playerInventory.Add_Item(shopItems[index].Materials[0], shopItems[index].MaterialsAmmount[0]);
    }

    public bool CanIBuildIt(ShopItem shopItem)
    {

        int min_count = Math.Min(shopItem.Materials.Count, shopItem.MaterialsAmmount.Count);

        for (int mat_index = 0; mat_index < min_count; mat_index++)
        {

            int mat_count = shopItem.MaterialsAmmount[mat_index];

            if (!playerInventory.Have(shopItem.Materials[mat_index], mat_count))
            {
                return false;
            }
            
        }

        return true;
    }

    private void Update_Ammount()
    {
        if (ammount_updated)
            return;

        for (int i = 0; i < shopItems.Length; i++)
        {
            shopItems[i].UI_Txt_Ammount.text = shopItems[i].MaterialsAmmount[0].ToString();
        }

        ammount_updated = true;
    }

    private void Update()
    {
        Update_Ammount();

        for (int i = 0; i < shopItems.Length; i++)
        {
            if (CanIBuildIt(shopItems[i]))
            {
                shopItems[i].UI_Txt_Ammount.color = green_txt;
                shopItems[i].UI_Can_u_Build_it.SetActive(false);
            }
            else
            {
                shopItems[i].UI_Txt_Ammount.color = red_txt;
                shopItems[i].UI_Can_u_Build_it.SetActive(true);
            }
            
        }
    }
}


