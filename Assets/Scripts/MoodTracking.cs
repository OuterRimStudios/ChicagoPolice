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

    string userID;
    int headsetID;
    string testTimestamp;
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
        userID = ChicagoSceneTransition.Instance.UserID;
        headsetID = ChicagoSceneTransition.Instance.HeadsetID;
        testTimestamp = ChicagoSceneTransition.Instance.TestTimestamp;

        if (baseScene.GetType() == typeof(VideoScene))
        {
            VideoScene videoScene = ((VideoScene)baseScene);            
            currentMediaPlayer = videoScene.mediaPlayer;
            videoId = videoScene.videoID;
            moodInfos.Clear();
            moodInfos.Add(new MoodInfo(userID, headsetID, testTimestamp, videoId));
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
            MoodInfo moodInfo = new MoodInfo(userID, headsetID, testTimestamp, videoId, time, mood);
            moodInfos.Add(moodInfo);

            Analytics.CustomEvent("MoodTracking", new Dictionary<string, object>
            {
                { "UserID", moodInfo.UserID },
                { "HeadsetID", moodInfo.HeadsetID},
                { "TestTimestamp", moodInfo.TestTimestamp},
                { "VideoID", moodInfo.VideoID },
                { "Time", moodInfo.Time.ToString("N1") },
                { "Mood", moodInfo.Mood }
            });
        }        
    }    
}

internal class MoodInfo
{
    public string UserID { get; set; }
    public int HeadsetID { get; set; }
    public string TestTimestamp { get; set; }
    public int VideoID { get; set; }
    public double Time { get; set; }
    public float Mood { get; set; }

    readonly int neutralValue = 6;

    public MoodInfo(string userID, int headsetID, string testTimestamp, int videoId)
    {
        UserID = userID;
        HeadsetID = headsetID;
        TestTimestamp = testTimestamp;
        VideoID = videoId;
        Time = 0;
        Mood = neutralValue;
    }

    public MoodInfo(string userID, int headsetID, string testTimestamp, int videoId, double time, float mood)
    {
        UserID = userID;
        HeadsetID = headsetID;
        TestTimestamp = testTimestamp;
        VideoID = videoId; 
        Time = time;
        Mood = mood;
    }
}