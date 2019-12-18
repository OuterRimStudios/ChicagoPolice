//hector sux
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using RenderHeads.Media.AVProVideo;
using OuterRimStudios.Utilities;
using Hear360;
using System.IO;

public class PerspectiveScene : BaseScene
{
    public VideoInfo[] videoInfos;
    public Animator fadeAnimator;

    Coroutine switchPerspective;
    Coroutine play;
    int perspectiveIndex;
    bool sceneStarted;

    private void OnEnable()
    {
        OVRInputManager.OnButtonDown += OnButtonDown;
    }

    private void OnDisable()
    {
        OVRInputManager.OnButtonDown -= OnButtonDown;
    }

    private void Start()
    {
        foreach(VideoInfo videoInfo in videoInfos)
            videoInfo.sources = videoInfo.audioController.GetComponents<AudioSource>();
    }

    void OnButtonDown(OVRInput.Button button)
    {
        if (sceneStarted && !fadeAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fade"))
        {
            if (switchPerspective == null)
            {
                if (button == OVRInput.Button.One || button == OVRInput.Button.Two)
                    switchPerspective = StartCoroutine(SwitchPerspective(button == OVRInput.Button.One));
            }            
        }
    }

    private void Update()
    {
        if(videoInfos.Any(x => x.mediaPlayer.Control.IsFinished()))
            SundanceSceneTransition.Instance.NextScene();
    }

    public override void StartScene()
    {
        sceneStarted = true;
        //Debug.LogError("Starting Scene.");
        gameObject.SetActive(true);
        Play(0);
    }

    public override void EndScene()
    {
        sceneStarted = false;
        //Debug.LogError("Ending Scene.");
        Stop();
        gameObject.SetActive(false);
    }

    IEnumerator SwitchPerspective(bool isNext)
    {
        fadeAnimator.SetTrigger("Fade");
        
        yield return new WaitForSeconds(1f);

        if (isNext)
            NextPerspective();
        else
            PreviousPerspective();

        switchPerspective = null;
    }

    void NextPerspective()
    {
        //Debug.LogError("Next Perspective");
        if (play != null)
            StopCoroutine(play);

        float videoTime = Stop();
        perspectiveIndex = perspectiveIndex.IncrementLoop(videoInfos.Length - 1);
        play = StartCoroutine(Play(videoTime));
    }

    void PreviousPerspective()
    {
        //Debug.LogError("Previous Perspective");
        if (play != null)
            StopCoroutine(play);

        float videoTime = Stop();
        perspectiveIndex = perspectiveIndex.DecrementLoop(0, videoInfos.Length - 1);
        play = StartCoroutine(Play(videoTime));
    }

    IEnumerator Play(float currentTime)
    {
        var videoInfo = videoInfos[perspectiveIndex];
        //Debug.LogError("Playing: " + mediaPlayers[perspectiveIndex].transform.parent.name);
        videoInfo.mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, Path.Combine(Application.persistentDataPath, videoInfo.videoPath), false);

        //Debug.LogError(mediaPlayers[perspectiveIndex].transform.parent.name + "is seeking to: " + time);
        videoInfo.mediaPlayer.Control.SeekFast(currentTime);

        //Debug.LogError(mediaPlayers[perspectiveIndex].transform.parent.name + "'s current time: " + time);
        videoInfo.videoSphere.SetActive(true);
        videoInfo.mediaPlayer.Play();

        yield return new WaitForSeconds(videoInfo.audioDelay);

        foreach(AudioSource source in videoInfo.sources)
            source.time = (currentTime / 1000);

        videoInfo.audioController.Play();
        videoInfo.headlockedSource.Play();
    }

    float Stop()
    {
        var videoInfo = videoInfos[perspectiveIndex];

        //Debug.LogError("Stopping: " + mediaPlayers[perspectiveIndex].transform.parent.name);
        float videoTime = videoInfo.sources[0].time;

        videoInfo.audioController.Stop();
        videoInfo.headlockedSource.Stop();

        //videoInfo.mediaPlayer.Control.GetCurrentTimeMs();

        //Debug.LogError(mediaPlayers[perspectiveIndex].transform.parent.name + "'s current time: " + time);
        videoInfo.videoSphere.SetActive(false);
        videoInfo.mediaPlayer.CloseVideo();

        return videoTime;
    }
}

[System.Serializable]
public class VideoInfo
{
    [Header("Video")]
    public MediaPlayer mediaPlayer;
    public GameObject videoSphere;
    public string videoPath;

    [Space, Header("Audio")]
    public EightBallAudioController audioController;
    public AudioSource headlockedSource;
    public float audioDelay;

    public AudioSource[] sources;
}
