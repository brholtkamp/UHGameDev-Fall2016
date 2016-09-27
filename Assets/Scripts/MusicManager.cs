using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {
    private AudioSource music;

    public void Awake() {
        music = GetComponent<AudioSource>();
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            SpeedUpMusic();
        } else if (Input.GetKeyDown(KeyCode.X)) {
            SpeedDownMusic();
        }
    }

    public void StartMusic() {
        music.Play();
    }

    public void StopMusic() {
        music.Stop();
    }

    public void SpeedUpMusic() {
        music.pitch = 1.25f;
    }

    public void SpeedDownMusic() {
        music.pitch = 1.0f;
    }
}
