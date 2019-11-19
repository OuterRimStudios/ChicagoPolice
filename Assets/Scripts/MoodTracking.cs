using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

public class MoodTracking : MonoBehaviour
{
    MediaPlayer currentMediaPlayer;

    List<MoodInfo> moodInfos = new List<MoodInfo>();   

    void OnEnable()
    {
        ChicagoSceneTransition.OnSceneStarted += SetMediaType;
        ChicagoSceneTransition.OnSceneEnded += SendMoodAnalytics;
    }

    void OnDisable()
    {
        ChicagoSceneTransition.OnSceneStarted -= SetMediaType;
        ChicagoSceneTransition.OnSceneEnded -= SendMoodAnalytics;
    }    

    void SetMediaType(BaseScene baseScene)
    {
        if (baseScene.GetType() == typeof(VideoScene))
        {
            currentMediaPlayer = ((VideoScene)baseScene).mediaPlayer;
        }
        else
        {
            currentMediaPlayer = null;
        }
    }

    void SendMoodAnalytics(BaseScene baseScene)
    {
        if (baseScene.GetType() == typeof(VideoScene))
        {
            //send the moodInfos over to wherever we're storing them
        }
    }

    public void CreateMoodLine(Slider slider)
    {
        if (currentMediaPlayer != null)
        {
            moodInfos.Add(new MoodInfo(currentMediaPlayer.Control.GetCurrentTimeMs() / 1000, slider?.value ?? -1));
        }        
    }    
}

internal class MoodInfo
{
    public double time;
    public float mood;

    public MoodInfo(double time, float mood) { this.time = time; this.mood = mood; }
}
