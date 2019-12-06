using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Keypad : MonoBehaviour
{
    public List<Button> inputKeys;
    public TextMeshProUGUI inputText;
    public TextMeshProUGUI defaultText;

    void Awake()
    {
        foreach (Button button in inputKeys)
            button.onClick.AddListener(() => Type(button));
    }   

    public void Type(Button button)
    {       
        Text text = button.GetComponentInChildren<Text>();
        inputText.text += text.text;
    }

    public void Backspace()
    {
        if (inputText.text.Length > 0)
        {
            inputText.text = inputText.text.Remove(inputText.text.Length - 1);
        }
    }

    public void CloseKeyboard()
    {
        if (inputText.text.Length == 0)
        {
            defaultText.gameObject.SetActive(true);
        }

        gameObject.SetActive(false);
    }

    public void Reset()
    {
        inputText.text = "";
        defaultText.gameObject.SetActive(true);
    }

}
