using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OuterRimStudios.Utilities;
using RenderHeads.Media.AVProVideo;

public class MoodTracking : MonoBehaviour
{
    MediaPlayer currentMediaPlayer;

    List<MoodInfo> moodInfos = new List<MoodInfo>();

    int videoId;

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
            VideoScene videoScene = ((VideoScene)baseScene);
            currentMediaPlayer = videoScene.mediaPlayer;
            videoId = videoScene.videoID;
        }
        else
        {
            currentMediaPlayer = null;
            videoId = -1;
        }
    }

    void SendMoodAnalytics(BaseScene baseScene)
    {
        if (baseScene.GetType() == typeof(VideoScene))
        {
            var data = moodInfos;
            AnalyticsUtilities.Event("MoodTracking", data);
        }
    }

    public void CreateMoodLine(Slider slider)
    {
        if (currentMediaPlayer != null)
        {
            moodInfos.Add(new MoodInfo(0, videoId, currentMediaPlayer.Control.GetCurrentTimeMs() / 1000, slider?.value ?? -1));
        }        
    }    
}

internal class MoodInfo
{
    public int UserID;
    public int VideoID;
    public double Time;
    public float Mood;

    public MoodInfo(int userId, int videoId, double time, float mood) { UserID = userId; VideoID = videoId; Time = time; Mood = mood; }
}
