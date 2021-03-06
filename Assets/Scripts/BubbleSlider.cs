﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using OuterRimStudios.Utilities;

public class BubbleSlider : MonoBehaviour
{
    [Header("UI Variables")]
    public bool changeColor;
    [ConditionalHide("changeColor", true)] public bool isNotGradient;
    [ConditionalHide("changeColor", true)] public Color rightColor;
    [ConditionalHide("changeColor", true)] public Color leftColor;
    [ConditionalHide("isNotGradient", true)] public Color middleColor;

    public Image nodeImage;
    public RectTransform sliderArea;
    [Tooltip("The transform that the bubbles will be childed under.")]
    public Transform bubbleParent;

    public bool changeSize = true;
    [Tooltip("Setting how much smaller the slider nodes are compared to the handle")]
    [ConditionalHide("changeSize", true)] public float sizeDif = 0f;
    public bool isHandleInFront = false;

    public float holdDownTime = .4f;
    Coroutine heldDownEnumerator;

    Slider slider;
    RectTransform handle;
    Image handleImage;

    byte stepIndex;
    byte middleIndex;

    void Awake()
    {
        slider = GetComponent<Slider>();

        if (slider == null)
        {
            Debug.LogError($"Hey! {name} has a BubbleSlider without a Slider component.");
        }

        handle = slider.handleRect;
        handleImage = handle.GetComponent<Image>();        
        SetUI();
    }

    void OnEnable()
    {
        Reset();
    #if UNITY_ANDROID
        OVRInputManager.OnButtonDown += OnButtonDown;
        OVRInputManager.OnButtonUp += OnButtonUp;
    #endif
    }

    private void OnDisable()
    {
    #if UNITY_ANDROID
        OVRInputManager.OnButtonDown -= OnButtonDown;
        OVRInputManager.OnButtonUp -= OnButtonUp;
    #endif
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            stepIndex = stepIndex.IncrementClamped(slider.maxValue);
            UpdateUI();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            stepIndex = stepIndex.DecrementClamped(slider.minValue);            
            UpdateUI();
        }
    }
#endif
#if UNITY_ANDROID
    void OnButtonUp(OVRInput.Button key)
    {
        if (key == OVRInput.Button.PrimaryThumbstickLeft || key == OVRInput.Button.PrimaryThumbstickRight)
        {
            if (heldDownEnumerator != null)
                StopCoroutine(heldDownEnumerator);
        }
    }
    void OnButtonDown(OVRInput.Button key)
    {
        if(key == OVRInput.Button.PrimaryThumbstickLeft)
        {
            stepIndex = stepIndex.DecrementClamped(slider.minValue);
            UpdateUI();
            if (heldDownEnumerator != null)
                StopCoroutine(heldDownEnumerator);
            heldDownEnumerator = StartCoroutine(UpdateHold(false));
        }
        else if(key == OVRInput.Button.PrimaryThumbstickRight)
        {
            stepIndex = stepIndex.IncrementClamped(slider.maxValue);
            UpdateUI();
            if (heldDownEnumerator != null)
                StopCoroutine(heldDownEnumerator);
            heldDownEnumerator = StartCoroutine(UpdateHold(true));
        }
    }
#endif

    IEnumerator UpdateHold(bool isIncreased)
    {
        yield return new WaitForSeconds(holdDownTime);
        if (isIncreased)
        {
            stepIndex = stepIndex.IncrementClamped(slider.maxValue);
        }
        else
        {
            stepIndex = stepIndex.DecrementClamped(slider.minValue);
        }

        UpdateUI();
        heldDownEnumerator = StartCoroutine(UpdateHold(isIncreased));
    }

    void SetUI()
    {        
        float sliderWidth = sliderArea.GetComponent<RectTransform>().sizeDelta.x;
        float incrementValue = sliderWidth / (slider.maxValue - slider.minValue);

        handle.sizeDelta = new Vector2(incrementValue, 0);

        float startingValue = -(sliderWidth / 2);
        for (int i = (int)(slider.minValue - 1); i < slider.maxValue; i++)
        {
            var spawnedRectTransform = Instantiate(nodeImage, bubbleParent).GetComponent<RectTransform>();
            spawnedRectTransform.localPosition = new Vector2(startingValue, nodeImage.GetComponent<RectTransform>().localPosition.y);
            if (changeSize)
            {
                spawnedRectTransform.sizeDelta = new Vector2(incrementValue + sizeDif, sizeDif);
                spawnedRectTransform.anchorMin = new Vector2(.5f, 0);
                spawnedRectTransform.anchorMax = new Vector2(.5f, 1);
            }
            startingValue += incrementValue;
        }

        if (isHandleInFront)
            slider.handleRect.gameObject.transform.SetAsLastSibling();
    }

    void UpdateUI()
    {        
        slider.value = stepIndex;

        if (changeColor)
        {
            if (isNotGradient)
            {
                if (stepIndex > middleIndex)
                    handleImage.color = rightColor;
                else if (stepIndex == middleIndex)
                    handleImage.color = middleColor;
                else
                    handleImage.color = leftColor;
            }
            else
            {
                float ratio = 1.0f / slider.maxValue;

                handleImage.color = Color.Lerp(leftColor, rightColor, ratio * stepIndex);
            }            
        }
    }

    public void Reset()
    {
        middleIndex = (byte)Mathf.CeilToInt(slider.maxValue / 2.0f);
        stepIndex = middleIndex;
        UpdateUI();
    }

    public int GetSliderValue()
    {
        return (int)slider.value;
    }

    public void SetSliderValue(int value)
    {
        stepIndex = (byte)value;
        UpdateUI();
    }
}