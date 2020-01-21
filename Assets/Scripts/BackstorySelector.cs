using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackstorySelector : MonoBehaviour
{
    public PerspectiveScene perspectiveScene;
    public CountdownManager countdown;
    public string videoPath;
    public float sphereRotation;

    public void SelectBackground()
    {
        Debug.LogError($"{transform.name} Backstory selected");
        SundanceSceneTransition.Instance.NextScene();
        countdown.StartCountdown();
        CountdownManager.OnCountdownFinished += PlayVideo;
        //perspectiveScene.Play(videoPath, sphereRotation);
        //gameObject.SetActive(false);
    }

    void PlayVideo()
    {
        perspectiveScene.Play(videoPath, sphereRotation);
        CountdownManager.OnCountdownFinished -= PlayVideo;
        gameObject.SetActive(false);
    }
}