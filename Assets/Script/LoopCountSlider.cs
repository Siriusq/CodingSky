using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopCountSlider : MonoBehaviour
{
    public Slider loopSlider;
    private Text loopCount;
    // Start is called before the first frame update
    void Start()
    {
        loopCount = GetComponent<Text>();
        ShowSliderValue();
    }

    public void ShowSliderValue()
    {
        loopCount.text = "Count: " + String.Format("{0:00}", loopSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
