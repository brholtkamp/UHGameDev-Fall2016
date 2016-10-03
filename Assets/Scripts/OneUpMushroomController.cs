using UnityEngine;

public class OneUpMushroomController : MushroomController {
    private GameManager gameManager;

    public new void Awake() {
        // Get a handle on the GameManager
        gameManager = FindObjectOfType<GameManager>();
        base.Awake();
    }

    public override void ApplyPowerup(GameObject targetObject) {
        // Increase the lives available
        gameManager.Lives++;
        Destroy(gameObject);
    }
}
