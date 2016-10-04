using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    /// <summary>
    /// Line of which, if they player falls below, they should die
    /// </summary>
    public float DeathPlane = -5.0f;

    public int Lives = 3;

    /// <summary>
    /// Path to save data
    /// </summary>
    /// <returns></returns>
    public string FilePath {
        get {
            return Application.persistentDataPath + "/lives.json";
        }
    }

    private Transform playerTransform;

    private bool died = false;

    public void Awake() {
        if (File.Exists(FilePath)) {
            var data = (SaveData)JsonUtility.FromJson(File.ReadAllText(FilePath), typeof(SaveData));
            Lives = data.Lives;
        }

        playerTransform = FindObjectOfType<MarioController>().transform;
    }

    public void Update() {
        // Check to see if the player is below the death plane
        if (playerTransform != null && playerTransform.position.y < DeathPlane && !died) {
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
        // Remove a life
        Lives--;

        // Stop music
        FindObjectOfType<MusicManager>().StopMusic();

        // Play audio clip
        FindObjectOfType<Camera>().GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/MarioDied"));

        // Make Mario fly upwards a small amount and fall down
        var playerRigidbody = playerTransform.GetComponent<Rigidbody2D>();
        playerRigidbody.velocity = Vector2.up * 20.0f;

        // Make sure Mario doesnt' collide with anything now
        var playerCollider = playerTransform.GetComponent<BoxCollider2D>();
        playerCollider.enabled = false;

        // Disable player input
        var playerController = playerTransform.GetComponent<MarioController>();
        playerController.enabled = false;

        died = true;

        if (Lives > -1) {
            StartCoroutine(ShowRespawnScreen());
        } else {
            File.Delete(FilePath);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    private IEnumerator ShowRespawnScreen() {
        yield return new WaitForSeconds(4.0f);

        // Find the UI
        var canvas = GameObject.Find("Canvas");

        // Black out the background and tell the camera to stop rendering stuff in the scene
        var camera = FindObjectOfType<Camera>();
        camera.backgroundColor = Color.black;
        camera.cullingMask = 0;

        // Turn on the lives text
        GameObject lives = null;
        foreach (Transform child in canvas.transform) {
            if (child.name == "Lives") {
                lives = child.gameObject;
            }
        }
        lives.SetActive(true);

        // Change the number of lives
        var text = lives.GetComponentInChildren<Text>();
        text.text = "World 1-1 \n\n x " + Lives;

        yield return new WaitForSeconds(4.0f);

        var data = new SaveData { Lives = Lives };
        var jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(FilePath, jsonData);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private class SaveData {
        public int Lives;
    }
}