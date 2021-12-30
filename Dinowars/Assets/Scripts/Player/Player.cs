using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Collections;

public class Player : NetworkBehaviour
{
    [SerializeField] private double maxHealth;
    [SerializeField] private double baseDamage;
    [SerializeField] private DinowarsNetworkRoomPlayer.Dino dino;
    [SerializeField] private HealthBar hbar;
    [SerializeField] private PlayerCanvas pCanv;
    [SerializeField] private Animator animator;

    [SyncVar]
    private DinowarsNetworkRoomPlayer.Team team;
    [SyncVar]
    private Color playerColor;
    [SyncVar]
    private string playerName;
    [SyncVar(hook = nameof(OnHealthChanged))]
    private double health;
    
    public string PlayerName { get => playerName; set => playerName = value; }
    public Color PlayerColor { get => playerColor; set => playerColor = value; }
    public DinowarsNetworkRoomPlayer.Team Team { get => team; set => team = value; }
    public double Health { get => health; }

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
        hbar.SetHealth(newHealth);
    }

    public void Heal(double healingAmount) {
        CmdChangeHealth(health + healingAmount);
    }

    public void TakeDamage(double damage) {
        Debug.Log("Take damage " + health);
        CmdChangeHealth(health - damage);
    }

    [ClientRpc]
    public void CRpcDie()
    {
        animator.SetTrigger("Death");

        var pbody = transform.Find("PlayerBody");
        var hand = transform.Find("Hand");
        var head = pbody.Find("Head");
        var body = pbody.Find("Body");


        GetComponent<PlayerDirectionController>().enabled = false;
        GetComponent<PlayerMovementController>().enabled = false;
        GetComponent<NetworkAnimator>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);

        hand.gameObject.GetComponentInChildren<Weapon>().enabled = false;
        head.gameObject.SetActive(false);
        body.gameObject.SetActive(false);
        hand.gameObject.SetActive(false);
    }


    [Command(requiresAuthority = false)]
    public void CmdChangeHealth(double newHealth)
    {
        if (newHealth > maxHealth)
        {
            health = maxHealth;
        } else if(newHealth <= 0) {
            health = 0;
            CRpcDie();
        }
        else { 
            health = newHealth;
        }
    }
}
