using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField] private double maxHealth;
    [SerializeField] private double baseDamage;
    [SerializeField] private DinowarsNetworkRoomPlayer.Dino dino;

    [SyncVar]
    private DinowarsNetworkRoomPlayer.Team team;
    [SyncVar]
    private Color playerColor;
    [SyncVar]
    private string playerName;
    HealthBar hBar;
    public string PlayerName { get => playerName; set => playerName = value; }
    public Color PlayerColor { get => playerColor; set => playerColor = value; }
    public DinowarsNetworkRoomPlayer.Team Team { get => team; set => team = value; }

    [SyncVar(hook = nameof(OnHealthChanged))]
    public double health;



    // Public ===================================================================

   

    public override void OnStartAuthority()
    {
        CmdChangeHealth(maxHealth);
       
    }

    public override void OnStartClient()
    {
        var playerCanvas = GetComponentInChildren<PlayerCanvas>();
        playerCanvas.PlayerNameText.text = playerName;
        playerCanvas.PlayerNameText.color = playerColor;
        hBar = this.transform.GetComponentInChildren<HealthBar>();
        health = maxHealth;
        hBar.SetHealth(health);
        hBar.SetMaxHealth(health);
    }


    public void OnHealthChanged(double oldHealth, double newHealth) {
        takeDamage(this.health);
    }
    public void takeDamage(double newHealth)
    {

        if (hBar != null)
            hBar.SetHealth(this.health);
        else Debug.Log("eeehe");
        CmdChangeHealth(newHealth);

    }
    public void Heal(double healingAmount)
    {
        this.health += healingAmount;
        Debug.Log(string.Format("My name is {0} - My new health is {1}", playerName, health));
        CmdChangeHealth(this.health);
        
    }

    [Command]
    public void CmdChangeHealth(double newHealth)
    {
        if (newHealth > maxHealth)
        {
            this.health = newHealth;

        }
        else { this.health = newHealth;
           
        }
    }



}
