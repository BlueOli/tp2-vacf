using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderUpdateString : MonoBehaviour
{
    public Slider slider;
    public Text difficultyText;

    public void UpdateTextValue()
    {
        difficultyText.text = "Difficulty: " + slider.value;
    }
}
