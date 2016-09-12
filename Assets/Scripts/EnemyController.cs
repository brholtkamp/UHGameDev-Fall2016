using UnityEngine;

[RequireComponent(typeof(ActorController))]
public abstract class EnemyController : MonoBehaviour, IHurt {
    protected ActorController controller;
    protected BoxCollider2D boxCollider;
    protected Rigidbody2D rigidBody;
    protected Animator animator;

    public float Direction = -1.0f;
    public bool Active = false;

    public void Awake() {
        controller = GetComponent<ActorController>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Update() {
        if (Active) {
            // Move our enemy
            controller.Move(Direction);
        }
    }

    public void FixedUpdate() {
        // We get half the size of our collider in order to determine how far we are from the center
        // We pad it with a tiny bit of distance to make sure we're not inside of the BoxCollider2D
        var distanceFromCenter = (boxCollider.bounds.size.y / 2.0f) + 0.1f;

        // We convert our direction into a Vector
        var directionVector = Direction * Vector2.right;

        // Let's send out a ray and see if we hit anything really close to us
        var hit = Physics2D.Raycast((Vector2)boxCollider.bounds.center + (directionVector * distanceFromCenter), directionVector, 0.15f);
        Debug.DrawRay((Vector2)boxCollider.bounds.center + (directionVector * distanceFromCenter), directionVector, Color.red);

        // If we hit something that isn't the player, we should turn around
        if (hit && hit.collider.tag != "Player") {
            // Change direction
            Direction *= -1.0f;
        }
    }

    public void OnBecameVisible() {
        // Turn active once we've been seen on the screen, because that's the behaviour we had in the original Super Mario Bros
        Active = true;
    }

    public abstract void OnCollisionEnter2D(Collision2D collision); 

    public abstract void Hurt(ContactPoint2D point);
}
