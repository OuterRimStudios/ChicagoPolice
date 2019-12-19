using UnityEngine;

public class ManualInput : MonoBehaviour
{
    public TextToggle groupId;
    public Keypad userId;   

    public GameObject[] gosManualInput;

    //sets the values for the specific user for later use in analytics
    public void Submit()
    {
        ChicagoSceneTransition.Instance.InitializeUser(userId.inputText.text, groupId.textDisplay.text);
        userId.Reset();
        groupId.Reset();
        Activate(false);
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