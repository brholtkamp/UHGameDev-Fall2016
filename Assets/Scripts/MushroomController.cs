using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ActorController))]
public class MushroomController : BasePowerup {
    private ActorController controller;

    public float Direction = 1.0f;

    public void Awake() {
        controller = GetComponent<ActorController>();
    }

    public void Update() {
        controller.Move(Direction);
    }

    public new void OnCollisionEnter2D(Collision2D collision) {
        if (collision.contacts.All(point => point.normal != Vector2.up) && collision.gameObject.layer == LayerMask.NameToLayer("Terrain")) {
            Direction *= -1.0f;
        }

        base.OnCollisionEnter2D(collision);
    }

    public override void ApplyPowerup(GameObject targetObject) {
        var controller = targetObject.GetComponent<MarioController>();
        if (controller != null) {
            if (!controller.IsBigMario) {
                controller.TurnBigMario();
            }
        }
    }
}