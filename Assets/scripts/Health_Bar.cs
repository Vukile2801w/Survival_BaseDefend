using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Health_Bar : MonoBehaviour
{
    [SerializeField] private Slider Health_Slider; // Slider representing the health bar.
    [SerializeField] private Gradient Health_Gradient; // Gradient for health bar colors.
    [SerializeField] private Image Health_Bar_Fill_Image; // Image representing the fill of the health bar.
    [SerializeField] private GameObject Helth_Bar_Canvas;
    

    /// <summary>
    /// Sets the maximum health value for the slider and optionally updates the current health.
    /// </summary>
    /// <param name="Max_Health">Maximum health value.</param>
    /// <param name="Update_Health">If true, updates the current health to the maximum value.</param>
    public void Set_Max_Health(float Max_Health, bool Update_Health = false)
    {
        Health_Slider.maxValue = Max_Health;

        if (Update_Health)
        {
            Set_Health(Max_Health);
        }
    }

    /// <summary>
    /// Sets the current health value on the slider and updates the fill color.
    /// </summary>
    /// <param name="Current_Health">Current health value.</param>
    public void Set_Health(float Current_Health)
    {
        // Clamp health to avoid out-of-range values.
        Current_Health = Mathf.Clamp(Current_Health, 0, Health_Slider.maxValue);

        Health_Slider.value = Current_Health;

        // Evaluate the gradient color based on the current health percentage.
        Color New_Color = Health_Gradient.Evaluate(Health_Slider.value / Health_Slider.maxValue);
        Health_Bar_Fill_Image.color = New_Color;
    
        if (Health_Slider.value == Health_Slider.maxValue)
        {
            Helth_Bar_Canvas.SetActive(false);
        }
        else
        {
            Helth_Bar_Canvas.SetActive(true);
        }
    }
}
