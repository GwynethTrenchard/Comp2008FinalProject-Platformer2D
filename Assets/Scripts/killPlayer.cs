using UnityEngine;

public class killPlayer : MonoBehaviour
{
    // Detect collision with the Player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the colliding object is tagged "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            
            collision.gameObject.GetComponent<PlayerHealth>().Die();
        }
    }
}
