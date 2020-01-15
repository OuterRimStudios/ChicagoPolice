using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class BackstoryScene : BaseScene
{
    public MediaPlayer mediaPlayer;
    //public string videoPath;
    public GameObject videoSphere;
    public GameObjectScene backstoryHub;

    private void OnButtonDown(OVRInput.Button key)
    {
        if (key == OVRInput.Button.Three)
            SundanceSceneTransition.Instance.ChangeScene(backstoryHub);
    }

    public override void StartScene()
    {
        //ediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, Path.Combine(Application.persistentDataPath, videoPath), true);
        videoSphere.SetActive(true);
        StartCoroutine(PlayVideo());
    }

    public override void EndScene()
    {
        videoSphere.SetActive(false);
        OVRInputManager.OnButtonDown -= OnButtonDown;
        mediaPlayer.Pause();
        mediaPlayer.CloseVideo();
    }

    IEnumerator PlayVideo()
    {
        yield return new WaitUntil(() => mediaPlayer.Control.IsPlaying());
        OVRInputManager.OnButtonDown += OnButtonDown;
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => !mediaPlayer.Control.IsPlaying());
        SundanceSceneTransition.Instance.ChangeScene(backstoryHub);
    }
}