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

    public string PlayerName { get => playerName; set => playerName = value; }
    public Color PlayerColor { get => playerColor; set => playerColor = value; }
    public DinowarsNetworkRoomPlayer.Team Team { get => team; set => team = value; }

    [SyncVar(hook = nameof(OnHealthChanged))]
    private double health;



    // Public ===================================================================
    public event System.Action<double> OnHealthChangedEvent;

    public override void OnStartAuthority()
    {
        CmdChangeHealth(maxHealth);
    }

    public override void OnStartClient()
    {
        var playerCanvas = GetComponentInChildren<PlayerCanvas>();
        playerCanvas.PlayerNameText.text = playerName;
        playerCanvas.PlayerNameText.color = playerColor;
    }


    private void OnHealthChanged(double _, double newHealth) { OnHealthChangedEvent?.Invoke(newHealth); }

    public void Heal(double healingAmount)
    {
        this.health += healingAmount;
        Debug.Log(string.Format("My name is {0} - My new health is {1}", playerName, health));
    }

    [Command]
    private void CmdChangeHealth(double newHealth)
    {
        if (newHealth > maxHealth) this.health = this.maxHealth;
        else this.health = newHealth;
    }



}
