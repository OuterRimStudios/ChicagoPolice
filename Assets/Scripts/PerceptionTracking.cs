using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using RenderHeads.Media.AVProVideo;
using OuterRimStudios.Utilities;
public class PerceptionTracking : MonoBehaviour
{
    public float trackingInterval = .1f;
    public LayerMask trackingLayer;

    bool initialized;
    bool casting;
    string lastTag;
    MediaPlayer mediaPlayer;
    VideoScene videoScene;
    string userID = "0";
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
                Debug.LogError(time + " " + lastTag);
                var data = new List<object> { new { UserID = userID, VideoID = videoScene.videoID, Time = time, Location = lastTag } };
                AnalyticsUtilities.Event("PerceptionTracking", data);

                Analytics.CustomEvent("PerceptionTracking", new Dictionary<string, object>{
                    { "UserID", userID},
                    { "VideoID", videoScene.videoID},
                    { "Time", time},
                    { "Location", lastTag}
                });
            }
        }

        yield return new WaitForSeconds(trackingInterval);
        casting = false;
    }
}
