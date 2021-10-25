using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private bool grounded;

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
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        

            //horizantalInput is always 0, if click the right button, it should be 1, and -1

            /* if (horizantalInput > 0.01f)
                transform.localScale = Vector3.one;
            else if (horizantalInput < -0.01f)
                transform.localScale = new Vector3(-1, 1, 1); */


            float horizantalInput = Input.GetAxis("Horizontal");
            body.velocity = new Vector2(horizantalInput * speed, body.velocity.y);



            if (Input.GetKey(KeyCode.Space) && grounded)
                Jump();

    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, 10);
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }
}
