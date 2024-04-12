using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButtons : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject mouseController;

    public void MouseController(bool active)
    {
        mouseController.SetActive(active);
        mainMenu.SetActive(!active);
    }
}
