using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CoinController : MonoBehaviour {
    public void OnTriggerEnter2D(Collider2D other) {
        // If we get hit by the player
        if (other.gameObject.tag == "Player") {
            // Add score
            FindObjectOfType<ScoreController>().Score += 100;

            // Add a coin
            FindObjectOfType<CoinManager>().AddCoin();

            // Remove this coin
            Destroy(gameObject);
        }
    }
}
