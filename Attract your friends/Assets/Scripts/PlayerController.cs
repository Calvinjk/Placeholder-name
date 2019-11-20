using UnityEngine;

public class PlayerController : MonoBehaviour{
    public float jumpForce = 100f;                              // Amount of force added when the player jumps
    public float moveSpeed = 10f;                               // How fast the player moves
    [Range(0, 0.3f)] public float movementSmoothing = 0.05f;    // How much to smooth the movement
    public LayerMask groundLayers;                              // A mask determining what to treat as the ground
    public Transform groundCheck;                               // A position at which to check if a player is grounded
    public bool airControl;                                     // Whether or not a character can move while in the air

    [Tooltip("Do not modify variables below this line in the inspector")]
    public bool ___________________________;    // Inspector divider between "public" and "private" variables.

    const float groundedRadius = .05f;          // Radius of the overlap circle to determine if grounded
    private bool isGrounded;                    // Whether or not the player is grounded.
    private Rigidbody2D rb;                     // The rigidbody attached to this object, set in start
    private Vector2 velocity = Vector3.zero;    // Reference vector for smoothdamp
    private float horizontalInput;              // Horizontal movement input from the user 
    private bool jumpInput;                     // Jumping input from the user

    // This function is caled only once: The moment this script instance comes into being!
    private void Awake() {
        // Save our rigidbody once so we do not have to keep figuring it out everytime we want it
        rb = GetComponent<Rigidbody2D>();
    }

    // This function is called once per frame.  Use this to get inputs from the user.
    private void Update() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump")) { jumpInput = true; } 
    }

    // This function is called zero, once, or multiple times per frame depending on the framerate of the computer.  Use this for all physics calculations.
    private void FixedUpdate() {
        bool wasGrounded = isGrounded;

        // The player is "grounded" if a circlecast at the groundCheck position hits any layer designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundLayers);
        for (int i = 0; i < colliders.Length; ++i) {
            if(colliders[i].gameObject != gameObject) {
                isGrounded = true;
            }
        }

        // Move.  That.  Player!
        Move(horizontalInput * Time.fixedDeltaTime, jumpInput);
    }

    // Use this function to move your character.
    private void Move(float move, bool jump) {
        if (isGrounded || airControl) {
            // Move the character by finding the target velocity and then smoothing it out and applying it to the character
            Vector2 targetVelocity = new Vector2(move * moveSpeed, rb.velocity.y);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);
        }

        // Jump by simply adding a force to the rigidbody on this object
        if (jump) {
            if (isGrounded) {
                rb.AddForce(new Vector2(0f, jumpForce));
                isGrounded = false;
            }
            jumpInput = false;
        }
    }
}
