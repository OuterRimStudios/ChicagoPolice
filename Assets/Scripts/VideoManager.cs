using System.IO;
using System.Collections;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class VideoManager : MonoBehaviour
{
    public MediaPlayer playerOne;
    public MeshRenderer sphereOne;
    public string path1;
    public MediaPlayer playerTwo;
    public MeshRenderer sphereTwo;
    public string path2;
    public MediaPlayer interview;
    public MeshRenderer interviewSphere;
    public string path3;

    bool acceptInput = true;
    bool allowSwitch = true;

    private void Awake()
    {
        playerOne.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, Path.Combine(Application.persistentDataPath, path1), true);
        playerTwo.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, Path.Combine(Application.persistentDataPath, path2), false);
        interview.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, Path.Combine(Application.persistentDataPath, path3), false);
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(60f);
        allowSwitch = false;
    }

    void Update()
    {
        if (playerOne.Control.IsFinished() || playerTwo.Control.IsFinished())
        {
            sphereOne.enabled = false;
            sphereTwo.enabled = false;
            interview.Control.Play();
            interviewSphere.enabled = true;
        }

        if (!allowSwitch)
            return;

        OVRInput.Update();

        if (acceptInput && OVRInput.Get(OVRInput.Button.One))
        {
            acceptInput = false;
            if (playerOne.Control.IsPlaying() && !playerTwo.Control.IsFinished())
            {
                float time = playerOne.Control.GetCurrentTimeMs();
                playerOne.Control.Pause();
                sphereOne.enabled = false;
                playerTwo.Control.SeekFast(time);
                playerTwo.Control.Play();
                sphereTwo.enabled = true;
            }
            else if (playerTwo.Control.IsPlaying() && !playerOne.Control.IsFinished())
            {
                float time = playerTwo.Control.GetCurrentTimeMs();
                playerTwo.Control.Pause();
                sphereTwo.enabled = false;
                playerOne.Control.SeekFast(time);
                playerOne.Control.Play();
                sphereOne.enabled = true;
            }

            StartCoroutine(ResetInput());
        }
    }

    IEnumerator ResetInput()
    {
        yield return new WaitForSeconds(0.25f);
        acceptInput = true;
    }
}