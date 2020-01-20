using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using OuterRimStudios.Utilities;
public class PerspectiveScene : BaseScene
{
    public MediaPlayer mediaPlayer;
    public GameObject videoSphere;
    public string[] videoPaths;
    public float[] videoRotations;
    //public GameObject moodSlider;
    public Animator fadeAnimator;
    public float delay = 1.5f;
    public CountdownManager countdown;

    public bool isTesting;

    int perspectiveIndex;
    float time;
    bool delayed;

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
        if (button != OVRInput.Button.Three && button != OVRInput.Button.Four)
            return;

        if (!fadeAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fade") && mediaPlayer.Control.IsPlaying() && !delayed)
        {
            delayed = true;
            StartCoroutine(Delay(button));
        }
    }

    private void Update()
    {
        if(mediaPlayer.Control.IsFinished())
            SundanceSceneTransition.Instance.NextScene();
    }

    public override void StartScene()
    {
        countdown.StartCountdown();
        CountdownManager.OnCountdownFinished += PlayVideo;
        //gameObject.SetActive(true);
        //moodSlider.SetActive(true);
        //Play(videoPaths[perspectiveIndex], videoRotations[perspectiveIndex]);
    }

    void PlayVideo()
    {
        gameObject.SetActive(true);
        //moodSlider.SetActive(true);
        Play(videoPaths[perspectiveIndex], videoRotations[perspectiveIndex]);
        CountdownManager.OnCountdownFinished -= PlayVideo;

        if (isTesting)
            SundanceSceneTransition.Instance.NextScene();
    }

    public override void EndScene()
    {
        Stop();
        time = 0;
        gameObject.SetActive(false);
        videoSphere.SetActive(false);
        //moodSlider.SetActive(false);
    }

    void NextPerspective()
    {
        Stop();
        perspectiveIndex = perspectiveIndex.IncrementLoop(videoPaths.Length - 1);
        Play(videoPaths[perspectiveIndex], time, videoRotations[perspectiveIndex]);
    }

    void PreviousPerspective()
    {
        Stop();
        perspectiveIndex = perspectiveIndex.DecrementLoop(0, videoPaths.Length - 1);
        Play(videoPaths[perspectiveIndex], time, videoRotations[perspectiveIndex]);
    }


    public void Play(string videoPath, float seekTime, float yOffset)
    {
        mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, Path.Combine(Application.persistentDataPath, videoPath), false);

        if (seekTime != 0)
            mediaPlayer.Control.SeekFast(seekTime);

        videoSphere.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yOffset, transform.rotation.eulerAngles.z);
        videoSphere.SetActive(true);
        mediaPlayer.Play();
    }

    public void Play(string videoPath, float yOffset)
    {
        Play(videoPath, 0, yOffset);
    }

    public void Play(string videoPath)
    {
        Play(videoPath, 0, videoSphere.transform.rotation.eulerAngles.y);
    }


    void Stop()
    {
        time = mediaPlayer.Control.GetCurrentTimeMs();

        videoSphere.SetActive(false);
        mediaPlayer.CloseVideo();
    }

    IEnumerator Delay(OVRInput.Button button)
    {
        fadeAnimator.SetTrigger("Fade");

        yield return new WaitForSeconds(.35f);
        if (button == OVRInput.Button.Three)
            NextPerspective();
        else if (button == OVRInput.Button.Four)
            PreviousPerspective();

        yield return new WaitForSeconds(delay);
        delayed = false;
    }
}
