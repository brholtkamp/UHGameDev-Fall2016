using System.Linq;
using UnityEngine;

public class ActorController : MonoBehaviour {
    public float Speed = 1.0f;
    public float JumpHeight = 1.0f;

    public bool IsGrounded { get; private set; }

    public bool FacingRight {
        get { return spriteRenderer.flipX == false; }
        set { spriteRenderer.flipX = !value; }
    }

    private Vector2 velocity;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    public void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Make sure the collider is matching the sprite
        UpdateCollider();
    }

    public void FixedUpdate() {
        // Check our ground below us
        GroundCheck();

        if (velocity != Vector2.zero) {
            // Change the velocity of our actor
            rigidBody.velocity = velocity;
        }

        // Clear our our velocity
        velocity = Vector2.zero;
    }

    private void GroundCheck() {
        // Cast a box below us just a hair to see if there's any objects below us
        var hits = Physics2D.BoxCastAll(boxCollider.bounds.center, boxCollider.bounds.size * 0.9f, 0.0f, Vector2.down, 0.1f);

        // Check to see if any of the things we hit is in the Terrain layer
        if (hits.Any(hit => hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))) {
            IsGrounded = true;
        } else {
            IsGrounded = false;
        }
    }

    /// <summary>
    /// Move our actor in a desired direction
    /// </summary>
    /// <param name="direction">Positive is right, negative is left, 1.0f is the normal speed of the actor</param>
    public void Move(float direction) {
        // If our direction is positive, we're moving to the right
        if (direction > 0.0f) {
            FacingRight = true;
        // Otherwise, we're going left
        // Note: we don't care about 0.0f, because it'd be unusual for our character to constantly face right
        } else if (direction < 0.0f) {
            FacingRight = false;
        }

        // Set our velocity to move on the next FixedUpdate tick
        velocity = new Vector2(direction * Speed, rigidBody.velocity.y);
    }

    /// <summary>
    /// Make our actor jump
    /// </summary>
    public void Jump() {
        if (IsGrounded) {
            // Set our new velocity to jump to our predesignated height
            velocity = new Vector2(rigidBody.velocity.x, Mathf.Sqrt(-2.0f * JumpHeight * Physics2D.gravity.y));
        }
    }

    /// <summary>
    /// Sets our colliders to the correct size
    /// </summary>
    private void UpdateCollider() {
        boxCollider.size = spriteRenderer.bounds.size;
    }
}
