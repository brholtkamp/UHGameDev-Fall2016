using System.Linq;

using UnityEngine;

[RequireComponent(typeof(ActorController))]
[RequireComponent(typeof(Jumping))]
public class FireballController : MonoBehaviour {
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

    public void OnCollisionEnter2D(Collision2D collision) {
        var hurt = collision.gameObject.GetComponent<IHurt>();
        if (hurt != null) {
            hurt.Hurt(collision.contacts.First());
            Destroy(gameObject);
        } else if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain")) {
            if (collision.contacts.All(point => point.normal == Vector2.left || point.normal == Vector2.right)) {
                Destroy(gameObject);
            }
        }
    }
}
