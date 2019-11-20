using RenderHeads.Media.AVProVideo;
using UnityEngine;
using System.Collections;
public class VideoScene : BaseScene
{
    public MediaPlayer mediaPlayer;
    public GameObject videoSphere;
    public GameObject moodSlider;
    public bool trackingEnabled;
    public int videoID;

    public override void StartScene()
    {
        videoSphere.SetActive(true);
        moodSlider.SetActive(true);
        mediaPlayer.Play();
        StartCoroutine(IsPlaying());
    }

    IEnumerator IsPlaying()
    {
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
