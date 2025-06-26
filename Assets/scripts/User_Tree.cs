using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class User_Tree : MonoBehaviour
{
    [SerializeField] private Animation_Menager animator;
    private bool animated; 

    [SerializeField] private float healt = 100f;
    [SerializeField] private int self_value = 1;

    public bool is_cut = false;
    private bool started = false;

    private void Update()
    {
        Animation_Menager.StateInfo info = animator.IsPlayingAnimation("Tree_Fall", "Tree_Animator");
        //Debug.Log("Tree destroyed checked", this);

        // Animacija je validna, igra se "Tree_Fall" i skoro je gotova
        if (info.nameCorrect && info.isValid && info.normTimeCorrect >= 0.98f)
        {
            Debug.Log($"Is valid {true}", this);


            if (!is_cut)
            {
                animated = true;
                is_cut = true;

                Debug.Log("Tree destroyed");
                Destroy(gameObject);
            }
        }
    }




    private void On_Down(Player_Inventory inventory)
    {
        Collider mainCollider = gameObject.GetComponent<Collider>();
        if (mainCollider != null)
        {
            mainCollider.enabled = false; // Disable main collider to prevent further interactions
        }

        foreach (Collider col in animator.gameObject.GetComponentsInChildren<Collider>())
        {
            if (col == null) continue;
            col.enabled = false; 
        }

        inventory.Add_Item(Materilals.Materials.Wood, self_value);

        animator.Play("Tree_Fall");

    }


    public bool Cut(float amount, Player_Inventory inventory)
    {

        healt -= amount;
        Debug.Log("Tree Hit-ed!");


        if (healt <= 0 && healt != float.NegativeInfinity)
        {
            healt = float.NegativeInfinity;


            On_Down(inventory);



            Debug.Log("Tree cut down!");
            return true;
        }
        else
        {
            Debug.Log($"Tree health: {healt}");
            animator.Play("Hit");
            return false;
        }
    }
}
