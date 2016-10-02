using System.Linq;
using UnityEngine;

public class UpgradeItemBlockController : ItemBlockController {
    public GameObject bigMarioItem;

    public override void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" && collision.contacts.All(point => point.normal == Vector2.up) && !used) {
            var marioController = collision.gameObject.GetComponent<MarioController>();
            if (marioController != null) {
                FindObjectOfType<ScoreController>().Score += 100;
                SpawnItem(marioController.IsBigMario ? bigMarioItem : item);
                ClearBlock();
            }
        }
    }
}