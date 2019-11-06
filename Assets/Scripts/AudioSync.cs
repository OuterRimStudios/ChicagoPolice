using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class AudioSync : MonoBehaviour
{
    public float delay;
    public AudioSource source1;
    public AudioSource source2;

    public MediaPlayer player1;
    public MediaPlayer player2;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => (player1.Control.IsPlaying() || player2.Control.IsPlaying()));
        source1.Play();
        source2.Play();
    }
}
