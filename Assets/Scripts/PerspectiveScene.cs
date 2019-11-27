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
        //Debug.LogError("Getting input from button " + button);
        if (button == OVRInput.Button.One)
            NextPerspective();
        else
            PreviousPerspective();
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
        //Debug.LogError("Starting Scene.");
        gameObject.SetActive(true);
        Play();
    }

    public override void EndScene()
    {
        //Debug.LogError("Ending Scene.");
        Stop();
        time = 0;
        gameObject.SetActive(false);
    }

    void NextPerspective()
    {
        //Debug.LogError("Next Perspective");
        Stop();
        perspectiveIndex = perspectiveIndex.IncrementLoop(mediaPlayers.Length - 1);
        Play();
    }

    void PreviousPerspective()
    {
        //Debug.LogError("Previous Perspective");
        Stop();
        perspectiveIndex = perspectiveIndex.DecrementLoop(0, mediaPlayers.Length - 1);
        Play();
    }

    void Play()
    {
        //Debug.LogError("Playing: " + mediaPlayers[perspectiveIndex].transform.parent.name);
        mediaPlayers[perspectiveIndex].OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, mediaPlayers[perspectiveIndex].m_VideoPath, false);

        //Debug.LogError(mediaPlayers[perspectiveIndex].transform.parent.name + "is seeking to: " + time);
        mediaPlayers[perspectiveIndex].Control.SeekFast(time);

        //Debug.LogError(mediaPlayers[perspectiveIndex].transform.parent.name + "'s current time: " + time);
        videoSpheres[perspectiveIndex].SetActive(true);
        mediaPlayers[perspectiveIndex].Play();
    }

    void Stop()
    {
        //Debug.LogError("Stopping: " + mediaPlayers[perspectiveIndex].transform.parent.name);
        time = mediaPlayers[perspectiveIndex].Control.GetCurrentTimeMs();

        //Debug.LogError(mediaPlayers[perspectiveIndex].transform.parent.name + "'s current time: " + time);
        videoSpheres[perspectiveIndex].SetActive(false);
        mediaPlayers[perspectiveIndex].CloseVideo();
    }
}
