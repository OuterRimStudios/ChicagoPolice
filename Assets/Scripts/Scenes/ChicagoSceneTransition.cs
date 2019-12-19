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

    public string UserID { get; private set; }
    public string GroupID { get; private set; }
    public int HeadsetID { get; private set; }
    public string TestTimestamp { get; private set; }

    int sceneIndex;
    bool isTestB;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        OVRInput.Update();      
    }

    public override void NextScene()
    {
        //gets the scene that is currently active and calls EndScene
        List<BaseScene> baseScene = GetActiveTest();
        baseScene[sceneIndex].EndScene();
        OnSceneEnded?.Invoke(baseScene[sceneIndex]);

        //increments to next scene, or resets to the beginning of the list
        if (sceneIndex < baseScene.Count - 1)
            sceneIndex++;
        else
            sceneIndex = 0;

        //starts new scene
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

    //returns the VideoScene of the video that was viewed last
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

    public void InitializeHeadset(int headsetID)
    {
        HeadsetID = headsetID;
        testA[0].gameObject.SetActive(true);
    }

    //sets user specific values for use in the analytics
    public void InitializeUser(string userID, string groupID)
    {
        UserID = userID;
        GroupID = groupID;
        TestTimestamp = DateTime.Now.ToString("MM/dd/yyyy H:mm");
        isTestB = GroupID.ToLower() == "b" ? true : false;
        NextScene();
    }
}