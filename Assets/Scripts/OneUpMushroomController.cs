using UnityEngine;

public class OneUpMushroomController : MushroomController {
    private GameManager gameManager;

    public new void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        base.Awake();
    }

    public override void ApplyPowerup(GameObject targetObject) {
        gameManager.Lives++;
        Destroy(gameObject);
    }
}
