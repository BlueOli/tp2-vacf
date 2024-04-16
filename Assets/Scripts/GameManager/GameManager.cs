using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject floor; // Reference to the floor GameObject
    public GameObject[] characters;

    public bool playerAlive = true; // Flag to track player's status
    private int numNPCsAlive; // Number of alive NPCs

    public Text endGameText;
    public bool isGameOver;

    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] NPCs = GameObject.FindGameObjectsWithTag("NPC");
        characters = new GameObject[players.Length + NPCs.Length];
        players.CopyTo(characters, 0);
        NPCs.CopyTo(characters, players.Length);

        // Initialize the number of alive NPCs
        numNPCsAlive = characters.Length - 1; // Exclude the player

        endGameText.text = "";
        endGameText.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (playerAlive)
        {
            // Check if any NPCs are inside the floor area
            foreach (var character in characters)
            {
                if (character != null)
                {
                    if (character.CompareTag("NPC") && !IsInsideFloorArea(character.transform))
                    {
                        // NPC is outside the floor area, mark as dead
                        character.GetComponent<NPCController>().isDead = true;
                        character.tag = "Dead_NPC";

                        numNPCsAlive--;
                        Debug.Log("NPC is outside the floor area!");
                    }
                }

            }

            // Check game win condition
            if (numNPCsAlive <= 0)
            {
                Debug.Log("Player wins! All NPCs are outside the floor area.");
                Time.timeScale = 0f;
                Victory();
            }
        }
        else
        {
            endGameText.text = "You Died! Press R to Restart";
            endGameText.gameObject.SetActive(true);
            isGameOver = true;
        }

        if(isGameOver)
        {
            if(Input.GetKeyUp(KeyCode.R))
            {
                ReloadScene();
            }
        }
    }

    void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Victory()
    {
        isGameOver = true;
        endGameText.text = "You Won! Press R to Restart";
        endGameText.gameObject.SetActive(true);
    }

    bool IsInsideFloorArea(Transform character)
    {
        // Check if the character's position is within the bounds of the floor
        Collider floorCollider = floor.GetComponent<Collider>();
        return floorCollider.bounds.Contains(character.position);
    }
}
