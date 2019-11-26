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

    int userID;
    int headsetID;
    string testTimestamp;

    void OnEnable()
    {
        ChicagoSceneTransition.OnSceneStarted += Initialize;
        
    }

    void OnDisable()
    {
        ChicagoSceneTransition.OnSceneStarted -= Initialize;
    }

    void Initialize(BaseScene baseScene)
    {
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
                PerceptionInfo perceptionInfo = new PerceptionInfo(userID, headsetID, testTimestamp, videoScene.videoID, time, lastTag);
                var data = new List<PerceptionInfo> { perceptionInfo };

                AnalyticsUtilities.Event(ANALYTICS_TITLE, data);

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
}

public class PerceptionInfo
{
    public int UserID { get; set; }
    public int HeadsetID { get; set; }
    public string TestTimestamp { get; set; }
    public int VideoID { get; set; }
    public double Time { get; set; }
    public string Location { get; set; }

    public PerceptionInfo(int userID, int headsetID, string testTimestamp, int videoID, double time, string location)
    {
        UserID = userID;
        HeadsetID = headsetID;
        TestTimestamp = testTimestamp;
        VideoID = videoID;
        Time = time;
        Location = location;
    }
}