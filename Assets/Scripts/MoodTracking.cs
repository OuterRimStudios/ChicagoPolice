using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

public class MoodTracking : MonoBehaviour
{
    public Slider moodSlider;
    MediaPlayer currentMediaPlayer;

    List<MoodInfo> moodInfos = new List<MoodInfo>();

    [SerializeField]
    double USER_DEAD_ZONE = .2;

    byte moodIndex = 6;
    bool canUpdateSlider = true;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    void OnEnable()
    {
        ChicagoSceneTransition.OnSceneStarted += SetMediaType;
    }

    void OnDisable()
    {
        ChicagoSceneTransition.OnSceneStarted -= SetMediaType;
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();

        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x > USER_DEAD_ZONE && canUpdateSlider)
        {
            StartCoroutine(InputCooldown());

            if (moodIndex < moodSlider.maxValue)
                moodIndex++;

            UpdateUI();
            CreateMoodLine();
        }
        else if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x < -USER_DEAD_ZONE && canUpdateSlider)
        {
            StartCoroutine(InputCooldown());

            if (moodIndex > moodSlider.minValue)
                moodIndex--;

            UpdateUI();
            CreateMoodLine();
        }
    }

    IEnumerator InputCooldown()
    {
        canUpdateSlider = false;
        yield return new WaitForSeconds(.25f);
        canUpdateSlider = true;
    }

    void SetMediaType(BaseScene baseScene)
    {
        if (baseScene.GetType() == typeof(VideoScene))
        {
            currentMediaPlayer = ((VideoScene)baseScene).mediaPlayer;
        }
    }

    void CreateMoodLine()
    {
        moodInfos.Add(new MoodInfo(currentMediaPlayer?.Control.GetCurrentTimeMs() * 1000 ?? 10, moodIndex));
    }

    void UpdateUI()
    {
        moodSlider.value = moodIndex;
    }
}

internal class MoodInfo{
    public double time;
    public byte mood;

    public MoodInfo(double time, byte mood) { this.time = time; this.mood = mood; }
}
