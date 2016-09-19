using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    /// <summary>
    /// Line of which, if they player falls below, they should die
    /// </summary>
    public float DeathPlane = -5.0f;

    public int Lives = 3;

    private Transform playerTransform;

    public void Awake() {
        playerTransform = FindObjectOfType<MarioController>().transform;
    }

    public void Update() {
        // Check to see if the player is below the death plane
        if (playerTransform != null && playerTransform.position.y < DeathPlane) {
            Dead();
        }

        // Allow us to restart the scene
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// The player died
    /// </summary>
    public void Dead() {
        Debug.Log("Game Over!");
        Destroy(playerTransform.gameObject);
        playerTransform = null;
    }
}