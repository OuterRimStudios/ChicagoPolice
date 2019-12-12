using RenderHeads.Media.AVProVideo;
using UnityEngine;
using System.Collections;
using Hear360;

public class VideoScene : BaseScene
{
    public MediaPlayer mediaPlayer;
    public GameObject videoSphere;
    public AudioSource headlockedSource;
    public GameObject moodSlider;
    public bool trackingEnabled;
    public int videoID;
    public float audioDelay;

    public EightBallAudioController eightBallAudioController;

    public override void StartScene()
    {
        videoSphere.SetActive(true);
        moodSlider.SetActive(true);
        mediaPlayer.Play();

        //eightBallAudioController.Play();

        StartCoroutine(IsPlaying());
    }

    IEnumerator IsPlaying()
    {
        yield return new WaitForSeconds(audioDelay);
        eightBallAudioController.Play();
        headlockedSource?.Play();
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => !mediaPlayer.Control.IsPlaying());
        ChicagoSceneTransition.Instance.NextScene();
    }

    public override void EndScene()
    {
        mediaPlayer.Pause();
        videoSphere.SetActive(false);
        moodSlider.SetActive(false);
    }
}
