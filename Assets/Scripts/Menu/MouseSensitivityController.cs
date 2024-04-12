using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityController : MonoBehaviour
{
    public float minSensitivity = 1f;
    public float maxSensitivity = 10f;
    public Slider sensitivitySlider;
    public Text sliderText;
    public CameraController controller;

    private void Start()
    {
        // Initialize the slider value based on the current mouse sensitivity
        sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 5f);
        SetMouseSensitivity(sensitivitySlider.value);
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        // Clamp sensitivity value within the specified range
        float clampedSensitivity = Mathf.Clamp(sensitivity, minSensitivity, maxSensitivity);

        // Set mouse sensitivity
        PlayerPrefs.SetFloat("MouseSensitivity", clampedSensitivity);
        PlayerPrefs.Save();
        controller.ChangeMouseSensitivy();
        sliderText.text = "Sensitivity: " + clampedSensitivity;
    }
}
