using System;
using System.Collections.Generic;
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

    private float last_attack = 0f;

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
        last_attack += Time.deltaTime;

        // Proverava da li je trenutna meta validna
        if (target == null || Vector3.Distance(target.transform.position, transform.position) > range)
        {
            target = FindNewTarget();
        }

        if (target == null) return;

        // Napad na metu
        if (last_attack >= 1 / attack_speed)
        {
            if (target.TryGetComponent(out Enemy_Health enemyHealth))
            {
                enemyHealth.Damage(attack);
            }

            last_attack = 0;
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
