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

    private void Awake()
    {
        Instance = this;
    }
    public override void NextScene()
    {
        baseScenes[sceneIndex].EndScene();
        OnSceneEnded?.Invoke(baseScenes[sceneIndex]);

        if (sceneIndex < baseScenes.Count - 1)
            sceneIndex++;
        else
            sceneIndex = 0;

        baseScenes[sceneIndex].StartScene();
        OnSceneStarted?.Invoke(baseScenes[sceneIndex]);
    }
}
