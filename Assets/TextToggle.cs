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

    public void SwitchText()
    {
        if (isOn)
            textDisplay.text = textOn;
        else
            textDisplay.text = textOff;

        isOn = !isOn;
    }
}
