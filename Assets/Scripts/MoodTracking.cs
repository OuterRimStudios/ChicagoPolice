using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class MoodTracking : MonoBehaviour
{
    byte moodIndex = 6;
    MediaPlayer currentMediaPlayer;
    const double USER_DEAD_ZONE = .2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        //ChicagoSceneTransition.OnSceneStarted += SetMediaType;
    }

    void OnDisable()
    {
        //ChicagoSceneTransition.OnSceneStarted -= SetMediaType;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x > USER_DEAD_ZONE)
        {
            Debug.Log("Moving Right");
            float secondsSinceStart = currentMediaPlayer.Control.GetCurrentTimeMs() * 1000;
            moodIndex++;

        }
        else if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x < -USER_DEAD_ZONE)
        {
            Debug.Log("Moving Left");
            moodIndex--;
        }
    }

    void SetMediaType(/*BaseScene baseScene*/)
    {
        /*if (baseScene.GetType() == typeof(VideoScene))
        {
            currentMediaPlayer = baseScene.GetMediaPlayer();*/
        
        //}
    }

    

    //Movement for the Slider
    //UI for the the Slider
    //Time sense 
}
