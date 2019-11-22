using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using OuterRimStudios.Utilities;
using RenderHeads.Media.AVProVideo;

public class MoodTracking : MonoBehaviour
{
    MediaPlayer currentMediaPlayer;

    List<MoodInfo> moodInfos = new List<MoodInfo>();

    string userID = "0";
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
            moodInfos.Clear();
            moodInfos.Add(new MoodInfo(userID, videoId));
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
            double time = currentMediaPlayer.Control.GetCurrentTimeMs() / 1000;
            float mood = slider?.value ?? -1;
            moodInfos.Add(new MoodInfo(userID, videoId, time, mood));

            Analytics.CustomEvent("MoodTracking", new Dictionary<string, object>
            {
                { "UserID", userID },
                { "VideoID", videoId },
                { "Time", time },
                { "Mood", mood }
            });
        }        
    }    
}

internal class MoodInfo
{
    public string UserID { get; set; }
    public int VideoID { get; set; }
    public double Time { get; set; }
    public float Mood { get; set; }

    readonly int neutralValue = 6;

    public MoodInfo(string userId, int videoId, double time, float mood) { UserID = userId; VideoID = videoId; Time = time; Mood = mood; }

    public MoodInfo(string userId, int videoId)
    {
        UserID = userId;
        VideoID = videoId;
        Time = 0;
        Mood = neutralValue;
    }
}
