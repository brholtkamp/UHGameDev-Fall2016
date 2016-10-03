using System.Linq;
using UnityEngine;

public class UpgradeItemBlockController : ItemBlockController {
    /// <summary>
    /// A secondary item to handle what Big Mario receives
    /// </summary>
    public GameObject bigMarioItem;

    public override void OnCollisionEnter2D(Collision2D collision) {
        // Do our initial check for the player hit from below
        if (collision.gameObject.tag == "Player" && collision.contacts.All(point => point.normal == Vector2.up) && !used) {
            // Get our MarioController
            var marioController = collision.gameObject.GetComponent<MarioController>();
            if (marioController != null) {
                // Add some points for this item block
                FindObjectOfType<ScoreController>().Score += 100;

                // If Mario happens to be Big Mario, switch out what GameObject is spawned
                SpawnItem(marioController.IsBigMario ? bigMarioItem : item);
                ClearBlock();
            }
        }
    }
}