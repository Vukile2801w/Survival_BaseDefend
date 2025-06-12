using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Enemy_Movment : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private Enemy_Attack script_attack;

    [SerializeField] GameObject Target;

    [SerializeField] private NavMeshAgent nav_mesh_agent;

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


    private void Start()
    {
        nav_mesh_agent.stoppingDistance = script_attack.attack_range;
    }


    void Update()
    {
        if (Target == null) Target = Find_new_Target();
        if (Target == null) return;

        nav_mesh_agent.SetDestination(Target.transform.position);


        Transform Target_Transform = Target.transform;

        
        nav_mesh_agent.destination = Target_Transform.position;

        float distance = nav_mesh_agent.remainingDistance;

        if (distance <= script_attack.attack_range)
        {
            script_attack.Attack(Target);
        }

    }
}
