using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.coins += 1;
            // play coin sound effect when coin is collected at 40% volume
            SoundManager.Instance.PlaySFX("COIN", 0.4f);
            Destroy(gameObject);
        }
    }
}
