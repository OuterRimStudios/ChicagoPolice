using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class BackstorySelector : MonoBehaviour
{
    public MediaPlayer mediaPlayer;
    public string videoPath;
    public GameObject videoSphere;
    public GameObject backstoryHub;

    public void SelectBackground()
    {
        Debug.LogError($"{transform.name} Backstory selected");
        mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, Path.Combine(Application.persistentDataPath, videoPath), true);
        videoSphere.SetActive(true);
        backstoryHub.SetActive(false);
    }
}