using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Enemy_Movment : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private Enemy_Attack script_attack;

    [SerializeField] GameObject Target;

    GameObject Find_new_Target()
    {
        Collider[] possible_targets = Physics.OverlapSphere(transform.position, script_attack.sight_range);

        // Filtriranje samo objekata sa tagom "Building"
        List<Collider> filteredTargets = new List<Collider>();
        foreach (Collider collider in possible_targets)
        {
            if (collider.CompareTag("Building"))
            {
                filteredTargets.Add(collider);
            }
        }

        // Ako nema meta, vraća null
        if (filteredTargets.Count == 0)
        {
            return null;
        }

        // Pronalaženje najbliže mete
        Tuple<float, Collider> min_d = new Tuple<float, Collider>(float.PositiveInfinity, null);

        foreach (Collider target in filteredTargets)
        {
            float dist = Vector3.Distance(target.transform.position, transform.position);
            if (dist < min_d.Item1)
            {
                min_d = new Tuple<float, Collider>(dist, target);
            }
        }

        // Vraćanje gameObject-a pronađene mete
        return min_d.Item2?.gameObject;
    }



    void Update()
    {
        if (Target == null) Target = Find_new_Target();
        if (Target == null) return;
        

        Transform Target_Transform = Target.transform;

        Vector3 direction = Target_Transform.position - transform.position;
        float distance = direction.magnitude;

        if (distance > script_attack.attack_range)
        {
            // Kreće se prema meti
            rb.velocity = direction.normalized * speed;
        }
        else
        {
            // Zaustavlja kretanje kada je u dometu napada
            rb.velocity = new Vector3(0, rb.velocity.y, 0);

            if (script_attack.Attack(Target))
            {
                Target = null;
            }
            
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
