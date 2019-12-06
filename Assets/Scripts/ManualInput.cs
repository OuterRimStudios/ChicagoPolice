using UnityEngine;

public class ManualInput : MonoBehaviour
{
    public TextToggle groupId;
    public Keypad userId;   

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
        gameObject.SetActive(isActive);
    }
}
