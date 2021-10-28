using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Player : NetworkBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Text playerNameText;
    public GameObject playerInfo;

    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    [SyncVar(hook = nameof(OnColorChanged))]
    public Color playerColor = Color.white;


    private void OnNameChanged(string oldName, string newName)
    {
        playerNameText.text = playerName;
    }

    private void OnColorChanged(Color oldColor, Color newColor)
    {
        playerNameText.color = playerColor;
        spriteRenderer.color = playerColor;
    }


    public override void OnStartLocalPlayer()
    {
        playerName = GenerateRandomName();
        playerColor = GenerateRandomColor();
    }

    private Color GenerateRandomColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 0.2f);

        return new Color(r, g, b);
    }

    private string GenerateRandomName()
    {
        return "Player " + Random.Range(100, 999);
    }


    private void Awake()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;      

    }

  
}
