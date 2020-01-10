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

    public List<BaseScene> testScenes;

    public string UserID { get; private set; }
    public string GroupID { get; private set; }
    public int HeadsetID { get; private set; }
    public string TestTimestamp { get; private set; }

    int sceneIndex;

    private void Awake()
    {
        Instance = this;
        StartScene();
    }

    private void Update()
    {
        OVRInput.Update();
    }

    public override void NextScene()
    {
        EndScene();
        //increments to next scene, or resets to the beginning of the list
        if (sceneIndex < testScenes.Count - 1)
            sceneIndex++;
        else
            sceneIndex = 0;

        StartScene();
    }

    public void EndScene()
    {
        //gets the scene that is currently active and calls EndScene
        testScenes[sceneIndex].EndScene();
        OnSceneEnded?.Invoke(testScenes[sceneIndex]);
    }

    public void StartScene()
    {
        //starts new scene
        testScenes[sceneIndex].StartScene();
        OnSceneStarted?.Invoke(testScenes[sceneIndex]);
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
        return testScenes;
    }
}