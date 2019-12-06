using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextToggle : MonoBehaviour
{
    public string textOn;
    public string textOff;

    public TextMeshProUGUI textDisplay;

    bool isOn;

    private void Start()
    {
        Reset();
    }

    public void SwitchText()
    {
        if (isOn)
            textDisplay.text = textOn;
        else
            textDisplay.text = textOff;

        isOn = !isOn;
    }

    public void Reset()
    {
        isOn = false;
        textDisplay.text = textOff;
    }
}
