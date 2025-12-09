using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{

    //public AudioClip coinClip;
    public int coinsToGive = 1;
    private TextMeshProUGUI coinText;


    //private TextMeshProUGUI coinText;
    //public int coinsToGive = 1;

    private void Start()
    {
        coinText = GameObject.FindWithTag("CoinText").GetComponent<TextMeshProUGUI>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>(); // playerController?
            player.coins += coinsToGive;
            // play coin sound effect when coin is collected at 40% volume
            //player.PlaySFX(coinClip, 0.4f);
            SoundManager.Instance.PlaySFX("COIN", 0.4f);
            coinText.text = player.coins.ToString();
            Destroy(gameObject);
        }
    }
}
