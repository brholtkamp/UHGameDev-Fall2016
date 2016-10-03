using System.Linq;

using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class ItemBlockController : MonoBehaviour {
    public GameObject item;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected BoxCollider2D boxCollider;

    /// <summary>
    /// Tracker to see if this item block has been used yet
    /// </summary>
    protected bool used;

    public void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public virtual void OnCollisionEnter2D(Collision2D collision) {
        // Check to make sure it's Mario and we're hit from below
        if (collision.gameObject.tag == "Player" && collision.contacts.All(point => point.normal == Vector2.up) && !used) {
            FindObjectOfType<ScoreController>().Score += 100;
            SpawnItem(item);
            ClearBlock();
        }
    }

    /// <summary>
    /// Spawns the item above the Item Block
    /// </summary>
    /// <param name="item"></param>
    protected void SpawnItem(GameObject item) {
        // Create a new instance of the item
        var newItem = Instantiate(item);
        
        // Move it one unit above the block itself
        newItem.transform.position = transform.position + new Vector3(0.0f, 1.0f, 0.0f);
    }

    /// <summary>
    /// Visually show that the block has been used
    /// </summary>
    protected void ClearBlock() {
        animator.SetTrigger("IsDestroyed");
        spriteRenderer.enabled = true;
        used = true;
    }
}