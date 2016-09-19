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
        controller.Move(Direction);

        if (controller.IsGrounded) {
            jumping.Jump();
        }
    }

    public new void OnCollisionEnter2D(Collision2D collision) {
        if (collision.contacts.All(point => point.normal != Vector2.up) && collision.gameObject.layer == LayerMask.NameToLayer("Terrain")) {
            Direction *= -1.0f;
        }

        base.OnCollisionEnter2D(collision);
    }

    public override void ApplyPowerup(GameObject targetObject) {
        var marioController = targetObject.GetComponent<MarioController>();
        if (marioController != null) {
            marioController.TurnStarMario();
        }
    }
}
