using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OuterRimStudios.Utilities;

public class ManualInputManager : GameObjectScene
{
    public float holdTime = 1.5f;
    [Header("The two scenes we're toggling between")]
    public ManualInput manualInput;
    public GameObjectScene mainMenu;

    HapticInput hapticInput;

    private bool buttonHeld;
    private bool isManual;
    private bool isScene;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        manualInput.Activate(false);
        hapticInput = GetComponent<HapticInput>();
    }

    public override void StartScene()
    {
        isScene = true;
        if (isManual)
        {
            manualInput.Activate(true);
        }
        else
        {
            mainMenu.StartScene();
        }
    }

    public override void EndScene()
    {
        isScene = false;
    }

    private void OnEnable()
    {
        OVRInputManager.OnButtonDown += OnButtonDown;
        OVRInputManager.OnButtonUp += OnButtonUp;
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
        if (isScene)
        {
            if (button == OVRInput.Button.Two || Input.GetKeyDown(KeyCode.B))
            {
                buttonHeld = true;
            }
        }        
    }

    void OnButtonUp(OVRInput.Button button)
    {
        if (isScene)
        {
            if (button == OVRInput.Button.Two || Input.GetKeyUp(KeyCode.B))
            {
                buttonHeld = false;
            }
        }        
    }

    void CheckCountdown()
    {
        if (buttonHeld)
        {
            if (MathUtilities.Timer(ref timer))
            {
                isManual = !isManual;

                if (isManual)
                {
                    manualInput.Activate(true);
                    mainMenu.EndScene();
                }
                else
                {
                    mainMenu.StartScene(); 
                    manualInput.Activate(false);
                }

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
}
