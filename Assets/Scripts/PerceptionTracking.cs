using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using RenderHeads.Media.AVProVideo;
using OuterRimStudios.Utilities;
public class PerceptionTracking : MonoBehaviour
{
    const string ANALYTICS_TITLE = "PerceptionTracking";
    public float trackingInterval = .1f;
    public LayerMask trackingLayer;

    bool initialized;
    bool casting;
    string lastTag;
    MediaPlayer mediaPlayer;
    VideoScene videoScene;

    string userID;
    int headsetID;
    string testTimestamp;
    List<PerceptionInfo> perceptionData = new List<PerceptionInfo>();

    void OnEnable()
    {
        ChicagoSceneTransition.OnSceneStarted += Initialize;
        ChicagoSceneTransition.OnSceneEnded += OnSceneEnded;
    }

    void OnDisable()
    {
        ChicagoSceneTransition.OnSceneStarted -= Initialize;
        ChicagoSceneTransition.OnSceneEnded -= OnSceneEnded;
    }

    void Initialize(BaseScene baseScene)
    {
        perceptionData.Clear();
        userID = ChicagoSceneTransition.Instance.UserID;
        headsetID = ChicagoSceneTransition.Instance.HeadsetID;
        testTimestamp = ChicagoSceneTransition.Instance.TestTimestamp;

        if (baseScene.GetType() == typeof(VideoScene))
        {
            videoScene = ((VideoScene)baseScene);

            if (videoScene.trackingEnabled)
            {
                mediaPlayer = videoScene.mediaPlayer;
                initialized = true;
            }
            else if (initialized)
                initialized = false;
        }
        else if (initialized)
            initialized = false;

    }

    private void OnSceneEnded(BaseScene baseScene)
    {
        SendAnalytics();
    }

    private void Update()
    {
        if (!initialized) return;
        if(!casting)
        {
            casting = true;
            StartCoroutine(Cast());
        }
    }

    IEnumerator Cast()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, trackingLayer))
        {
            if(hit.transform.tag != lastTag)
            {
                lastTag = hit.transform.tag;
                float time = mediaPlayer.Control.GetCurrentTimeMs() / 1000;
                PerceptionInfo perceptionInfo = new PerceptionInfo(userID, headsetID, testTimestamp, videoScene.videoID, time.ToString("N1"), lastTag);
                perceptionData.Add(perceptionInfo);

                Analytics.CustomEvent(ANALYTICS_TITLE, new Dictionary<string, object>{
                    { "UserID", perceptionInfo.UserID},
                    { "HeadsetID", perceptionInfo.HeadsetID},
                    { "TestTimestamp", perceptionInfo.TestTimestamp},
                    { "VideoID", perceptionInfo.VideoID},
                    { "Time", perceptionInfo.Time},
                    { "Location", perceptionInfo.Location}
                });
            }
        }

        yield return new WaitForSeconds(trackingInterval);
        casting = false;
    }

    void SendAnalytics()
    {
        AnalyticsUtilities.Event(ANALYTICS_TITLE, perceptionData);
    }
}

public class PerceptionInfo
{
    public string UserID { get; set; }
    public int HeadsetID { get; set; }
    public string TestTimestamp { get; set; }
    public int VideoID { get; set; }
    public string Time { get; set; }
    public string Location { get; set; }

    public PerceptionInfo(string userID, int headsetID, string testTimestamp, int videoID, string time, string location)
    {
        UserID = userID;
        HeadsetID = headsetID;
        TestTimestamp = testTimestamp;
        VideoID = videoID;
        Time = time;
        Location = location;
    }
}