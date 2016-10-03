using System.Linq;

using UnityEngine;

[RequireComponent(typeof(ActorController))]
[RequireComponent(typeof(Jumping))]
public class FireballController : MonoBehaviour {
    private ActorController controller;
    private Jumping jumping;

    /// <summary>
    /// Direction to move in
    /// </summary>
    public float Direction = 1.0f;

    public void Awake() {
        controller = GetComponent<ActorController>();
        jumping = GetComponent<Jumping>();
    }

    public void Update() {
        // Move the fireball
        controller.Move(Direction);

        // "Bounce" the fireball
        if (controller.IsGrounded) {
            jumping.Jump();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        // Determine if what we can collide with can be "hurt"
        var hurt = collision.gameObject.GetComponent<IHurt>();
        if (hurt != null) {
            // If so, hurt it!
            hurt.Hurt(collision.contacts.First());
            
            // Clean up our fireball
            Destroy(gameObject);
        // Check to see if we hit the terrain
        } else if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain")) {
            // If we collide with something to the left or right, we hit a wall
            if (collision.contacts.All(point => point.normal == Vector2.left || point.normal == Vector2.right)) {
                // Destroy the fireball
                Destroy(gameObject);
            }
        }
    }
}
