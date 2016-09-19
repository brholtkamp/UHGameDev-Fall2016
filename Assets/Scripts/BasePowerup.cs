using UnityEngine;

public abstract class BasePowerup : MonoBehaviour {
    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            var canUsePowerups = collision.gameObject.GetComponent<ICanUsePowerups>();
            if (canUsePowerups != null) {
                canUsePowerups.UsePowerup(this);
                Destroy(gameObject);
            }
        }
    }

    public abstract void ApplyPowerup(GameObject targetObject);
}
