using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoClip clip1;
    public VideoClip clip2;

    public VideoPlayer player;

    long frame;

    void Update()
    {
        frame = player.frame;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.clip = player.clip == clip1 ? clip2 : clip1;
            player.frame = frame;
        }
    }
}
