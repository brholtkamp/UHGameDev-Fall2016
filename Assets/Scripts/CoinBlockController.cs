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
        if (collision.gameObject.tag == "Player" && collision.contacts.All(point => point.normal == Vector2.up) && !used) {
            FindObjectOfType<ScoreController>().Score += 100;
            CoinsRemaining--;
            coinManager.AddCoin();

            if (CoinsRemaining == 0) {
                animator.SetTrigger("IsDestroyed");
                used = true;
            }
        }
    }
}
