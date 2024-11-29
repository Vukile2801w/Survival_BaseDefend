using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class User_Tree : MonoBehaviour
{
    [SerializeField] private Animate user_animation;
    private bool animated; 

    [SerializeField] private float healt = 100f;
    [SerializeField] private int self_value = 1;

    private bool is_cut = false;

    private void Start()
    {
        if (user_animation != null)
        {
            animated = true;
        }
        else
        {
            animated = false;
        }
    }

    private void Handle_Playing_Dying_Anim()
    {
        if (animated)
        {
            user_animation.Play();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


    private void On_Down(Player_Inventory inventory)
    {
        is_cut = true;

        inventory.Update_Item(Player_Inventory.Item.Wood, self_value);

        Handle_Playing_Dying_Anim();

    }

    public bool Cut(float amount, Player_Inventory inventory)
    {


        healt -= amount;

        if (healt <= 0)
        {
            healt = 0;


            if (is_cut == false)
            {
                On_Down(inventory);
            }


            
            Debug.Log("Tree cut down!");
            return true;
        }
        else
        {
            Debug.Log($"Tree health: {healt}");
            return false;
        }
    }
}
