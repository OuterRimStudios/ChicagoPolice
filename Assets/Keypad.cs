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
        if (inputText.text.Length == 0)
        {
            defaultText.gameObject.SetActive(false);
        }

        Text text = button.GetComponentInChildren<Text>();
        inputText.text += text.text;
    }

    public void Backspace()
    {
        if (inputText.text.Length > 0)
        {
            inputText.text = inputText.text.Remove(inputText.text.Length - 1);
        }

        if (inputText.text.Length == 0)
        {
            defaultText.gameObject.SetActive(true);
        }
    }

    public void CloseKeyboard()
    {
        gameObject.SetActive(false);
    }    

}
