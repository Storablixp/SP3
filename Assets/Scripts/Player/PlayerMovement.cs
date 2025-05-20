using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private WorldTile waterTile;
    [SerializeField] private float speed = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private float groundCheckRadius = 1f;
    [SerializeField] private Transform groundCheckTrans;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;
    private bool isInWater;

    [SerializeField] private Transform bodyTrans;
    private ChunkInstance currentChunk;
    private WorldTile currentTile;
    private float dirX;
    private Rigidbody2D rb;
    private WorldGenerator worldGenerator;

    private void Awake()
    {
        worldGenerator = FindAnyObjectByType<WorldGenerator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        if (dirX == 1)
        {
            bodyTrans.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (dirX == -1)
        {
            bodyTrans.rotation = Quaternion.Euler(0, 0, 0);
        }

        isGrounded = Physics2D.CircleCast(groundCheckTrans.position, groundCheckRadius, Vector2.down, groundCheckDistance, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.Space) && isInWater)
        {
            rb.linearVelocityY = 5;
        }

        if (currentChunk == null) return;
        currentTile = TryGetTile();

        if (currentTile != null && currentTile == waterTile)
        {
            isInWater = true;
        }
        else isInWater = false;

    }

    private WorldTile TryGetTile()
    {
        Vector3Int tilePos = currentChunk.Tilemap.WorldToCell(transform.position);

        int pixelX = tilePos.x + worldGenerator.worldSize.x / 2;
        int pixelY = tilePos.y + worldGenerator.worldSize.y / 2;
        Vector2Int lookupKey = new Vector2Int(pixelX, pixelY);

        if (worldGenerator.Tiles.TryGetValue(lookupKey, out TileInstance tileInstance))
        {
            return tileInstance.Tile;
        }

        return null;
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
   
    private void FixedUpdate()
    {
       rb.linearVelocity = new Vector2(dirX * speed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (collision.gameObject.TryGetComponent(out ChunkInstance chunk))
            {
                currentChunk = chunk;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.color = Color.green;
        }
        else Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(groundCheckTrans.position, groundCheckRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundCheckTrans.position, groundCheckTrans.position + Vector3.down * groundCheckDistance);
    }
}
