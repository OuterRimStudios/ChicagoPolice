using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioScene[] audioScenes;
    AudioSource source;

    private void Awake()
    {
        SundanceSceneTransition.OnSceneStarted += SceneStarted;
    }
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        SundanceSceneTransition.OnSceneStarted -= SceneStarted;
    }

    void SceneStarted(BaseScene baseScene)
    {
        AudioScene scene = GetScene(baseScene);
        Debug.LogError("Scene: " + scene);
     
        if (scene.scene != null)
        {
            if(scene.playMusic)
            {
                Debug.LogError("Playing: " + scene.clip.name);
                source.clip = scene.clip;
                source.Play();
            }
            else
                source.Stop();
        }
    }

    AudioScene GetScene(BaseScene scene)
    {
        return audioScenes.FirstOrDefault(x => x.scene == scene);
    }
}

[System.Serializable]
public struct AudioScene
{
    public BaseScene scene;
    public bool playMusic;
    [ConditionalHide("playMusic", true)]
    public AudioClip clip;
}
