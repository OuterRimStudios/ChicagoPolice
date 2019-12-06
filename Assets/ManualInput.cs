using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManualInput : MonoBehaviour
{
    public Keypad userId;
    public TextToggle groupId;

    public GameObject[] gosManualInput;

    public void Submit()
    {
        ChicagoSceneTransition.Instance.InitializeUser(userId.inputText.text, groupId.textDisplay.text);
        userId.Reset();
        groupId.Reset();
        Activate(false);
        ChicagoSceneTransition.Instance.NextScene();        
    }

    public void Activate(bool isActive)
    {
        foreach (GameObject go in gosManualInput)
        {
            go.SetActive(isActive);
        }
    }
}
