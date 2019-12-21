using UnityEngine;

public class PlayerController : MonoBehaviour{    
    public int playerNum = 1;                                   // What player number this character represents (1-4)
    public float pullRadius = 5f;                               // How far the Pull action can influence
    public float pullForce = 1f;                                // How strong of an effect the pull action has
    public float pushRadius = 5f;                               // How far the Push action can influence
    public float pushForce = 1f;                                // How strong of an effect the push action has
    public float pushCooldown = 2f;                             // How long this character must wait before pushing again
    public float minJumpHeight = 1f;                            // Minimum height the character reaches when jumping
    public float maxJumpHeight = 10f;                           // Max height the character can reach when jumping
    public float timeToJumpApex = .4f;                          // How long it takes the player to reach max height
    public float moveSpeed = 10f;                               // How fast the player moves
    public float maxVelocity = 100f;                            // How fast the player can possibly move, all outside forces considered
    [Range(0, 0.3f)] public float movementSmoothing = 0.05f;    // How much to smooth the movement
    public LayerMask influenceLayers;                           // A mask determining what players can push and pull
    public LayerMask groundLayers;                              // A mask determining what to treat as the ground
    public bool airControl = true;                              // Whether or not a character can move while in the air
    public float groundedHeight = 0.05f;                        // Height of the overlap box to determine if grounded
    public float groundedWidth = .9f;                           // Percentage of width of player the grounded check is

    // Variables below this line are taken care of in the code.  Do not change them!

    private bool isGrounded;                    // Whether or not the player is grounded
    private bool isOnGroundedPlayer;            // Whether or not the player is on top of another player
    private Rigidbody2D rb;                     // The rigidbody attached to this object, set in Awake()
    private Vector2 velocity = Vector3.zero;    // Reference vector for smoothdamp
    private float horizontalInput;              // Horizontal movement input from the user 
    private bool jumpInput;                     // Jumping input from the user
    private bool pushInput;                     // Push effect input from the user
    private bool pullInput;                     // Pull effect input from the user
    private float minJumpVelocity;              // What is the velocity necessary to reach the set minimum jump height
    private float maxJumpVelocity;              // What is the velocity necessary to reach the set maximum jump height
    private float curPushCooldown;              // The current countdown on pushing again

    // This function is caled only once: The moment this script instance comes into being!
    private void Awake() {
        // Save our rigidbody once so we do not have to keep figuring it out everytime we want it
        rb = GetComponent<Rigidbody2D>();
    }

    // This function is called once per frame.  Use this to get inputs from the user
    private void Update() {
        horizontalInput = InputManager.HorizontalAxis(playerNum) * Time.fixedDeltaTime;
        jumpInput = InputManager.JumpButton(playerNum);
        pullInput = InputManager.PullButton(playerNum);
        pushInput = InputManager.PushButton(playerNum);

        // Calculate the jump velocities (This only lives in update so you can dynamically change these values)
        float gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    // This function is called zero, once, or multiple times per frame depending on the framerate of the computer.  Use this for all physics or time-based calculations
    private void FixedUpdate() {
        // Determine if the player can jump
        isGrounded = CheckIfGrounded();
        isOnGroundedPlayer = CheckIfOnGroundedPlayer();

        // Move.  That.  Player!
        Move();

        // Make sure the player never exceeds its maximum speed
        if (rb.velocity.magnitude > maxVelocity){
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }

        // Does this player want to push or pull anything?
        if (pullInput) { Pull(); }
        if (pushInput) { Push(); }
        curPushCooldown -= Time.deltaTime;
    }

    // Moves the character based on player input
    private void Move() {
        if (isGrounded || airControl && horizontalInput != 0) {
            // Move the character by finding the target velocity and then smoothing it out and applying it to the character
            Vector2 targetVelocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);
        }

        // If the player is pressing the jump button, we want to jump!
        if (jumpInput) {
            if (isGrounded || isOnGroundedPlayer) {
                rb.velocity = new Vector2(rb.velocity.x, maxJumpVelocity);
            }
        // This triggers the moment the player releases the jump button
        } else {
            if (rb.velocity.y > minJumpVelocity){
                rb.velocity = new Vector2(rb.velocity.x, minJumpVelocity);
            }
        }
    }

    // Pulls in other player objects
    private void Pull(){
        Collider2D myCollider = GetComponent<Collider2D>();

        // Check for all objects within a certain radius
        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, pullRadius, influenceLayers);

        // Loop through each of the players found, and add a force to them towards this object
        for (int i = 0; i < players.Length; ++i) {
            Rigidbody2D rigidbody = players[i].gameObject.GetComponent<Rigidbody2D>();
            Vector2 pullDirection = transform.position - players[i].gameObject.transform.position;

            // We only want to add this force if the object is NOT touching us
            if (!myCollider.IsTouching(players[i])){
                rigidbody.AddForce(pullDirection.normalized * pullForce, ForceMode2D.Force);
            } 
        }
    }

    // Pushes other player objects away
    private void Push(){
        // First check to see if the player is even allowed to push right now
        if (curPushCooldown <= 0){
            // If so, reset the timer
            curPushCooldown = pushCooldown;

            // Check for all objects within a certain radius
            Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, pushRadius, influenceLayers);
            
            // Loop through each of the players found, and add an impulse force to them away from this object
            for (int i = 0; i < players.Length; ++i){
                Rigidbody2D rigidbody = players[i].gameObject.GetComponent<Rigidbody2D>();
                Vector2 pushDirection = -(transform.position - players[i].transform.position);

                rigidbody.AddForce(pushDirection.normalized * pushForce, ForceMode2D.Impulse);
            }
        }
    }

    // Check if this player is grounded or not
    private bool CheckIfGrounded() {
        bool grounded = false;

        // Create a box beneath the player, if that overlaps with something we can jump off of, we are grounded!
        Collider2D[] colliders = Physics2D.OverlapBoxAll(GetBottomOfPlayer(), new Vector2(transform.localScale.x * groundedWidth, groundedHeight), 0f, groundLayers);
        for (int i = 0; i < colliders.Length; ++i) {
            if (colliders[i].gameObject != gameObject) {
                grounded = true;
            }
        }

        return grounded;
    }

    // This is a special case.  Let players jump off other players, but ONLY if that player is grounded
    private bool CheckIfOnGroundedPlayer() {
        bool onPlayer = false;
        int layerMask = 1 << LayerMask.NameToLayer("Player");

        // Create a box beneath the player, if that overlaps with another player, we are on another player
        Collider2D[] colliders = Physics2D.OverlapBoxAll(GetBottomOfPlayer(), new Vector2(transform.localScale.x * groundedWidth, groundedHeight), 0f, layerMask);
        for (int i = 0; i < colliders.Length; ++i) {
            if (colliders[i].gameObject != gameObject && colliders[i].gameObject.GetComponent<PlayerController>().IsGrounded()) {
                onPlayer = true;
            }
        }

        return onPlayer;
    }

    // This will find and return the bottom center of this character
    private Vector2 GetBottomOfPlayer() {
        return new Vector2(transform.position.x, transform.position.y - transform.localScale.y / 2);
    }

    ///////////////////// Public Interfaces /////////////////////
    public bool IsGrounded() {
        return isGrounded;
    }
}
