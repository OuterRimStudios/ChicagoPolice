using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SundanceSceneTransition : SceneTransition
{
    public static SundanceSceneTransition Instance;

    public delegate void SceneEvents(BaseScene baseScene);
    public static event SceneEvents OnSceneStarted;
    public static event SceneEvents OnSceneEnded;
    public AudioSource source;
    public AudioClip transitionClip;

    public List<BaseScene> baseScenes;

    int sceneIndex;
    BaseScene currentScene;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentScene = baseScenes[0];
        StartScene();
    }

    public override void NextScene()
    {
        source.PlayOneShot(transitionClip);
        //calls end scene on the currently playing scene
        EndScene();

        if (sceneIndex < baseScenes.Count - 1)
            sceneIndex++;
        else
            sceneIndex = 0;

        //starts next scene
        currentScene = baseScenes[sceneIndex];
        StartScene();
    }

    public void EndScene()
    {
        currentScene?.EndScene();
        OnSceneEnded?.Invoke(currentScene);
    }

    public void StartScene()
    {
        currentScene?.StartScene();
        OnSceneStarted?.Invoke(currentScene);
        Debug.LogError("Starting Scene: " + currentScene.name);
    }

    public void PreviousScene()
    {
        //calls end scene on the currently playing scene
        EndScene();

        if (sceneIndex > 0)
            sceneIndex--;
        else
            sceneIndex = 0;

        //starts next scene
        currentScene = baseScenes[sceneIndex];
        StartScene();
    }

    public void ChangeScene(BaseScene nextScene)
    {
        currentScene?.EndScene();
        OnSceneEnded?.Invoke(currentScene);
        currentScene = nextScene;
        sceneIndex = baseScenes.IndexOf(nextScene);
        currentScene?.StartScene();
        OnSceneStarted?.Invoke(currentScene);
    }
}
