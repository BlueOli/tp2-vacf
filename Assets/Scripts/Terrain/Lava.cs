using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public GameManager gameManager;

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "Player" or "NPC" tag
        if (collision.gameObject.CompareTag("Dead_NPC") || collision.gameObject.CompareTag("Grabbable"))
        {
            // Destroy the collided game object
            Destroy(collision.gameObject);  
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game over! Player touched the lava.");
            Time.timeScale = 0f;
            gameManager.playerAlive = false;
        }
    }
}
