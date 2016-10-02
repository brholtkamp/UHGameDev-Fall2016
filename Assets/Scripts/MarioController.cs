using UnityEngine;

using System.Collections;
using System.Linq;

[RequireComponent(typeof(ActorController))]
[RequireComponent(typeof(Jumping))]
[RequireComponent(typeof(Animator))]
public class MarioController : MonoBehaviour, IHurt, ICanUsePowerups {
    /// <summary>
    /// Multiplier for the velocity of running
    /// </summary>
    public float RunMultiplier = 1.5f;

    /// <summary>
    /// Are we currently Big Mario?
    /// </summary>
    public bool IsBigMario {
        get { return !animator.runtimeAnimatorController.name.Contains("Little"); }
    }

    /// <summary>
    /// Are we currently Fire Mario?
    /// </summary> 
    public bool IsFireMario {
        get { return animator.runtimeAnimatorController.name.Contains("Fire"); }
    }

    /// <summary>
    /// Are we currently Star Mario?
    /// </summary>
    public bool IsStarMario {
        get { return currentStarTime > 0.0f; }
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

    /// <summary>
    /// Length of Mario's star animation from a Star powerup
    /// </summary>
    public float StarDuration = 8.0f;

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
    /// Tracker for the amount of star time left
    /// </summary>
    private float currentStarTime;

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

        if (IsStarMario) {
            currentStarTime -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            TurnBigMario();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            TurnFireMario();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            TurnStarMario();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if (IsStarMario) {
            var hurt = collision.gameObject.GetComponent<IHurt>();
            if (hurt != null) {
                hurt.Hurt(collision.contacts.First());
            }
        }
    }

    public void Hurt(ContactPoint2D point) {
        // Only allow us to get hurt when we're no longer invulnerable
        if (!IsInvulnerable && !IsStarMario) {
            // Turn into big Mario
            if (IsFireMario) {
                TurnBigMario();
                currentInvulnerabilityTime = InvulnerabilityDuration;
            }
            // Turn into little Mario
            else if (IsBigMario) {
                TurnLittleMario();
                currentInvulnerabilityTime = InvulnerabilityDuration;
            // Oops, we've died
            } else {
                gameManager.Dead();
            }
        }
    }

    public void UsePowerup(BasePowerup powerup) {
        powerup.ApplyPowerup(gameObject);
    }

    /// <summary>
    /// Transform this Mario into little Mario
    /// </summary>
    public void TurnLittleMario() {
        StartCoroutine(ChangeAnimatorController("AnimationControllers/LittleMarioController"));
    }

    /// <summary>
    /// Transform this Mario into big Mario
    /// </summary>
    public void TurnBigMario() {
        var fireballPowerup = GetComponent<FireballPowerupController>();
        if (fireballPowerup != null) {
            Destroy(fireballPowerup);
        }

        StartCoroutine(ChangeAnimatorController("AnimationControllers/BigMarioController"));
    }

    /// <summary>
    /// Transform this Mario into fire Mario
    /// </summary>
    public void TurnFireMario() {
        var existingFireballController = GetComponent<FireballPowerupController>();
        if (existingFireballController == null) {
            gameObject.AddComponent<FireballPowerupController>();
        }

        StartCoroutine(ChangeAnimatorController("AnimationControllers/FireMarioController"));
    }

    public void TurnStarMario() {
        currentStarTime = StarDuration;
        StartCoroutine(StarAnimation());
    }

    private IEnumerator ChangeAnimatorController(string name) {
        // Move ourselves up 0.5 units since Mario is growing taller
        if (!IsBigMario) {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f);
        }

        // Assign our new animator controller
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(name);

        // Wait a frame so the sprite updates
        yield return new WaitForEndOfFrame();

        // Update the collider to this new sprite
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

    private IEnumerator StarAnimation() {
        // https://simple.wikipedia.org/wiki/Rainbow
        Color[] colors = {
            new Color(255, 0, 0),
            new Color(255, 127, 0), 
            new Color(255, 255, 0), 
            new Color(0, 255, 0), 
            new Color(0, 0, 255), 
            new Color(75, 0, 130), 
            new Color(139, 0, 255)
        };

        while (IsStarMario) {
            foreach (var color in colors) {
                spriteRenderer.color = color;
                yield return new WaitForEndOfFrame();
            }
        }

        spriteRenderer.color = Color.white;
    }
}
