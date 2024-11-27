using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Tree : MonoBehaviour
{
    [SerializeField] private float healt = 100f;

    public void Cut(float amount)
    {
        healt -= amount;

        if (healt <= 0)
        {
            healt = 0;
            gameObject.SetActive(false); // Deaktiviraj drvo
            Debug.Log("Tree cut down!");
        }
        else
        {
            Debug.Log($"Tree health: {healt}");
        }
    }
}
