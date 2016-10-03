using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {
    /// <summary>
    /// The amount of time remaining in the level
    /// </summary>
    public int CurrentTime = 400;

    /// <summary>
    /// The amount of time remaining to change the music
    /// </summary>
    public int LowTime = 100;

    /// <summary>
    /// Multiplier to change the delta time by
    /// </summary>
    public float TimeMultiplier = 1.75f;

    private Text timeText;
    private float timeElapsed;
    private MusicManager musicManager;

    public void Awake() {
        // Find the Time text
        timeText = GameObject.Find("Time").GetComponent<Text>();

        // Find the MusicManager
        musicManager = FindObjectOfType<MusicManager>();
    }

    public void Update() {
        // Update the text
        timeText.text = "Time\n" + CurrentTime;

        // Determine time elapsed between frames
        timeElapsed += Time.deltaTime * TimeMultiplier;

        // If the amount of time has passed 1.0, then we can actually change the amount of time
        // This is due to the time between frame is typically under a second
        if (timeElapsed >= 1.0f) {
            CurrentTime--;
            timeElapsed -= 1.0f;
        }

        // Speed up the music if we pass the LowTime threshold
        if (CurrentTime < LowTime) {
            musicManager.SpeedUpMusic();
        }
    }
}
