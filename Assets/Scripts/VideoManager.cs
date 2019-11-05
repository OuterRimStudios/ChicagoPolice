using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class VideoManager : MonoBehaviour
{
    public MediaPlayer playerOne;
    public MeshRenderer sphereOne;
    public MediaPlayer playerTwo;
    public MeshRenderer sphereTwo;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(30f);
        float time = playerOne.Control.GetCurrentTimeMs();
        playerOne.Control.Pause();
        sphereOne.enabled = false;
        playerTwo.Control.Seek(time);
        sphereTwo.enabled = true;
        playerTwo.Control.Play();
    }

    //void Update()
    //{
    //    if (switchPerspective > 0)
    //    {
    //            if (playerOne.Control.IsPlaying())
    //            {
    //                float time = playerOne.Control.GetCurrentTimeMs();
    //                playerOne.Control.Pause();
    //                sphereOne.enabled = false;
    //                playerTwo.Control.Seek(time);
    //                sphereTwo.enabled = true;
    //                playerTwo.Control.Play();
    //            }
    //            else
    //            {
    //                float time = playerTwo.Control.GetCurrentTimeMs();
    //                playerTwo.Control.Pause();
    //                sphereTwo.enabled = false;
    //                playerOne.Control.Seek(time);
    //                sphereOne.enabled = true;
    //                playerOne.Control.Play();
    //            }
    //    }
    //}
}