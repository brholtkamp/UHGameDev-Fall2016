using System.Linq;
using UnityEngine;

public class BreakableBlockController : MonoBehaviour {
    public void OnCollisionEnter2D(Collision2D collision) {
        var marioController = collision.gameObject.GetComponent<MarioController>();

        if (marioController != null && marioController.IsBigMario && collision.contacts.All(point => point.normal == Vector2.up)) {
            FindObjectOfType<ScoreController>().Score += 100;
            Destroy(gameObject);
        }
    }
}
