using RenderHeads.Media.AVProVideo;
using UnityEngine;
public class VideoScene : BaseScene
{
    public MediaPlayer mediaPlayer;
    public GameObject videoSphere;
    public override void StartScene()
    {
        Debug.LogError("Start Scene: " + name);
        videoSphere.SetActive(true);
        mediaPlayer.Play();
    }

    public override void EndScene()
    {
        Debug.LogError("End Scene Scene: " + name);
        mediaPlayer.Pause();
        videoSphere.SetActive(false);
    }
}
