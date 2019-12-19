using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeadsetSelectionManager : MonoBehaviour
{
    public TextMeshProUGUI idText;
    public GameObject mainPanel;
    public GameObject acknowledgementPanel;
    public TextMeshProUGUI acknowledgementText;
    int currentID = 1;
    bool acknowledgedID;

    private void OnEnable()
    {
        acknowledgedID = false;
        OVRInputManager.OnButtonDown += OnButtonDown;
    }

    private void OnDisable()
    {
        OVRInputManager.OnButtonDown -= OnButtonDown;
    }

    private void OnButtonDown(OVRInput.Button key)
    {
        if (key == OVRInput.Button.SecondaryThumbstickLeft && !acknowledgementPanel.activeInHierarchy)
            ChangeID(-1);   //decreases the ID by 1
        else if (key == OVRInput.Button.SecondaryThumbstickRight && !acknowledgementPanel.activeInHierarchy)
            ChangeID(1);    //increases the ID by 1
        else if (key == OVRInput.Button.One)
            SubmitID();
        else if (key == OVRInput.Button.Two && acknowledgementPanel.activeInHierarchy)
            Cancel();
    }

    //adds the value passed in to the current ID
    void ChangeID(int changeAmount)
    {
        if (currentID + changeAmount < 1)
            return;

        currentID += changeAmount;
        idText.text = currentID.ToString();
    }


    void SubmitID()
    {
        //This will activate a confirmation panel the first time submit is called
        if (!acknowledgementPanel.activeInHierarchy)
        {
            mainPanel.SetActive(false);
            acknowledgementText.text = $"Are you sure you want to set the ID of this headset to {currentID}?";
            acknowledgementPanel.SetActive(true);
        }
        else
            acknowledgedID = true;

        //if the user has confirmed the ID is correct, this sets the ID of this headset and proceeds with the experience
        if (acknowledgedID)
        {
            gameObject.SetActive(false);
            ChicagoSceneTransition.Instance.InitializeHeadset(currentID);
            ChicagoSceneTransition.Instance.enabled = true;
        }
    }

    //allows the user to return to the menu where they set the value of the ID
    void Cancel()
    {
        if(acknowledgementPanel.activeInHierarchy)
        {
            acknowledgedID = false;
            acknowledgementPanel.SetActive(false);
            mainPanel.SetActive(true);
        }
    }
}