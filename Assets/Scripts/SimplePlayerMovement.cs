using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour {

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    // layers
    public LayerMask groundLayer;
    public LayerMask blobLayer;

    // horizontal movement
    private Vector2 linearVelocity;
    public float moveSpeed = 6f;
    private float move;

    // vertical movement (jump)
    public float jumpHeight = 3f;
    public float jumpForce;
    public bool jumpPressed;
    public bool jumpReleased;
    public float jumpCutMultiplier = 0.5f;
    public float fallMultiplier = 3f;

    // ground and wall checks
    public bool isGrounded;

    public Transform groundCheck;
    public float groundCheckRadius = 0.25f;

    public Transform wallCheck;
    public float wallCheckDistance = 0.5f;
    private bool touchingWall;


    // Awake is called first regardless of if the script is enabled, before Start
    void Start() {
        rb = GetComponent<Rigidbody2D>();

        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        jumpForce = Mathf.Sqrt(2 * gravity * jumpHeight); // v = sqrt(2gh)
    }

    void Update() {
        move = Input.GetAxisRaw("Horizontal");
        // (point, radius, layer)
        isGrounded = (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) 
                        || Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, blobLayer));

        print("Is grounded = " + isGrounded);

        // (point, vector, distance, layer)
        touchingWall = (Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, groundLayer)
                        || Physics2D.Raycast(wallCheck.position, Vector2.left, wallCheckDistance, groundLayer));

        print("Touching wall = " + touchingWall);

        jumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W);

        jumpReleased = Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W);

        if (jumpPressed && rb.linearVelocity.y <= 0f && isGrounded) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // jumpCutMultiplier is < 0, so decreases the linearVelocity.y if button is taopped short
        if (jumpReleased && rb.linearVelocity.y > 0f) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }

        // makes player fall faster than its jump
        if (rb.linearVelocity.y < 0f) {
            rb.gravityScale = fallMultiplier;
        } else {
            rb.gravityScale = 2f;
        }
    }

    void FixedUpdate() {

        // set velocity
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        // prevent sticking to walls
        if (!isGrounded && touchingWall) { // if not grounded and is touching a wall, vel = 0
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
    }
}