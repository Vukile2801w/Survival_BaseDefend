using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Tower : MonoBehaviour
{
    [SerializeField] private float range;

    [SerializeField] public float attack;
    [SerializeField] private float attack_speed;

    [Header("Debug")]
    [SerializeField] private bool debug;
    [SerializeField] private Mesh circle;

    private GameObject target;

    private float nextAttackTime = 0f;


    [SerializeField] private bool commponent_enabled = false;

    [SerializeField] private Building building;

    [Header("Stats")]
    [SerializeField, Tooltip("Damage Per Second (DPS)")]
    private float dps;


    private void OnValidate()
    {
        dps = attack * attack_speed;
    }

    public void Placed()
    {
        Debug.Log("\"Placed\" has been called");

        commponent_enabled = true;

    }

    private void OnDestroy()
    {
        /*
         * Za sad koristicu manuelni mod
         
        building.OnBuildingPlaced -= Placed;
        */
        }


    private void Start()
    {
        /*
         * Za sad koristicu manuelni mod
         
        building.OnBuildingPlaced -= Placed;
        building.OnBuildingPlaced += Placed;
        */

    }


    private GameObject FindNewTarget()
    {
        Collider[] possible_targets = Physics.OverlapSphere(transform.position, range);

        GameObject closestTarget = null;
        float minDistance = float.PositiveInfinity;

        foreach (Collider collider in possible_targets)
        {
            if (!collider.CompareTag("Enemy")) continue;

            float dist = Vector3.Distance(collider.transform.position, transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestTarget = collider.gameObject;
            }
        }

        return closestTarget;
    }

    void Update()
    {
        if (commponent_enabled == false) return;


        // Proverava da li je trenutna meta validna
        if (target == null || Vector3.Distance(target.transform.position, transform.position) > range)
        {
            target = FindNewTarget();
        }

        if (target != null && !target.activeInHierarchy)
        {
            target = null;
        }


        if (target == null) return;

        // Napad na metu

        if (Time.time >= nextAttackTime)
        {
            // Napad na metu
            if (target.TryGetComponent(out Enemy_Health enemyHealth))
            {
                enemyHealth.Damage(attack);
            }

            nextAttackTime = Time.time + 1 / attack_speed;
        }

    }

    private void OnDrawGizmos()
    {
        if (circle == null || !debug) return;

        Gizmos.color = Color.red;

        // Iscrtavanje kruga na horizontalnoj ravni
        Gizmos.DrawWireMesh(
            circle,
            transform.position + Vector3.down * 1.9f, // Blago podizanje kruga iznad tla
            Quaternion.identity,
            new Vector3(range * 2, 0.1f, range * 2) // Skaliranje sa malom visinom
        );
    }
}
