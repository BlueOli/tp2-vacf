using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsButtons : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject mouseController;

    public void MouseController(bool active)
    {
        mouseController.SetActive(active);
        mainMenu.SetActive(!active);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
