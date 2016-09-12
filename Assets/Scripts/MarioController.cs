using UnityEngine;

[RequireComponent(typeof(ActorController))]
[RequireComponent(typeof(Jumping))]
[RequireComponent(typeof(Animator))]
public class MarioController : MonoBehaviour, IHurt {
    /// <summary>
    /// Multiplier for the velocity of running
    /// </summary>
    public float RunMultiplier = 1.5f;

    /// <summary>
    /// Amount of force to bounce off an enemy with
    /// </summary>
    public float BounceAmountWhenHittingEnemy = 6.0f;

    /// <summary>
    /// Are we currently Big Mario?
    /// </summary>
    public bool IsBigMario {
        get { return animator.runtimeAnimatorController.name.Contains("Big"); }
    }

    /// <summary>
    /// Is Mario currently invulnerable?
    /// </summary>
    public bool IsInvulnerable {
        get { return currentInvulnerabilityTime > 0.0f; }
    }

    /// <summary>
    /// Length of Mario's invulnerability after being hit
    /// </summary>
    public float InvulnerabilityDuration = 0.5f;

    private ActorController controller;
    private Jumping jumping;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;

    /// <summary>
    /// Tracker for the amount of invulnerability left
    /// </summary>
    private float currentInvulnerabilityTime;

    /// <summary>
    /// Tracker to make sure the player was running before jumping if they want to keep the horizontal momentum of running
    /// </summary>
    private bool wasRunningBeforeJump = false;

    public void Awake() {
        controller = GetComponent<ActorController>();
        jumping = GetComponent<Jumping>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        gameManager = FindObjectOfType<GameManager>();
    }

    public void Update() {
        // Get the input from A/D of WASD or Left/Right from arrow keys
        var horizontalInput = Input.GetAxis("Horizontal");

        // Check to see if there even is an input
        if (Mathf.Abs(horizontalInput) > 0.0f) {
            // Handle the case for running
            if (Input.GetKey(KeyCode.LeftShift) && (controller.IsGrounded || wasRunningBeforeJump)) {
                controller.Move(horizontalInput * RunMultiplier);
                wasRunningBeforeJump = true;
                RunAnimation(true);
            }
            // Handle the case for normal walking
            else {
                controller.Move(horizontalInput);
                wasRunningBeforeJump = false;
                WalkAnimation(true);
            }
        }
        // There was no input, so we should be idle
        else if (controller.Velocity == Vector2.zero) {
            IdleAnimation();
        }

        // Make Jumpman jump!
        if (Input.GetKeyDown(KeyCode.Space)) {
            jumping.Jump();
            JumpAnimation();
        }

        // See if there's any input on WS for WASD or Up/Down for arrow keys
        var verticalInput = Input.GetAxis("Vertical");

        // Check to see if there's any downwards input
        if (verticalInput < 0.0f) {
            // Crouch and shrink the collider
            CrouchingAnimation(true);
            controller.UpdateCollider();
        } else if (verticalInput >= 0.0f && animator.GetBool("IsCrouching")){
            // Stand and enlarge the collider
            CrouchingAnimation(false);
            controller.UpdateCollider();
        }

        // Pass IsGrounded into the Animator
        GroundedAnimation(controller.IsGrounded);

        // If we've recently got hit, we should flicker our sprite to show how long we've got invulnerability
        if (IsInvulnerable) {
            currentInvulnerabilityTime -= Time.deltaTime;

            spriteRenderer.enabled = !spriteRenderer.enabled;
        } else if (!spriteRenderer.enabled) {
            spriteRenderer.enabled = true;
        }
    }

    public void Hurt(ContactPoint2D point) {
        // Only allow us to get hurt when we're no longer invulnerable
        if (!IsInvulnerable) {
            // Turn into little Mario
            if (IsBigMario) {
                TurnLittleMario();
                currentInvulnerabilityTime = InvulnerabilityDuration;
            // Oops, we've died
            } else {
                gameManager.Dead();
            }
        }
    }

    /// <summary>
    /// Transform this Mario into little Mario
    /// </summary>
    public void TurnLittleMario() {
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimationControllers/LittleMarioController");
        controller.UpdateCollider();
        HurtAnimation();
    }

    /// <summary>
    /// Transform this Mario into big Mario
    /// </summary>
    public void TurnBigMario() {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f);
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimationControllers/BigMarioController");
        controller.UpdateCollider();
    }

    private void WalkAnimation(bool value) {
        animator.SetBool("IsWalking", value);
    }

    private void RunAnimation(bool value) {
        WalkAnimation(value);
        animator.SetBool("IsRunning", value);
    }

    private void JumpAnimation() {
        animator.SetTrigger("JumpPressed");
        animator.SetBool("IsGrounded", false);
    }

    private void GroundedAnimation(bool value) {
        animator.SetBool("IsGrounded", value);
    }

    private void CrouchingAnimation(bool value) {
        animator.SetBool("IsCrouching", value);
    }

    private void HurtAnimation() {
        animator.SetTrigger("Hurt");
    }

    private void IdleAnimation() {
        WalkAnimation(false);
        RunAnimation(false);
    }
}