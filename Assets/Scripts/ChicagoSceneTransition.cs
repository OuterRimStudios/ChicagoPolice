using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChicagoSceneTransition : MonoBehaviour
{
    public delegate void SceneEvents(BaseScene baseScene);
    public static event SceneEvents OnSceneStarted;

    public static ChicagoSceneTransition Instance;

    public List<BaseScene> testA;
    public List<BaseScene> testB;

    int sceneIndex;
    bool isTestB;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        OVRInput.Update();
        if (sceneIndex == 0 && OVRInput.GetDown(OVRInput.Button.One))
            isTestB = false;
        else if (sceneIndex == 0 && OVRInput.GetDown(OVRInput.Button.Two))
            isTestB = true;

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            NextScene();
    }

    public void NextScene()
    {
        List<BaseScene> baseScene = GetActiveTest();
        baseScene[sceneIndex].EndScene();

        if (sceneIndex < baseScene.Count)
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
}
