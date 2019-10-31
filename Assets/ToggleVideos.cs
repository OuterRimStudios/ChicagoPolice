using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class ToggleVideos : MonoBehaviour
{
    public PlayableDirector directorOne;
    public PlayableDirector directorTwo;
    double time;

    private void Start()
    {
        directorTwo.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(directorOne.gameObject.activeInHierarchy)
            {
                time = directorOne.time;
                directorTwo.time = time;
                directorOne.gameObject.SetActive(false);
                directorTwo.gameObject.SetActive(true);
                directorTwo.Play();
            }
            else
            {
                time = directorTwo.time;
                directorOne.time = time;
                directorTwo.gameObject.SetActive(false);
                directorOne.gameObject.SetActive(true);
                directorOne.Play();
            }
        }
    }
}
