using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextToggle : MonoBehaviour
{
    public string textOn;
    public string textOff;

    public TextMeshProUGUI textDisplay;

    bool isOff;

    private void Start()
    {
        Reset();
    }

    public void SwitchText()
    {
        isOff = !isOff;

        if (isOff)
            textDisplay.text = textOff;
        else
            textDisplay.text = textOn;        
    }

    public void Reset()
    {
        isOff = false;
        textDisplay.text = textOn;
    }
}
