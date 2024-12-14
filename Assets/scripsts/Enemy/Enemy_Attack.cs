using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Enemy_Attack : MonoBehaviour
{
    [SerializeField] public float range;
    [SerializeField] public float attack;
    [SerializeField] private float attack_speed;

    private float last_attack = 0f;

    [Header("Debug")]
    [SerializeField] bool debug;
    [SerializeField] private Mesh circle;
    
    private void Update()
    {
        last_attack += Time.deltaTime;
    }

    public void Attack(GameObject target)
    {
        if (last_attack > 1 / attack_speed)
        {
            if (target.TryGetComponent(out Building building))
            {
                building.Damage(attack);
            }

            last_attack = 0;
        }
    }

    private void OnDrawGizmos()
    {
        if (circle == null) return; // Provera da li je mesh postavljen
        if (!debug) return;

        Gizmos.color = Color.red;

        // Iscrtavanje kruga na horizontalnoj ravni
        Gizmos.DrawWireMesh(
            mesh: circle,
            position: new Vector3(transform.position.x, 0.14f, transform.position.z),
            rotation: Quaternion.identity,
            scale: new Vector3(range * 2, 0, range * 2) // Skaliranje samo u ravni x i z
        );
    }
}
