using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField] private double maxHealth;
    [SerializeField] private double baseDamage;
    [SerializeField] private DinowarsNetworkRoomPlayer.Dino dino;
    [SerializeField] private HealthBar hbar;
    [SerializeField] private PlayerCanvas pCanv;

    [SyncVar]
    private DinowarsNetworkRoomPlayer.Team team;
    [SyncVar]
    private Color playerColor;
    [SyncVar]
    private string playerName;
    [SyncVar(hook = nameof(OnHealthChanged))]
    public double health;
    
    
    public string PlayerName { get => playerName; set => playerName = value; }
    public Color PlayerColor { get => playerColor; set => playerColor = value; }
    public DinowarsNetworkRoomPlayer.Team Team { get => team; set => team = value; }

    public override void OnStartAuthority()
    {
        CmdChangeHealth(maxHealth);
    }

    public override void OnStartClient()
    {

        pCanv.PlayerNameText.text = playerName;
        pCanv.PlayerNameText.color = playerColor;
        health = maxHealth;
        hbar.SetHealth(health);
        hbar.SetMaxHealth(health);
    }


    public void OnHealthChanged(double oldHealth, double newHealth) {
        hbar.SetHealth(this.health);
    }

    public void Heal(double healingAmount)
    {
        this.health += healingAmount;
        Debug.Log(string.Format("My name is {0} - My new health is {1}", playerName, health));
        CmdChangeHealth(this.health);
    }

    public void TakeDamage(double damage) {
        this.health -= damage;
        Debug.Log(string.Format("My name is {0} - My new health is {1}", playerName, health));
        CmdChangeHealth(this.health);
    }

    [Command]
    public void CmdChangeHealth(double newHealth)
    {
        if (newHealth > maxHealth)
        {
            this.health = newHealth;
        } else if(newHealth < 0) {
            this.health = 0;
        }
        else { 
            this.health = newHealth;
        }
    }



}
