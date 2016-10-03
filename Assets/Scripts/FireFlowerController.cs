using UnityEngine;

public class FireFlowerController : BasePowerup {
    public override void ApplyPowerup(GameObject targetObject) {
        // Get our MarioController
        var controller = targetObject.GetComponent<MarioController>();
        if (controller != null) {
            // Transform Mario into FireMario
            controller.TurnFireMario();

            // Get rid of the FireFlower
            Destroy(gameObject);
        }
    }
}