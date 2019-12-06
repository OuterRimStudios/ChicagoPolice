using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManualInput : MonoBehaviour
{
    public TextMeshProUGUI inputText;
    public TextMeshProUGUI groupIdText;

    public GameObject[] gosManualInput;

    public void Submit()
    {
        ChicagoSceneTransition.Instance.InitializeUser(inputText.text, groupIdText.text.ToLower());
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
