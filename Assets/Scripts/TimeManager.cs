using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {
    public int CurrentTime = 400;
    public int LowTime = 100;
    public float TimeMultiplier = 1.75f;

    private Text timeText;
    private float timeElapsed;
    private MusicManager musicManager;

    public void Awake() {
        timeText = GameObject.Find("Time").GetComponent<Text>();
        musicManager = FindObjectOfType<MusicManager>();
    }

    public void Update() {
        timeText.text = "Time\n" + CurrentTime;

        timeElapsed += Time.deltaTime * TimeMultiplier;

        if (timeElapsed >= 1.0f) {
            CurrentTime--;
            timeElapsed -= 1.0f;
        }

        if (CurrentTime < LowTime) {
            musicManager.SpeedUpMusic();
        }
    }
}
