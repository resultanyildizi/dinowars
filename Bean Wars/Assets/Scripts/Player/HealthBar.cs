using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private Player player;

    private void Awake()
    {
        Player.OnPlayerCreatedEvent += SetPlayer;

    }

    private void SetPlayer(Player player)
    {
        this.player = player;
        SetMaxHealth(player);
        player.OnHealthChangedEvent += SetCurrentHealth;
    }

    public void SetMaxHealth(Player player)
    {
        slider.maxValue = player.PlayerMaxHealth;
        slider.value = player.PlayerHealth;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetCurrentHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
