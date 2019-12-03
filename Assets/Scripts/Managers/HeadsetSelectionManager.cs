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
    int currentID;
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
            ChangeID(-1);
        else if (key == OVRInput.Button.SecondaryThumbstickRight && !acknowledgementPanel.activeInHierarchy)
            ChangeID(1);
        else if (key == OVRInput.Button.One)
            SubmitID();
        else if (key == OVRInput.Button.Two && acknowledgementPanel.activeInHierarchy)
            Cancel();
    }

    void ChangeID(int changeAmount)
    {
        if (currentID + changeAmount < 1)
            return;

        currentID += changeAmount;
        idText.text = currentID.ToString();
    }

    void SubmitID()
    {
        if (!acknowledgementPanel.activeInHierarchy)
        {
            mainPanel.SetActive(false);
            acknowledgementText.text = $"Are you sure you want to set the ID of this headset to {currentID}?";
            acknowledgementPanel.SetActive(true);
        }
        else
            acknowledgedID = true;

        if (acknowledgedID)
        {
            gameObject.SetActive(false);
            ChicagoSceneTransition.Instance.InitializeHeadset(currentID);
            ChicagoSceneTransition.Instance.enabled = true;
        }
    }

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