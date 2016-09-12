using System.Linq;

using UnityEngine;

public class ActorController : MonoBehaviour {
    /// <summary>
    /// Speed of our actor's horizontal movement
    /// </summary>
    public float Speed = 1.0f;

    /// <summary>
    /// Is this actor currently on the ground?
    /// </summary>
    public bool IsGrounded { get; private set; }

    /// <summary>
    /// Velocity of this actor
    /// </summary>
    public Vector2 Velocity { get; set; }

    /// <summary>
    /// Is this actor facing to the right? (Default pose)
    /// </summary>
    public bool FacingRight {
        get { return spriteRenderer.flipX == false; }
        set { spriteRenderer.flipX = !value; }
    }

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    /// <summary>
    /// Tracker to make sure that we leave the ground
    /// </summary>
    private bool hasLeftGround;

    public void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Make sure the collider is matching the sprite
        UpdateCollider();
    }

    public void FixedUpdate() {
        // Check our ground below us
        GroundCheck();

        if (Velocity != Vector2.zero) {
            // Change the Velocity of our actor
            rigidBody.velocity = Velocity;

            // If we're supposed to be going up, and we're already grounded, make sure we leave the ground
            if (Velocity.y > 0.0f && IsGrounded) {
                IsGrounded = false;
                hasLeftGround = false;
            }
        }

        // Clear our our Velocity
        Velocity = Vector2.zero;
    }

    /// <summary>
    /// Checks the ground and assigns IsGrounded
    /// </summary>
    private void GroundCheck() {
        // Cast a box below us just a hair to see if there's any objects below us
        var hits = Physics2D.BoxCastAll(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x * 0.9f, boxCollider.bounds.size.y), 0.0f, Vector2.down, 0.1f);

        // Check to see if any of the things we hit is in the Terrain layer and that we've already left the ground
        if (hits.Any(hit => hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain")) && hasLeftGround) {
            IsGrounded = true;
        } else {
            IsGrounded = false;

            // If we can't hit the ground anymore, that means we've successfully left the ground below us
            hasLeftGround = true;
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

        // Set our Velocity to move on the next FixedUpdate tick
        Velocity = new Vector2(direction * Speed, rigidBody.velocity.y);
    }

    /// <summary>
    /// Sets our colliders to the correct size
    /// </summary>
    public void UpdateCollider() {
        boxCollider.size = spriteRenderer.bounds.size;
    }
}