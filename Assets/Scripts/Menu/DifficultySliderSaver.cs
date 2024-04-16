using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySliderSaver : MonoBehaviour
{
    public Slider slider; // Reference to the Slider component
    public DifficultySO sliderData; // Reference to the ScriptableObject

    // Function called when the slider value changes
    public void OnSliderValueChanged()
    {
        // Check if the ScriptableObject reference is valid
        if (sliderData != null)
        {
            // Update the slider value in the ScriptableObject
            sliderData.Difficulty = (int)slider.value;
        }
        else
        {
            // Log a warning if the ScriptableObject reference is not set
            Debug.LogWarning("SliderData reference is not set.");
        }
    }
}
