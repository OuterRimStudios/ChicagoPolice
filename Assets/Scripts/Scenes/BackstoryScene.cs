using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackstoryScene : BaseScene
{
    public PerspectiveScene perspectiveScene;

    private void OnButtonDown(OVRInput.Button key)
    {
        //if (key == OVRInput.Button.Three)
        //    SundanceSceneTransition.Instance.PreviousScene();
    }

    public override void StartScene()
    {
        StartCoroutine(PlayVideo());
    }

    public override void EndScene()
    {
        OVRInputManager.OnButtonDown -= OnButtonDown;
        perspectiveScene.videoSphere.GetComponent<Renderer>().material.SetTexture("_MainTex", null);
        perspectiveScene.mediaPlayer.CloseVideo();
        perspectiveScene.videoSphere.SetActive(false);
    }

    IEnumerator PlayVideo()
    {
        yield return new WaitUntil(() => perspectiveScene.mediaPlayer.Control.IsPlaying());
        OVRInputManager.OnButtonDown += OnButtonDown;
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => !perspectiveScene.mediaPlayer.Control.IsPlaying());
        SundanceSceneTransition.Instance.PreviousScene();
    }
}