using UnityEngine;

public class FireballPowerupController : MonoBehaviour {
    public float Cooldown = 0.5f;

    public bool IsOnCooldown {
        get { return currentCooldown < Cooldown; }
    }

    private BoxCollider2D boxCollider;
    private ActorController controller;

    private GameObject fireballPrefab;
    private float currentCooldown;

    public void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
        controller = GetComponent<ActorController>();

        fireballPrefab = Resources.Load<GameObject>("Prefabs/Fireball");
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !IsOnCooldown) {
            var newFireball = Instantiate(fireballPrefab);

            Physics2D.IgnoreCollision(newFireball.GetComponent<BoxCollider2D>(), gameObject.GetComponent<BoxCollider2D>());

            newFireball.transform.position = new Vector2(controller.FacingRight ? boxCollider.bounds.max.x + 0.3f : boxCollider.bounds.min.x - 0.3f, boxCollider.bounds.center.y + 0.25f);

            var fireballController = newFireball.GetComponent<FireballController>();
            if (fireballController != null) {
                fireballController.Direction = controller.FacingRight ? 1.0f : -1.0f;
            }

            currentCooldown = 0.0f;
        }

        currentCooldown += Time.deltaTime;
    }
}