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
    }

    void OnDisable()
    {
        ChicagoSceneTransition.OnSceneStarted -= SetMediaType;
    }    

    void SetMediaType(BaseScene baseScene)
    {
        if (baseScene.GetType() == typeof(VideoScene))
        {
            currentMediaPlayer = ((VideoScene)baseScene).mediaPlayer;
        }
    }

    public void CreateMoodLine(Slider sliderValue)
    {
        moodInfos.Add(new MoodInfo(currentMediaPlayer?.Control.GetCurrentTimeMs() * 1000 ?? -1, sliderValue?.value ?? -1));
    }
}

internal class MoodInfo
{
    public double time;
    public float mood;

    public MoodInfo(double time, float mood) { this.time = time; this.mood = mood; }
}
