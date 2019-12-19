﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using OuterRimStudios.Utilities;
public class PerspectiveScene : BaseScene
{
    public MediaPlayer[] mediaPlayers;
    public GameObject[] videoSpheres;
    public Animator fadeAnimator;

    int perspectiveIndex;
    float time;

    private void OnEnable()
    {
        OVRInputManager.OnButtonDown += OnButtonDown;
    }

    private void OnDisable()
    {
        OVRInputManager.OnButtonDown -= OnButtonDown;
    }

    void OnButtonDown(OVRInput.Button button)
    {
        if(!fadeAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fade"))
        {
            fadeAnimator.SetTrigger("Fade");

            if (button == OVRInput.Button.One)
                NextPerspective();
            else
                PreviousPerspective();
        }
    }

    private void Update()
    {
        foreach (MediaPlayer mediaPlayer in mediaPlayers)
        {
            if(mediaPlayer.Control.IsFinished())
                SundanceSceneTransition.Instance.NextScene();
        }
    }

    public override void StartScene()
    {
        gameObject.SetActive(true);
        Play();
    }

    public override void EndScene()
    {
        Stop();
        time = 0;
        gameObject.SetActive(false);
    }

    void NextPerspective()
    {
        Stop();
        perspectiveIndex = perspectiveIndex.IncrementLoop(mediaPlayers.Length - 1);
        Play();
    }

    void PreviousPerspective()
    {
        Stop();
        perspectiveIndex = perspectiveIndex.DecrementLoop(0, mediaPlayers.Length - 1);
        Play();
    }

    void Play()
    {
        mediaPlayers[perspectiveIndex].OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, mediaPlayers[perspectiveIndex].m_VideoPath, false);

        mediaPlayers[perspectiveIndex].Control.SeekFast(time);

        videoSpheres[perspectiveIndex].SetActive(true);
        mediaPlayers[perspectiveIndex].Play();
    }

    void Stop()
    {
        time = mediaPlayers[perspectiveIndex].Control.GetCurrentTimeMs();

        videoSpheres[perspectiveIndex].SetActive(false);
        mediaPlayers[perspectiveIndex].CloseVideo();
    }
}
