using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class CoinBlockController : MonoBehaviour {
    public int CoinsRemaining;

    private Animator animator;
    private CoinManager coinManager;

    private bool used = false;

    public void Awake() {
        animator = GetComponent<Animator>();
        coinManager = FindObjectOfType<CoinManager>();
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        // Trigger if we're hit by Mario and it's from below
        if (collision.gameObject.tag == "Player" && collision.contacts.All(point => point.normal == Vector2.up) && !used) {
            // Add score
            FindObjectOfType<ScoreController>().Score += 100;

            // Decrement the number of coins in this block
            CoinsRemaining--;

            // Add a coin to the CoinManager
            coinManager.AddCoin();

            // If we hit 0, then this CoinBlock is exhausted
            if (CoinsRemaining == 0) {
                animator.SetTrigger("IsDestroyed");
                used = true;
            }
        }
    }
}
