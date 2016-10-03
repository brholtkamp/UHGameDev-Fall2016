using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {
    private Text scoreText;

    /// <summary>
    /// Update the current score
    /// </summary>
    public int Score {
        get { return score; }
        set {
            // Set the score
            score = value;

            // Update the text on the screen
            scoreText.text = "MARIO\n" + score.ToString("D6");
        }
    }

    private int score;

    public void Awake() {
        // Find the score text
        scoreText = GetComponent<Text>();
    }
}
