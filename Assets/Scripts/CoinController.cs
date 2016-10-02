using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CoinController : MonoBehaviour {
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            FindObjectOfType<ScoreController>().Score += 100;
            FindObjectOfType<CoinManager>().AddCoin();
            Destroy(gameObject);
        }
    }
}
