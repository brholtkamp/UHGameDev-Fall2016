using System.Collections;
using UnityEngine;

public class GoombaController : EnemyController {
    public override void OnCollisionEnter2D(Collision2D collision) {
        foreach (var point in collision.contacts) {
            if (point.normal == Vector2.down) {
                Hurt(point);
                return;
            }

            if ((point.normal == Vector2.left || point.normal == Vector2.right) && collision.gameObject.tag == "Player") {
                collision.gameObject.GetComponent<MarioController>().Hurt(point);
                return;
            }
        }
    }

    public override void Hurt(ContactPoint2D point) {
        // Add score
        FindObjectOfType<ScoreController>().Score += 100;

        StartCoroutine(DeathAnimation());
    }

    /// <summary>
    /// Coroutine to do the death animation
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeathAnimation() {
        // Stop moving the Goomba
        controller.Move(0.0f);
        rigidBody.velocity = Vector2.zero;

        // Disable the collider so it stops interfering with the world
        boxCollider.enabled = false;

        // Start the hurt animation
        animator.SetTrigger("IsHit");

        // Let the Goomba sprite linger for a moment
        yield return new WaitForSeconds(0.1f);

        // Remove the Goomba from the game
        Destroy(gameObject);
    }
}