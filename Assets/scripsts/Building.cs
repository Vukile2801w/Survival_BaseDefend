
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Building : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] Health_Bar health_bar;

    // Start is called before the first frame update
    void Start()
    {
        health_bar.Set_Max_Health(health, true);
    }

    // Update is called once per frame
    void Update()
    {
        health_bar.Set_Health(health);

        
    }

    public void Damage(float damage)
    {
        health -= damage;

        if (health < 0)
        {
            gameObject.SetActive(false);
        }

    }

    public float Get_Health()
    {
        return health;
    }
}
