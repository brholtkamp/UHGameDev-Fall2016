using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour {
    /// <summary>
    /// Number of coins
    /// </summary>
    public int Coins { get; private set; }

    private Text coinText;
    private GameManager gameManager;
    private AudioSource audioSource;

    public void Awake() {
        // Grab the necessary components
        gameManager = GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();

        // Find the text for Coins
        coinText = GameObject.Find("CoinCount").GetComponent<Text>();
    }

    public void Update() {
        // Set the coin text with 2 digits
        coinText.text = Coins.ToString("D2");
    }

    public void AddCoin() {
        // Increment the coin counter
        Coins++;
        
        // Play the coin noise
        audioSource.Play();

        // If we hit 100, reset it and add a life
        if (Coins == 100) {
            Coins = 0;
            gameManager.Lives++;
        }
    }
}
