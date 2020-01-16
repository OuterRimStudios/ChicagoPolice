using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackstorySelector : MonoBehaviour
{
    public PerspectiveScene perspectiveScene;
    public string videoPath;
    public float sphereRotation;

    public void SelectBackground()
    {
        Debug.LogError($"{transform.name} Backstory selected");
        SundanceSceneTransition.Instance.NextScene();
        perspectiveScene.Play(videoPath, sphereRotation);
        gameObject.SetActive(false);
    }
}