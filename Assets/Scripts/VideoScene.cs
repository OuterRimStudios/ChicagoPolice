using RenderHeads.Media.AVProVideo;
using UnityEngine;
using System.Collections;
public class VideoScene : BaseScene
{
    public MediaPlayer mediaPlayer;
    public GameObject videoSphere;
    public bool trackingEnabled;
    public override void StartScene()
    {
        Debug.LogError("Start Scene: " + name);
        videoSphere.SetActive(true);
        mediaPlayer.Play();
        StartCoroutine(IsPlaying());
    }

    IEnumerator IsPlaying()
    {
        if(OVRInput.GetDown(OVRInput.Button.Three))
        {
            ChicagoSceneTransition.Instance.NextScene();
            yield break;
        }

        yield return new WaitUntil(() => !mediaPlayer.Control.IsPlaying());
        ChicagoSceneTransition.Instance.NextScene();
    }

    public override void EndScene()
    {
        Debug.LogError("End Scene Scene: " + name);
        mediaPlayer.Pause();
        videoSphere.SetActive(false);
    }
}
