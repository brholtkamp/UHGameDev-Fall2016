using UnityEngine;

using System.Collections;

[RequireComponent(typeof(ActorController))]
[RequireComponent(typeof(Jumping))]
[RequireComponent(typeof(Animator))]
public class MarioController : MonoBehaviour {
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

    private ActorController controller;
    private Jumping jumping;
    private Animator animator;

    /// <summary>
    /// Tracker to make sure the player was running before jumping if they want to keep the horizontal momentum of running
    /// </summary>
    private bool wasRunningBeforeJump = false;

    public void Awake() {
        controller = GetComponent<ActorController>();
        jumping = GetComponent<Jumping>();
        animator = GetComponent<Animator>();
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
        StartCoroutine(ChangeAnimatorController("AnimationControllers/LittleMarioController"));
    }

    /// <summary>
    /// Transform this Mario into big Mario
    /// </summary>
    public void TurnBigMario() {
        StartCoroutine(ChangeAnimatorController("AnimationControllers/BigMarioController"));
    }

    /// <summary>
    /// Transform this Mario into fire Mario
    /// </summary>
    public void TurnFireMario() {
        StartCoroutine(ChangeAnimatorController("AnimationControllers/FireMarioController"));
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
}