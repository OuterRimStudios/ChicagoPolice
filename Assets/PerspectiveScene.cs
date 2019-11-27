using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using OuterRimStudios.Utilities;
public class PerspectiveScene : BaseScene
{
    public MediaPlayer[] mediaPlayers;
    public GameObject[] videoSpheres;

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
        Debug.Log("Getting input from button " + button);
        if (button == OVRInput.Button.One)
            NextPerspective();
        else
            PreviousPerspective();
    }

    private void Update()
    {
        bool isDone = false;
        foreach(MediaPlayer mediaPlayer in mediaPlayers)
        {
            if (mediaPlayer.Control.IsPlaying())
            {
                isDone = false;
                break;
            }
            else
                isDone = true;
        }

        if (isDone)
            SundanceSceneTransition.Instance.NextScene();
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
        mediaPlayers[perspectiveIndex].Control.SeekFast(time);
        videoSpheres[perspectiveIndex].SetActive(true);
        mediaPlayers[perspectiveIndex].Play();
    }

    void Stop()
    {
        time = mediaPlayers[perspectiveIndex].Control.GetCurrentTimeMs();
        videoSpheres[perspectiveIndex].SetActive(false);
        mediaPlayers[perspectiveIndex].Pause();
    }

}
