using System.Linq;

using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class ItemBlockController : MonoBehaviour {
    public GameObject item;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected BoxCollider2D boxCollider;

    protected bool used;

    public void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public virtual void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" && collision.contacts.All(point => point.normal == Vector2.up) && !used) {
            FindObjectOfType<ScoreController>().Score += 100;
            SpawnItem(item);
            ClearBlock();
        }
    }

    protected void SpawnItem(GameObject item) {
        var newItem = Instantiate(item);
        newItem.transform.position = transform.position + new Vector3(0.0f, 1.0f, 0.0f);
    }

    protected void ClearBlock() {
        animator.SetTrigger("IsDestroyed");
        spriteRenderer.enabled = true;
        used = true;
    }
}