using System.Linq;
using UnityEngine;

public class BreakableBlockController : MonoBehaviour {
    public void OnCollisionEnter2D(Collision2D collision) {
        // Get our MarioController
        var marioController = collision.gameObject.GetComponent<MarioController>();

        // If it's Mario, and we're Big Mario, and we hit it from the bottom
        if (marioController != null && marioController.IsBigMario && collision.contacts.All(point => point.normal == Vector2.up)) {
            // Destroy this block
            Destroy(gameObject);
        }
    }
}
