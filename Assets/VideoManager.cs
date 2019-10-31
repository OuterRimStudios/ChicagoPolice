using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public GameObject clip1;
    public GameObject clip2;

    public VideoPlayer player;
    public VideoPlayer player2;

    long frame;
    long frame2;

    bool toggle;

    private void Start()
    {
        player.Play();
        player2.Prepare();
    }

    void Update()
    {
        frame = player.frame;
        frame2 = player2.frame;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            toggle = !toggle;

            clip1.SetActive(!clip1.activeInHierarchy);
            clip2.SetActive(!clip2.activeInHierarchy);

            if(clip1.activeInHierarchy)
                player.frame = frame2;
            else
                player2.frame = frame;
        }
    }
}
