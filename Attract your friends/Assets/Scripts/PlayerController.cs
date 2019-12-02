﻿using UnityEngine;

public class PlayerController : MonoBehaviour{
    public enum PlayerNumber{ one, two, three, four };
    
    public PlayerNumber player = PlayerNumber.one;              // What player number this character is
    public float minJumpHeight = 1f;                            // Minimum height the character reaches when jumping
    public float maxJumpHeight = 10f;                           // Max height the character can reach when jumping
    public float timeToJumpApex = .4f;                          // How long it takes the player to reach max height
    public float moveSpeed = 10f;                               // How fast the player moves
    [Range(0, 0.3f)] public float movementSmoothing = 0.05f;    // How much to smooth the movement
    public LayerMask groundLayers;                              // A mask determining what to treat as the ground
    public Transform groundCheck;                               // A position at which to check if a player is grounded
    public bool airControl;                                     // Whether or not a character can move while in the air

    const float groundedRadius = .05f;          // Radius of the overlap circle to determine if grounded
    private bool isGrounded;                    // Whether or not the player is grounded.
    private Rigidbody2D rb;                     // The rigidbody attached to this object, set in start
    private Vector2 velocity = Vector3.zero;    // Reference vector for smoothdamp
    private float horizontalInput;              // Horizontal movement input from the user 
    private bool jumpInput;                     // Jumping input from the user
    private float minJumpVelocity;              // What is the velocity necessary to reach the set minimum jump height
    private float maxJumpVelocity;              // What is the velocity necessary to reach the set maximum jump height

    // This function is caled only once: The moment this script instance comes into being!
    private void Awake() {
        // Save our rigidbody once so we do not have to keep figuring it out everytime we want it
        rb = GetComponent<Rigidbody2D>();
    }

    // This function is called once per frame.  Use this to get inputs from the user.
    private void Update() {
        horizontalInput = InputManager.HorizontalAxis(player) * Time.fixedDeltaTime;
        jumpInput = InputManager.JumpButton(player);

        // Calculate the jump velocities
        float gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    // This function is called zero, once, or multiple times per frame depending on the framerate of the computer.  Use this for all physics calculations.
    private void FixedUpdate() {
        bool wasGrounded = isGrounded;

        // The player is "grounded" if a circlecast at the groundCheck position hits any layer designated as ground
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundLayers);
        for (int i = 0; i < colliders.Length; ++i) {
            if(colliders[i].gameObject != gameObject) {
                isGrounded = true;
            }
        }

        // Move.  That.  Player!
        Move();
    }

    // Moves the character based on player input
    private void Move() {
        if (isGrounded || airControl) {
            // Move the character by finding the target velocity and then smoothing it out and applying it to the character
            Vector2 targetVelocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);
        }

        // If the player is pressing the jump button, we want to jump!
        if (jumpInput) {
            if (isGrounded) {
                rb.velocity = new Vector2(rb.velocity.x, maxJumpVelocity);
            }
        // This triggers the moment the player releases the jump button
        } else {
            if (rb.velocity.y > minJumpVelocity){
                rb.velocity = new Vector2(rb.velocity.x, minJumpVelocity);
            }
        }
    }
}
