using UnityEngine;

[RequireComponent(typeof(ActorController))]
public class MarioController : MonoBehaviour {
    public float RunMultiplier = 1.5f;

    private ActorController controller;

    public void Awake() {
        controller = GetComponent<ActorController>();
    }

    public void Update() {
        var horizontalInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horizontalInput) > 0.0f) {
            if (Input.GetKey(KeyCode.LeftShift) && controller.IsGrounded) {
                controller.Move(horizontalInput * RunMultiplier);
            } else {
                controller.Move(horizontalInput);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            controller.Jump();
        }
    }
}