using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

public class BubbleSlider : MonoBehaviour
{
    [Header("UI Variables")]
    public Color neutralColor;
    public Color tenseColor;
    public Color relaxedColor;

    public Slider moodSlider;
    public Image nodeImage;
    public RectTransform sliderArea;

    [Tooltip("Setting how much smaller the slider nodes are compared to the handle")]
    public float sizeDif = 0f;
    public bool isHandleBehind = false;

    RectTransform handle;
    Image handleImage;

    [SerializeField, Header("Control Variables")]
    double userDeadZone = .2;
    [SerializeField]
    float controllerDelayTime = .25f;

    byte moodIndex = 6;
    bool canUpdateSlider = true;

    // Start is called before the first frame update
    void Start()
    {
        handle = moodSlider.handleRect;
        handleImage = handle.GetComponent<Image>();
        UpdateUI();
        SetUI();
    }    

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();

        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x > userDeadZone && canUpdateSlider || Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(InputCooldown());

            if (moodIndex < moodSlider.maxValue)
                moodIndex++;

            UpdateUI();
        }
        else if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x < -userDeadZone && canUpdateSlider || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(InputCooldown());

            if (moodIndex > moodSlider.minValue)
                moodIndex--;

            UpdateUI();
        }
    }

    IEnumerator InputCooldown()
    {
        canUpdateSlider = false;
        yield return new WaitForSeconds(controllerDelayTime);
        canUpdateSlider = true;
    }   

    void SetUI()
    {        
        float sliderWidth = sliderArea.GetComponent<RectTransform>().sizeDelta.x;
        float incrementValue = sliderWidth / (moodSlider.maxValue - moodSlider.minValue);

        handle.sizeDelta = new Vector2(incrementValue, 0);

        float startingValue = -(sliderWidth / 2);
        for (int i = 0; i < moodSlider.maxValue; i++)
        {
            var spawnedRectTransform = Instantiate(nodeImage, sliderArea.transform).GetComponent<RectTransform>();
            spawnedRectTransform.localPosition = new Vector2(startingValue, nodeImage.GetComponent<RectTransform>().localPosition.y);
            spawnedRectTransform.sizeDelta = new Vector2(incrementValue + sizeDif, sizeDif);
            spawnedRectTransform.anchorMin = new Vector2(.5f, 0);
            spawnedRectTransform.anchorMax = new Vector2(.5f, 1);
            startingValue += incrementValue;
        }

        if (isHandleBehind)
            moodSlider.handleRect.gameObject.transform.SetAsLastSibling();
    }

    void UpdateUI()
    {        
        moodSlider.value = moodIndex;

        if (moodIndex > 6)
        {
            handleImage.color = tenseColor;
        }
        else if (moodIndex == 6)
        {
            handleImage.color = neutralColor;
        }
        else
        {
            handleImage.color = relaxedColor;
        }        
    }
}
