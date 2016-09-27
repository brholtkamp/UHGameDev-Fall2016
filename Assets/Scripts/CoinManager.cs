using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour {
    public int Coins { get; private set; }

    private Text coinText;
    private GameManager gameManager;
    private AudioSource audioSource;

    public void Awake() {
        gameManager = GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();

        coinText = GameObject.Find("CoinCount").GetComponent<Text>();
    }

    public void Update() {
        coinText.text = Coins.ToString("D2");
    }

    public void AddCoin() {
        Coins++;
        audioSource.Play();
        if (Coins == 100) {
            Coins = 0;
            gameManager.Lives++;
        }
    }
}
