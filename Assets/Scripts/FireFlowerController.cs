using UnityEngine;

public class FireFlowerController : BasePowerup {
    public override void ApplyPowerup(GameObject targetObject) {
        var controller = targetObject.GetComponent<MarioController>();
        if (controller != null) {
            controller.TurnFireMario();
            Destroy(gameObject);
        }
    }
}