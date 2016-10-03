using UnityEngine;

public abstract class BasePowerup : MonoBehaviour {
    public void OnCollisionEnter2D(Collision2D collision) {
        // Only handle collisions with Mario
        if (collision.gameObject.tag == "Player") {
            // Get the component that can properly handle the powerup, namely MarioController
            var canUsePowerups = collision.gameObject.GetComponent<ICanUsePowerups>();
            if (canUsePowerups != null) {
                // Add score
                FindObjectOfType<ScoreController>().Score += 1000;

                // Pass this powerup onto Mario to use
                canUsePowerups.UsePowerup(this);

                // Destroy the powerup
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Handle the game logic that this powerup should do
    /// </summary>
    /// <param name="targetObject"></param>
    public abstract void ApplyPowerup(GameObject targetObject);
}
