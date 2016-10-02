using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {
    private Text scoreText;

    public int Score {
        get { return score; }
        set {
            score = value;
            scoreText.text = "MARIO\n" + score.ToString("D6");
        }
    }

    private int score;

    public void Awake() {
        scoreText = GetComponent<Text>();
    }
}
