using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Enemy_Movment : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private Enemy_Attack script_attack;

    public GameObject Target;

    private float last_attack = 0;

    void Update()
    {
        if (Target == null) return;

        last_attack += Time.deltaTime;

        Transform Target_Transform = Target.transform;

        Vector3 direction = Target_Transform.position - transform.position;
        float distance = direction.magnitude;

        if (distance > script_attack.range)
        {
            // Kreće se prema meti
            rb.velocity = direction.normalized * speed;
        }
        else
        {
            // Zaustavlja kretanje kada je u dometu napada
            rb.velocity = new Vector3(0, rb.velocity.y, 0);

            script_attack.Attack(Target);
        }

        // Rotira prema meti
        Vector3 lookDirection = new Vector3(direction.x, 0, direction.z); // Ignoriše Y osu
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = targetRotation;
        }
    }
}
