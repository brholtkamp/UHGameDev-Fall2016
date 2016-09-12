using UnityEngine;

public class CameraController : MonoBehaviour {
    private Transform playerTransform;

    public void Awake() {
        // Find our Mario
        playerTransform = FindObjectOfType<MarioController>().transform;
    }

    public void LateUpdate() {
        // Let's move the camera if Mario isn't missing
        if (playerTransform != null) {
            // Move our camera so that the camera's x-axis is matching Mario's
            transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
        }
    }
}
