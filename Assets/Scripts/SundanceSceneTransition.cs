using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SundanceSceneTransition : SceneTransition
{
    public static SundanceSceneTransition Instance;

    public delegate void SceneEvents(BaseScene baseScene);
    public static event SceneEvents OnSceneStarted;
    public static event SceneEvents OnSceneEnded;

    public List<BaseScene> baseScenes;

    int sceneIndex;
    BaseScene currentScene;

    private void Awake()
    {
        Instance = this;
        currentScene = baseScenes[0];
        //currentScene.StartScene();
    }
    public override void NextScene()
    {
        //calls end scene on the currently playing scene
        currentScene?.EndScene();
        OnSceneEnded?.Invoke(currentScene);

        if (sceneIndex < baseScenes.Count - 1)
            sceneIndex++;
        else
            sceneIndex = 0;

        //starts next scene
        currentScene = baseScenes[sceneIndex];
        currentScene?.StartScene();
        OnSceneStarted?.Invoke(currentScene);
    }

    public void ChangeScene(BaseScene nextScene)
    {
        currentScene?.EndScene();
        OnSceneEnded?.Invoke(currentScene);
        currentScene = nextScene;
        currentScene?.StartScene();
        OnSceneStarted?.Invoke(currentScene);
    }
}
