using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
public class PerceptionTracking : MonoBehaviour
{
    public float trackingInterval = .1f;
    public LayerMask trackingLayer;

    bool initialized;
    bool casting;
    string lastTag;
    MediaPlayer mediaPlayer;
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
            VideoScene videoScene = ((VideoScene)baseScene);

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
            }
        }

        yield return new WaitForSeconds(trackingInterval);
        casting = false;
    }
}
