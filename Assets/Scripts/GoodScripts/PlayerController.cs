using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour {

    public PlayerUI playerUI;
    public GameObject deathMenu;
    public PauseMenu pauseMenu;
    public PlayerController controler;
    private Animator animator;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    public AudioSource gameBackgroundMuisc;

    [SerializeField] Transform groundCheck;
    [SerializeField] Transform groundCheckL;
    [SerializeField] Transform groundCheckR;

    [SerializeField] Transform celingCheck;
    [SerializeField] Transform celingCheckL;
    [SerializeField] Transform celingCheckR;

    [SerializeField] Collider2D circleColliderNormal;
    [SerializeField] Collider2D circleColliderCrouching;

    private float moveSpeed = 8f;
    private float crouchSpeed = .5f;
    private float jumpSpeed = 15f;

    private float fallMultiplier = 3f;
    private float lowJumpMultiplier = 3f;

    private bool isGrounded;
    private bool celingAbove;
    private bool isCrouching;
    private bool isMovingRight = false;
    private bool isMovingLeft = false;
    private bool isJumping = false;
    public bool onLadder = false;
    public static bool isDead = false;

    public TextMeshProUGUI displayFinalCherryCount;
    public TextMeshProUGUI displayFinalGemCount;
    public TextMeshProUGUI displayFinalStarCount;
    public TextMeshProUGUI displayFinalTime;

    public Vector3 GetPosition() {
        return transform.position;
    }
    private void Start() {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Physics2D.gravity = new Vector2(0, -9.8f);

    }

    // Use Update() for controller input and animations
    private void Update() {
        //moveSpeed = Input.GetAxisRaw("Horizontal") * runSpeed;
        //animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        isMovingRight = false;
        isMovingLeft = false;
        isJumping = false;


        // Ground check
        if ((Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Floor"))) ||
            (Physics2D.Linecast(transform.position, groundCheckL.position, 1 << LayerMask.NameToLayer("Floor"))) ||
            (Physics2D.Linecast(transform.position, groundCheckR.position, 1 << LayerMask.NameToLayer("Floor")))) {

            isGrounded = true;
            rb2d.velocity = new Vector2(0, 0);
        }
        else {
            isGrounded = false;
            if (rb2d.velocity.y > 0 && !onLadder) {
                animator.Play("player_jump");
            }
            else if (rb2d.velocity.y < 0 && !onLadder) {
                animator.Play("player_jump_fall");
            }
            else if (onLadder) {
                animator.Play("player_climb");
            }
        }

        // Ceiling check
        if ((Physics2D.Linecast(transform.position, celingCheck.position, 1 << LayerMask.NameToLayer("Floor"))) ||
            (Physics2D.Linecast(transform.position, celingCheckL.position, 1 << LayerMask.NameToLayer("Floor"))) ||
            (Physics2D.Linecast(transform.position, celingCheckR.position, 1 << LayerMask.NameToLayer("Floor")))) {
            celingAbove = true;
            circleColliderNormal.enabled = false;
            circleColliderCrouching.enabled = true;
        }
        else {
            celingAbove = false;
            circleColliderNormal.enabled = true;
            circleColliderCrouching.enabled = false;
        }

        // Move right
        if ((Input.GetKey("d") || Input.GetKey("right"))) {
            isMovingRight = true;
            spriteRenderer.flipX = false;
            if (isGrounded && !isCrouching && !celingAbove) {
                animator.Play("player_run");
            }
            else if (isGrounded && isCrouching && !celingAbove) {
                animator.Play("player_crouch");
            }
            else if (isGrounded && isCrouching && celingAbove) {
                animator.Play("player_crouch");
            }
        }
        // Move left
        else if ((Input.GetKey("a") || Input.GetKey("left"))) {
            isMovingLeft = true;
            spriteRenderer.flipX = true;
            if (isGrounded && !isCrouching && !celingAbove) {
                animator.Play("player_run");
            }
            else if (isGrounded && isCrouching && !celingAbove) {
                animator.Play("player_crouch");
            }
            else if (isGrounded && isCrouching && celingAbove) {
                animator.Play("player_crouch");
            }
        }

        // Crouching but not moving
        else if (isGrounded && (celingAbove || isCrouching)) {
            animator.Play("player_crouch");
        }

        else {
            // Player idle
            if (isGrounded) {
                animator.Play("player_idle");
            }
            else if (onLadder) {
                animator.Play("player_climb");
            }
        }

        // Crouching
        if ((Input.GetKey("s") || Input.GetKey("down"))) {
            if (onLadder) {
                isCrouching = false;
            }
            else {
                isCrouching = true;
                circleColliderNormal.enabled = false;
                circleColliderCrouching.enabled = true;
            }
        }
        else {
            isCrouching = false;
        }

        // Jump
        if (Input.GetKey("space") && isGrounded && !celingAbove && !isCrouching) {
            FindObjectOfType<AudioManager>().Play("Mario_Jump");
            isJumping = true;
        }

        if (onLadder) {
            if (rb2d.velocity.y > 0 || rb2d.velocity.y < 0) {
                animator.Play("player_climb");
            }
            rb2d.gravityScale = 0f;
            rb2d.velocity = new Vector2(rb2d.velocity.x, moveSpeed * Input.GetAxisRaw("Vertical"));
        }
        if (!onLadder) {
            rb2d.gravityScale = 1f;
        }


    }

    // Use FixedUpdate() for physics
    private void FixedUpdate() {
        // Move right
        if (isMovingRight && !isCrouching && !celingAbove) {
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
        }
        // Move left
        else if (isMovingLeft && !isCrouching && !celingAbove) {
            rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
        }
        // Crouching
        else if (isCrouching && isGrounded) {
            // Crouch and move right
            if (isCrouching && isMovingRight) {
                rb2d.velocity = new Vector2(moveSpeed * crouchSpeed, rb2d.velocity.y);
                spriteRenderer.flipX = false;
            }
            // Crouch and move left
            else if (isCrouching && isMovingLeft) {
                rb2d.velocity = new Vector2(-moveSpeed * crouchSpeed, rb2d.velocity.y);
                spriteRenderer.flipX = true;
            }
            // Crouch and no movement
            else {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
        }
        // Player idle
        else {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        // Jump
        if ((isJumping && isGrounded && !celingAbove && !isCrouching)) {
            rb2d.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
        // Jump optimization
        if (rb2d.velocity.y < 0 && !onLadder) {
            rb2d.gravityScale = fallMultiplier; // the larger the gravity scale, the faster the player falls back down
        }
        else if (rb2d.velocity.y > 0 && !onLadder) {
            rb2d.gravityScale = lowJumpMultiplier;
        }
        else if (!onLadder) {
            rb2d.gravityScale = 1f;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Floor") {
            isGrounded = true;
            rb2d.velocity = Vector3.zero;
            rb2d.angularVelocity = 0f;
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Spikes" || other.gameObject.tag == "KillZone") {
            gameBackgroundMuisc.enabled = false;
            FindObjectOfType<AudioManager>().Play("GameOver");
            rb2d.velocity = new Vector2(0, 0);
            Physics2D.gravity = Vector2.zero;
            animator.Play("player_die");
            controler.enabled = false;
            pauseMenu.enabled = false;
            playerUI.enabled = false;
            deathMenu.SetActive(true);

            displayFinalCherryCount.text = playerUI.cherryCount.ToString("00");
            displayFinalGemCount.text = playerUI.gemCount.ToString("00");
            displayFinalStarCount.text = playerUI.gemCount.ToString("00");
            displayFinalTime.text = playerUI.currentTime.ToString("0000");

        }
        if (other.gameObject.tag == "Ladder") {
            onLadder = true;
        }
        else {
            onLadder = false;
        }
    }


}