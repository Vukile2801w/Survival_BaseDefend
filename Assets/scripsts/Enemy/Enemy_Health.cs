using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Enemy_Health : MonoBehaviour
{

    [SerializeField] float health;
    [SerializeField] Health_Bar health_Bar;

    private void Start()
    {
        health_Bar.Set_Max_Health(health, true);
    }

    public void Damage(float damage)
    {
        health -= damage;
        health_Bar.Set_Health(health);

        if (health < 0)
        {
            On_Death();
        }
    }

    private void On_Death()
    {
        Destroy(gameObject);
    }
    
    void Update()
    {
        
    }
}
