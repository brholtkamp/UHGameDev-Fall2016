using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform playerTransform;

    public void Awake() {
        playerTransform = FindObjectOfType<MarioController>().transform;
    }

    public void LateUpdate() {
        transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
    }
}
