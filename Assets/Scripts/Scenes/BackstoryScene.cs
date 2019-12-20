using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using Hear360;

public class BackstoryScene : BaseScene
{
    public MediaPlayer mediaPlayer;
    public string videoPath;
    public GameObject videoSphere;
    public EightBallAudioController eightBallAudioController;
    public float audioDelay;
    public GameObjectScene backstoryHub;

    private void OnButtonDown(OVRInput.Button key)
    {
        if (key == OVRInput.Button.Three)
            SundanceSceneTransition.Instance.ChangeScene(backstoryHub);
    }

    public override void StartScene()
    {
        mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, Path.Combine(Application.persistentDataPath, videoPath), true);
        videoSphere.SetActive(true);
        StartCoroutine(PlayVideo());
    }

    public override void EndScene()
    {
        videoSphere.SetActive(false);
        OVRInputManager.OnButtonDown -= OnButtonDown;
        mediaPlayer.Pause();
        eightBallAudioController.Stop();        
        mediaPlayer.CloseVideo();
    }

    IEnumerator PlayVideo()
    {
        yield return new WaitUntil(() => mediaPlayer.Control.IsPlaying());
        OVRInputManager.OnButtonDown += OnButtonDown;
        yield return new WaitForSeconds(audioDelay);
        eightBallAudioController.Play();
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => !mediaPlayer.Control.IsPlaying());
        SundanceSceneTransition.Instance.ChangeScene(backstoryHub);
    }
}