using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ActorController))]
[RequireComponent(typeof(Jumping))]
public class StarController : BasePowerup {
    private ActorController controller;
    private Jumping jumping;

    public float Direction = 1.0f;

    public void Awake() {
        controller = GetComponent<ActorController>();
        jumping = GetComponent<Jumping>();
    }

    public void Update() {
        // Move the star like any other actor
        controller.Move(Direction);

        // Make the star "bounce"
        if (controller.IsGrounded) {
            jumping.Jump();
        }
    }

    public new void OnCollisionEnter2D(Collision2D collision) {
        // Switch directions if we hit a wall of terrain
        if (collision.contacts.All(point => point.normal != Vector2.up) && collision.gameObject.layer == LayerMask.NameToLayer("Terrain")) {
            Direction *= -1.0f;
        }

        // Use the existing Mario collision logic
        base.OnCollisionEnter2D(collision);
    }

    public override void ApplyPowerup(GameObject targetObject) {
        // Get the MarioController
        var marioController = targetObject.GetComponent<MarioController>();
        if (marioController != null) {
            // Give Mario star power
            marioController.TurnStarMario();
        }
    }
}
