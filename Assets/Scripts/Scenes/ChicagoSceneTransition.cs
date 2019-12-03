using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OuterRimStudios.Utilities;

public class ChicagoSceneTransition : SceneTransition
{
    public delegate void SceneEvents(BaseScene baseScene);
    public static event SceneEvents OnSceneStarted;
    public static event SceneEvents OnSceneEnded;

    public static ChicagoSceneTransition Instance;

    public List<BaseScene> testA;
    public List<BaseScene> testB;
    HapticInput hapticInput;

    public float holdTime = 1.5f;

    public int headsetID = 1;
    public string UserID { get; private set; }
    public string GroupID { get; private set; }
    public int HeadsetID { get; private set; }
    public string TestTimestamp { get; private set; }

    int sceneIndex;
    bool isTestB;
    float timer;
    bool buttonHeld;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        hapticInput = GetComponent<HapticInput>();
        ResetTime();
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
        OVRInput.Update();
        if (sceneIndex == 0)
            CheckCountdown();
    }

    void OnButtonDown(OVRInput.Button button)
    {
        if (button == OVRInput.Button.One || button == OVRInput.Button.Two)
        {
            isTestB = button == OVRInput.Button.Two ? true : false;
            buttonHeld = true;
        }
    }

    void OnButtonUp(OVRInput.Button button)
    {
        if (button == OVRInput.Button.One || button == OVRInput.Button.Two)
            buttonHeld = false;
    }

    void CheckCountdown()
    {
        if (buttonHeld)
        {
            if (MathUtilities.Timer(ref timer))
            {
                NextScene();
            }
            else
                hapticInput.PerformHapticRumble();
        }
        else
            ResetTime();
    }

    void ResetTime()
    {
        timer = holdTime;
    }

    public override void NextScene()
    {
        List<BaseScene> baseScene = GetActiveTest();
        baseScene[sceneIndex].EndScene();
        OnSceneEnded?.Invoke(baseScene[sceneIndex]);

        if (sceneIndex < baseScene.Count - 1)
            sceneIndex++;
        else
            sceneIndex = 0;

        baseScene[sceneIndex].StartScene();
        OnSceneStarted?.Invoke(baseScene[sceneIndex]);
    }

    public BaseScene GetActiveScene()
    {
        return isTestB ? testB[sceneIndex] : testA[sceneIndex];
    }
    public BaseScene GetPreviousScene()
    {
        return isTestB ? testB[sceneIndex - 1] : testA[sceneIndex - 1];
    }

    public VideoScene GetLastVideo()
    {
        for(int i = sceneIndex; i > 0; i--)
        {
            if (GetActiveTest()[i].GetType() == typeof(VideoScene))
                return GetActiveTest()[i] as VideoScene;
        }

        return null;
    }

    List<BaseScene> GetActiveTest()
    {
        return isTestB ? testB : testA;
    }

    public void InitializeUser(string userID, string groupID)
    {
        UserID = userID;
        GroupID = groupID;
        HeadsetID = headsetID;
        TestTimestamp = DateTime.Now.ToString("MM/dd/yyyy H:mm");
        isTestB = GroupID.ToLower() == "b" ? true : false;
        NextScene();
    }
}