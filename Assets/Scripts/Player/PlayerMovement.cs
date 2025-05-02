using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private float groundCheckRadius = 1f;
    [SerializeField] private Transform groundCheckTrans;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    [SerializeField] private Transform bodyTrans;
    private float dirX;
    private Rigidbody2D rb;

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
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
   
    private void FixedUpdate()
    {
       rb.linearVelocity = new Vector2(dirX * speed, rb.linearVelocity.y);
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
