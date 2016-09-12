using System.Collections;
using UnityEngine;

public class KoopaController : EnemyController {
    public bool IsShell = false;
    public float ShellSpeed = 12.0f;

    public override void OnCollisionEnter2D(Collision2D collision) {
        foreach (var point in collision.contacts) {
            // If we're still a Koopa and we get stomped, get hurt and turn into a shell
            if (!IsShell) {
                if (point.normal == Vector2.down) {
                    Hurt(point);
                    return;
                }

                if ((point.normal == Vector2.left || point.normal == Vector2.right) && collision.gameObject.tag == "Player") {
                    collision.gameObject.GetComponent<MarioController>().Hurt(point);
                    return;
                }
            } else {
                // If we're a shell and we get stomped, fly to the opposite side of where we got stomped
                if (point.normal == Vector2.down) {
                    if (collision.collider.bounds.center.x <= boxCollider.bounds.center.x) {
                        MoveShell(1.0f);
                        return;
                    }

                    MoveShell(-1.0f);
                    return;
                }

                // If we're a shell and we get hit on the left, we have to check if we're moving yet or not
                if (point.normal == Vector2.left || point.normal == Vector2.right) {
                    // If we're not moving, check if it's the player and if so, fly in that direction for being kicked
                    if (Mathf.Abs(rigidBody.velocity.x) <= 0.0f) {
                        if (collision.gameObject.tag == "Player") {
                            Direction = -1.0f * point.normal.x;
                            return;
                        }
                    // We're flying, thus we need to hurt stuff
                    } else if (collision.gameObject.tag == "Actor" || collision.gameObject.tag == "Player") {
                        var hurtableComponent = collision.gameObject.GetComponent<IHurt>();
                        if (hurtableComponent != null) {
                            hurtableComponent.Hurt(point);
                            return;
                        }
                    }
                }
            }
        }
    }

    public override void Hurt(ContactPoint2D point) {
        // If we're a normal Koopa, get hurt and turn into a shell
        if (!IsShell) {
            IsShell = true;
            StartCoroutine(HurtAnimation());
            Direction = 0.0f;
            controller.Speed = 0.0f;
        // We're currently a shell, which means only other Koopa shells can kill us
        } else {
            // Find out if it's a Koopa and it's a shell
            var isKoopa = point.collider.GetComponent<KoopaController>();
            if (isKoopa && isKoopa.IsShell) {
                // Well, we gotta die now
                Death();
            }
        }
    }

    /// <summary>
    /// Moves the shell in a desired direction
    /// </summary>
    /// <param name="direction">-1.0f for left, 1.0f for right</param>
    private void MoveShell(float direction) {
        // Set our shell's speed
        controller.Speed = ShellSpeed;

        // Set the direction to what we're given
        Direction = direction;
    }

    /// <summary>
    /// Animation to convert the Koopa into a shell
    /// </summary>
    /// <returns></returns>
    private IEnumerator HurtAnimation() {
        // Transition to hurt state
        animator.SetTrigger("IsHit");

        // Let it render the hurt animation
        yield return new WaitForEndOfFrame();

        // Update our collider so the shell is correctly placed in the world
        controller.UpdateCollider();
    }

    /// <summary>
    /// Remove the Koopa from the world
    /// </summary>
    private void Death() {
        Destroy(gameObject);
    }
}