using UnityEngine;

public abstract class BasePowerup : MonoBehaviour {
    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            var canUsePowerups = collision.gameObject.GetComponent<ICanUsePowerups>();
            if (canUsePowerups != null) {
                FindObjectOfType<ScoreController>().Score += 1000;
                canUsePowerups.UsePowerup(this);
                Destroy(gameObject);
            }
        }
    }

    public abstract void ApplyPowerup(GameObject targetObject);
}
