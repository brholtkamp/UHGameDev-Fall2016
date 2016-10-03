using UnityEngine;

public class FireballPowerupController : MonoBehaviour {
    /// <summary>
    /// The 
    /// </summary>
    public float Cooldown = 0.5f;

    /// <summary>
    /// Are we currently on cooldown?
    /// </summary>
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

        // Store a copy of the prefab we load from the filesystem
        fireballPrefab = Resources.Load<GameObject>("Prefabs/Fireball");
    }

    public void Update() {
        // Trigger only if we get the input and we're not on cooldown
        if (Input.GetKeyDown(KeyCode.LeftControl) && !IsOnCooldown) {
            // Create a new copy of the fireball
            var newFireball = Instantiate(fireballPrefab);

            // Make the fireball ignore collision events with Mario
            Physics2D.IgnoreCollision(newFireball.GetComponent<BoxCollider2D>(), gameObject.GetComponent<BoxCollider2D>());

            // Move the fireball to eithet the left or right hand of Mario, depending on which direction we're facing
            newFireball.transform.position = new Vector2(controller.FacingRight ? boxCollider.bounds.max.x + 0.3f : boxCollider.bounds.min.x - 0.3f, boxCollider.bounds.center.y + 0.25f);

            // Set the initial direction of the FireballController from the fireball
            var fireballController = newFireball.GetComponent<FireballController>();
            if (fireballController != null) {
                fireballController.Direction = controller.FacingRight ? 1.0f : -1.0f;
            }

            // Set our cooldown back to 0.0
            currentCooldown = 0.0f;
        }

        // Increment our cooldown so that we can determine when we can fire the next one
        currentCooldown += Time.deltaTime;
    }
}