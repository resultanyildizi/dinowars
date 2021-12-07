using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Player : NetworkBehaviour
{
    // Static ===================================================================
    public static event System.Action<Player> OnPlayerCreatedEvent;

    // Public ===================================================================
    public event System.Action<string> OnNameChangedEvent;
    public event System.Action<Color> OnColorChangedEvent;
    public event System.Action<float> OnHealthChangedEvent;

    public string PlayerName { get => playerName; set => playerName = value; }
    public Color PlayerColor { get => playerColor; set => playerColor = value; }
    public float PlayerHealth { get => health; }

    public readonly float PlayerMaxHealth = 1200;

    //Deneme için yazdýðým koddur. Dokunmayýn arada açmam gerekiyor bu kodu.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (health > 0)
            {
                health -= 100;
            }
            
        }
    }

    public override void OnStartClient()
    {
        // Fill player's data
        health = PlayerMaxHealth;
        PlayerName = Utils.GenerateRandomName("Resul");
        PlayerColor = Utils.GenerateRandomColor();

        // Tell all listeners the player is created
        OnPlayerCreatedEvent?.Invoke(this);
    }

    // Private ===================================================================
    [SyncVar(hook = nameof(OnHealthChanged))]
    private float health;
    [SyncVar(hook = nameof(OnNameChanged))]
    private string playerName;
    [SyncVar(hook = nameof(OnColorChanged))]
    private Color playerColor = Color.white;

    private void OnNameChanged(string _, string newName)     { OnNameChangedEvent?.Invoke(newName);     }
    private void OnColorChanged(Color _, Color newColor)     { OnColorChangedEvent?.Invoke(newColor);   }
    private void OnHealthChanged(float _, float newHealth) { OnHealthChangedEvent?.Invoke(newHealth); }

    public void Heal(float healingAmount)
    {
        this.health += healingAmount;

        if (this.health >= PlayerMaxHealth)
        {
            this.health = PlayerMaxHealth;
        }
        
        Debug.Log(string.Format("My name is {0} - My new health is {1}", playerName, health));
    }


}
