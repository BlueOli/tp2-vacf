using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public AudioSource audioSource;

    private void Start()
    {
        pauseMenuUI.SetActive(false); // Start with the pause menu hidden
    }

    private void Update()
    {
        // Toggle pause menu visibility when the specified key is pressed (e.g., "P")
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        // Toggle the visibility of the pause menu
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);

        // Pause/unpause the game time
        Time.timeScale = (pauseMenuUI.activeSelf) ? 0f : 1f;

        if(!pauseMenuUI.activeSelf)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
}