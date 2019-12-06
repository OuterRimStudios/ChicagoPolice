using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Keypad : MonoBehaviour
{
    public List<Button> inputKeys;
    public TMP_InputField inputField;

    void Awake()
    {
        foreach (Button button in inputKeys)
            button.onClick.AddListener(() => Type(button));
    }   

    public void Type(Button button)
    {
        Text text = button.GetComponentInChildren<Text>();
        inputField.text += text.text;
    }

    public void Backspace()
    {
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Remove(inputField.text.Length - 1);
        }
    }

    public void CloseKeyboard()
    {
        gameObject.SetActive(false);
    }    

}
