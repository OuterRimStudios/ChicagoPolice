using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class BackstorySelector : MonoBehaviour
{
    public BackstoryScene backstoryScene;

    public void SelectBackground()
    {
        Debug.LogError($"{transform.name} Backstory selected");
        gameObject.SetActive(false);
        SundanceSceneTransition.Instance.ChangeScene(backstoryScene);
    }
}