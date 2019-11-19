using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

public class MoodTracking : MonoBehaviour
{
    public Color neutralColor;
    public Color tenseColor;
    public Color relaxedColor;

    public Slider moodSlider;
    public Image spotImage;
    public RectTransform middleBackground;
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
        SetUI();
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

        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x > USER_DEAD_ZONE && canUpdateSlider || Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(InputCooldown());

            if (moodIndex < moodSlider.maxValue)
                moodIndex++;

            UpdateUI();
            CreateMoodLine();
        }
        else if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x < -USER_DEAD_ZONE && canUpdateSlider || Input.GetKeyDown(KeyCode.LeftArrow))
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

    void SetUI()
    {        
        float sliderWidth = moodSlider.GetComponent<RectTransform>().sizeDelta.x;
        float incrementValue = sliderWidth / (moodSlider.maxValue - moodSlider.minValue);

        moodSlider.handleRect.sizeDelta = new Vector2(incrementValue, 0);

        float startingValue = -(sliderWidth / 2);
        for (int i = 0; i < moodSlider.maxValue; i++)
        {
            var spawnedRectTransform = Instantiate(spotImage, middleBackground.transform).GetComponent<RectTransform>();
            spawnedRectTransform.localPosition = new Vector2(startingValue, spotImage.GetComponent<RectTransform>().localPosition.y);
            spawnedRectTransform.sizeDelta = new Vector2(incrementValue, moodSlider.handleRect.rect.height);
            startingValue += incrementValue;
        }


    }

    void UpdateUI()
    {        
        moodSlider.value = moodIndex;

        if (moodIndex > 6)
        {
            moodSlider.handleRect.GetComponent<Image>().color = tenseColor;
        }
        else if (moodIndex == 6)
        {
            moodSlider.handleRect.GetComponent<Image>().color = neutralColor;
        }
        else
        {
            moodSlider.handleRect.GetComponent<Image>().color = relaxedColor;
        }

        /*
        float handleX = moodSlider.handleRect.localPosition.x;
        middleBackground.sizeDelta = new Vector2(Mathf.Abs(handleX), middleBackground.sizeDelta.y);
        middleBackground.localPosition = new Vector2(handleX / 2, middleBackground.localPosition.y);*/           
    }
}

internal class MoodInfo{
    public double time;
    public byte mood;

    public MoodInfo(double time, byte mood) { this.time = time; this.mood = mood; }
}
