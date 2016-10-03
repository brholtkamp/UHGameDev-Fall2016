using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {
    private AudioSource music;

    public void Awake() {
        music = GetComponent<AudioSource>();
    }

    public void Update() {
        // Debugging commands
        if (Input.GetKeyDown(KeyCode.Z)) {
            SpeedUpMusic();
        } else if (Input.GetKeyDown(KeyCode.X)) {
            SpeedDownMusic();
        }
    }

    /// <summary>
    /// Start playing the music
    /// </summary>
    public void StartMusic() {
        music.Play();
    }

    /// <summary>
    /// Stop playing the music
    /// </summary>
    public void StopMusic() {
        music.Stop();
    }

    /// <summary>
    /// Speed up the music
    /// </summary>
    public void SpeedUpMusic() {
        music.pitch = 1.25f;
    }

    /// <summary>
    /// Speed down the music
    /// </summary>
    public void SpeedDownMusic() {
        music.pitch = 1.0f;
    }
}
