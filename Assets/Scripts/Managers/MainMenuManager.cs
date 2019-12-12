using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OuterRimStudios.Utilities;

public class MainMenuManager : MonoBehaviour
{
    public float holdTime = 1.5f;
    [Header("The two scenes we're toggling between")]
    public ManualInput manualInput;
    public GameObject mainMenuText;
    AppManager appManager;

    HapticInput hapticInput;

    private bool buttonHeld;
    private bool isManual;

    float timer;

    // Start is called before the first frame update

    private void OnEnable()
    {
        hapticInput = GetComponent<HapticInput>();
        appManager = GetComponent<AppManager>();
        OVRInputManager.OnButtonDown += OnButtonDown;
        OVRInputManager.OnButtonUp += OnButtonUp;

        SetScene(isManual);
    }

    private void OnDisable()
    {
        OVRInputManager.OnButtonDown -= OnButtonDown;
        OVRInputManager.OnButtonUp -= OnButtonUp;
    }

    private void Update()
    {
        CheckCountdown();
    }

    void OnButtonDown(OVRInput.Button button)
    {
        if (button == OVRInput.Button.Two || Input.GetKeyDown(KeyCode.B))
        {
            buttonHeld = true;
        }
    }

    void OnButtonUp(OVRInput.Button button)
{
        if (button == OVRInput.Button.Two || Input.GetKeyUp(KeyCode.B))
        {
            buttonHeld = false;
        }     
    }

    void CheckCountdown()
    {
        if (buttonHeld)
        {
            if (MathUtilities.Timer(ref timer))
            {                
                isManual = !isManual;

                SetScene(isManual);

                ResetTimer();
            }
            else
                hapticInput.PerformHapticRumble();

        }
        else
            ResetTimer();
    }

    void ResetTimer()
    {
        timer = holdTime;
    }

    void SetScene(bool isManualInput)
    {
        appManager.enabled = !isManualInput;
        mainMenuText.SetActive(!isManualInput);
        manualInput.Activate(isManualInput);
    }
}
