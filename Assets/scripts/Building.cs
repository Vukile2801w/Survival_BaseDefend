using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Building : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float max_health;

    [SerializeField] private Health_Bar health_bar;

    public event Action OnBuildingPlaced, OnDestroyed;
    [SerializeField] private Tower Tower;

    private void Start()
    {
        if (health_bar == null) return;

        // Postavlja maksimalno zdravlje na health_bar
        health_bar.Set_Max_Health(health, true);
    }

    private void Update()
    {
        if (health_bar == null) return;
        // Ažurira prikaz zdravlja
        health_bar.Set_Health(health);
    }


    public void Damage(float damage)
    {
        // Smanjuje zdravlje
        health -= damage;

        // Ako zdravlje padne na nulu ili manje, deaktivira ceo objekat
        if (health <= 0)
        {
            health = 0;
            OnDestroyed?.Invoke();
            gameObject.SetActive(false);
        }
        
    }

    public float Get_Health()
    {
        return health;
    }

    public void PlaceBuilding()
    {
        Debug.Log("Building has been placed");

        /*
        */
        if (Tower != null)
        {
            Tower.Placed();
        }

        
        OnBuildingPlaced?.Invoke();


    }


}
