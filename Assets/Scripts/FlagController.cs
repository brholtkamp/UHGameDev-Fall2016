using UnityEngine;
using System.Collections;

public class FlagController : MonoBehaviour {
    public float FlagAnimationDuration = 1.0f;

    private GameObject flag;

    public void Awake() {
        // Iterate through all of our children
        foreach (Transform child in transform) {
            // Find the one named flag
            if (child.name == "Flag") {
                flag = child.gameObject;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            StartCoroutine(FlagAnimation());
        }
    }

    private IEnumerator FlagAnimation() {
        // Disable player input
        var player = FindObjectOfType<MarioController>();
        player.enabled = false;

        // Disable gravity
        var playerRigidbody = player.GetComponent<Rigidbody2D>();
        playerRigidbody.isKinematic = true;

        // Stop the music
        var music = FindObjectOfType<MusicManager>();
        music.StopMusic();

        // Stop the timer
        var time = FindObjectOfType<TimeManager>();
        time.enabled = false;

        // Store the top of the flag to reach
        var topYPosition = flag.transform.position.y;

        // Move the flag down to the player
        flag.transform.position = new Vector2(flag.transform.position.x, player.transform.position.y);

        // Determine how far we have to move per frame
        var distance = (topYPosition - flag.transform.position.y) / FlagAnimationDuration;

        while (flag.transform.position.y < topYPosition) {
            var distanceToMove = distance * Time.deltaTime;

            player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + distanceToMove);
            flag.transform.position = new Vector2(flag.transform.position.x, flag.transform.position.y + distanceToMove);
            yield return new WaitForEndOfFrame();
        }

        // Remove the player object
        player.gameObject.SetActive(false);

        // Stop the camera from moving automatically
        var camera = FindObjectOfType<CameraController>();
        camera.enabled = false;

        // Determine the end point of the camera, of which is just 6 units over
        var destinationX = camera.transform.position.x + 6.0f;

        // Slide over until the camera is focused on the castle
        while (camera.transform.position.x < destinationX) {
            var distanceToMove = distance * Time.deltaTime;
            camera.transform.position = new Vector3(camera.transform.position.x + distanceToMove, camera.transform.position.y, camera.transform.position.z);
            yield return new WaitForEndOfFrame();
        }

        // Determine the number of fireworks to use, which is only if the last digit of the time is an odd number
        var numberOfFireworks = time.CurrentTime % 2 != 0 ? time.CurrentTime % 10 : 0;
        
        // Grab the score manager
        var scoreManager = FindObjectOfType<ScoreController>();

        // Reduce the time until 0
        while (time.CurrentTime > 0) {
            time.CurrentTime -= 9;

            // Add points for the time
            scoreManager.Score += 100;

            // If we happen to go below, correct it back up to 0
            if (time.CurrentTime < 0) {
                time.CurrentTime = 0;
            }

            yield return new WaitForEndOfFrame();
        }

        while (numberOfFireworks > 0) {
            // Find a new location randomly above the castle
            var location = new Vector2(camera.transform.position.x + Random.Range(-5, 5), 7 + Random.Range(0, 3));

            // Create a new firework at the location
            var fireworkGameObject = Instantiate(Resources.Load<GameObject>("Prefabs/Fireworks"), location, Quaternion.identity);

            // Consume this firework
            numberOfFireworks--;

            // Wait for the animation to finish
            yield return new WaitForSeconds(0.5f);

            // Clean up the firework
            Destroy(fireworkGameObject);
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
