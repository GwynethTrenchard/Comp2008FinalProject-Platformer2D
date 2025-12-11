using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int coins;

    // --- Movement & Animation ---
    private Animator animator;             // Reference to Animator for controlling animations
    public float moveSpeed = 4f;           // How fast the player moves left/right

    // --- Jump variables ---
    public float jumpForce = 8f;           // Base jump force (vertical speed)
    public int extraJumpsValue = 1;        // How many extra jumps allowed (1 = double jump, 2 = triple jump)
    private int extraJumps;                // Counter for jumps left

    public float coyoteTime = 0.2f;          // Time after leaving ground when jump is still allowed
    private float coyoteTimeCounter;         // Counter for coyote time

    public Transform groundCheck;          // Empty child object placed at the player's feet
    public float groundCheckRadius = 0.2f; // Size of the circle used to detect ground
    public LayerMask groundLayer;          // Which layer counts as "ground" (set in Inspector)

    // --- Internal state ---
    private Rigidbody2D rb;                // Reference to the Rigidbody2D component
    private bool isGrounded;               // True if player is standing on ground

    public float jumpBufferTime = 0.15f;        // Time before landing when jump input is buffered
    private float jumpBufferCounter;          // Counter for jump buffer

    void Start()
    {
        // Grab references once at the start
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // --- Horizontal movement ---
        // Get input from keyboard (A/D or Left/Right arrows).
        float moveInput = Input.GetAxis("Horizontal");

        // Apply horizontal speed while keeping the current vertical velocity.
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // --- Ground check ---
        // Create an invisible circle at the GroundCheck position.
        // If this circle overlaps any collider on the "Ground" layer, player is grounded.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Reset extra jumps when grounded
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // Reset coyote time counter
            extraJumps = extraJumpsValue;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // Decrease coyote time counter when not grounded
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime; // Reset jump buffer counter when jump is pressed
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime; // Decrease jump buffer counter
        }

        // --- Jump & Double Jump ---
        // If Space is pressed:
        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f )
            {
                // Normal jump
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                SoundManager.Instance.PlaySFX("JUMP");
                coyoteTimeCounter = 0f; // Prevent double jump from coyote time
                jumpBufferCounter = 0f; // Reset jump buffer counter
            }

            else if (extraJumps > 0)
            {
                // Extra jump (double or triple depending on extraJumpsValue)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumps--; // Reduce available extra jumps
                SoundManager.Instance.PlaySFX("JUMP");
                jumpBufferCounter = 0f; // Reset jump buffer counter
            }
        }

        // --- Animations ---
        SetAnimation(moveInput);
    }

    private void SetAnimation(float moveInput)
    {
        // Handle animations based on grounded state and movement
        if (isGrounded)
        {
            if (moveInput == 0)
            {
                animator.Play("Player_Idle"); // Idle animation when not moving
            }
            else
            {
                animator.Play("Player_Run");  // Run animation when moving
            }
        }
        else
        {
            if (rb.linearVelocityY > 0)
            {
                animator.Play("Player_Jump"); // Jump animation when moving upward
            }
            else
            {
                animator.Play("Player_Fall"); // Fall animation when moving downward
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BouncePad"))
        {
            //Apply a stronger velocity force when hitting the bounce pad
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 2f);
            //Play Squash sound effect
            SoundManager.Instance.PlaySFX("SQUASH");
        }

        if(collision.gameObject.tag == "StrawBerry")
        {
            extraJumps = 2;
            Destroy(collision.gameObject);
        }
    }
}



//using UnityEngine;

//public class PlayerController : MonoBehaviour
//{
//    public int coins;
//    // Public variables appear in the Inspector, so you can tweak them without editing code.
//    public float moveSpeed = 4f;       // How fast the player moves left/right

//    //Jump realated variables for the Jump Feature (later)
//    public float jumpForce = 4f;      // How strong the jump is (vertical speed)
//    public Transform groundCheck;      // Empty child object placed at the player's feet
//    public float groundCheckRadius = 0.2f; // Size of the circle used to detect ground
//    public LayerMask groundLayer;      // Which layer counts as "ground" (set in Inspector)

//    // Private variables are used internally by the script.
//    private Rigidbody2D rb;            // Reference to the Rigidbody2D component
//    private bool isGrounded;           // True if player is standing on ground

//    private Animator animator;        // Reference to the Animator component (for future use)
//    void Start()
//    {
//        // Grab the Rigidbody2D attached to the Player object once at the start.
//        rb = GetComponent<Rigidbody2D>();
//        animator = GetComponent<Animator>();   
//    }

//    void Update()
//    {
//        // --- Horizontal movement ---
//        // Get input from keyboard (A/D or Left/Right arrows).
//        float moveInput = Input.GetAxis("Horizontal");
//        // Apply horizontal speed while keeping the current vertical velocity.
//        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

//        // Jump realated code for the Jump Feature (later)
//        // --- Ground check ---
//        // Create an invisible circle at the GroundCheck position.
//        // If this circle overlaps any collider on the "Ground" layer, player is grounded.
//        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

//        // --- Jump ---
//        // If player is grounded AND the Jump button (Spacebar by default) is pressed:
//        if (isGrounded && Input.GetButtonDown("Jump"))
//        {
//            // Set vertical velocity to jumpForce (launch upward).
//            // Horizontal velocity stays the same.
//            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

//        }

//        SetAnimation(moveInput); //Call animation function
//    }

//        private void SetAnimation(float moveInput)
//        {
//            if(isGrounded)   //on the ground
//            {
//                if(moveInput == 0) //not moving
//                {
//                    animator.Play("Player_Idle");//Play idle animation
//                }
//                else //moving
//                {
//                    animator.Play("Player_Run");//Play run animation
//                }
//            }
//            else //in the air not on ground
//            {
//                if(rb.linearVelocityY > 0) //going upward
//                {
//                    animator.Play("Player_Jump");//Play jump animation
//                }
//                else //going down
//                {
//                    animator.Play("Player_Fall");//Play fall animation
//                }
//            }

//        }
//    }

