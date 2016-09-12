using UnityEngine;

[RequireComponent(typeof(ActorController))]
[RequireComponent(typeof(Jumping))]
[RequireComponent(typeof(Animator))]
public class MarioController : MonoBehaviour {
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
            }
            // Handle the case for normal walking
            else {
                controller.Move(horizontalInput);
                wasRunningBeforeJump = false;
            }
        }
        // There was no input, so we should be idle
        else if (controller.Velocity == Vector2.zero) {
            // Idle
        }

        // Make Jumpman jump!
        if (Input.GetKeyDown(KeyCode.Space)) {
            jumping.Jump();
        }
    }

    /// <summary>
    /// Transform this Mario into little Mario
    /// </summary>
    public void TurnLittleMario() {
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimationControllers/LittleMarioController");
        controller.UpdateCollider();
    }

    /// <summary>
    /// Transform this Mario into big Mario
    /// </summary>
    public void TurnBigMario() {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f);
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimationControllers/BigMarioController");
        controller.UpdateCollider();
    }
}