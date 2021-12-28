using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    
    public void SetHealth(double health)
    {
        slider.value = (float)health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxHealth(double maxHealth)
    {
        slider.maxValue = (float)maxHealth;
        slider.value = (float)maxHealth;

        fill.color = gradient.Evaluate(1f);
    }
}
