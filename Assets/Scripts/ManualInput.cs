using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManualInput : MonoBehaviour
{
    public TextMeshProUGUI inputText;
    public TextMeshProUGUI groupIdText;
    // Start is called before the first frame update

    public void Submit()
    {
        ChicagoSceneTransition.Instance.InitializeUser(inputText.text, groupIdText.text.ToLower());

        gameObject.SetActive(false);

        ChicagoSceneTransition.Instance.NextScene();
    }
}
