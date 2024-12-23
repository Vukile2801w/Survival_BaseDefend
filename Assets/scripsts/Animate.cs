using System;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]

public class Animate : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void Play()
    {
        animator.Play("Tree_Fall");
        //animator.("Play", true);
        Debug.Log("Animacija pokrenuta");
    }

    public void On_Animation_End()
    {
        Debug.Log("Gotovo");
        Destroy(gameObject);
    }
}
