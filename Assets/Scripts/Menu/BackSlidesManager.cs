using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackSlidesManager : MonoBehaviour
{
    [SerializeField]
    private Slider difficultySlider;

    [SerializeField]
    private List<Sprite> hellSprites = new List<Sprite>();

    private Image backgroundImage;

    private int section;
    private int index;
    private int newIndex;
    private int sliderLength;

    // Start is called before the first frame update
    void Start()
    {
        backgroundImage = GetComponent<Image>();

        sliderLength = (int)difficultySlider.maxValue;

        if (hellSprites.Count > 0)
        {
            section = sliderLength / hellSprites.Count;
        }

        index = 0;
        newIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        newIndex = (int)difficultySlider.value / section;

        if(newIndex >= hellSprites.Count) newIndex = hellSprites.Count-1;

        if(newIndex != index)
        {
            backgroundImage.sprite = hellSprites[newIndex];
            index = newIndex;
        }
    }
}
