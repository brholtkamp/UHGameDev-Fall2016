using UnityEngine;

[RequireComponent(typeof(ActorController))]
public class Jumping : MonoBehaviour {
    public float JumpHeight = 5.0f;

    private ActorController controller;
    private Rigidbody2D rigidBody;

    public void Awake() {
        controller = GetComponent<ActorController>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Make our actor jump
    /// </summary>
    public void Jump() {
        if (controller.IsGrounded) {
            // Set our new Velocity to jump to our predesignated height
            controller.Velocity = new Vector2(rigidBody.velocity.x, Mathf.Sqrt(-2.0f * JumpHeight * Physics2D.gravity.y));
        }
    }

    /// <summary>
    /// Make our actor bounce upwards
    /// </summary>
    /// <param name="amount">Amount of force to bounce by</param>
    public void Bounce(float amount) {
        controller.Velocity = new Vector2(controller.Velocity.x, amount);
    }
}