﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

public class BubbleSlider : MonoBehaviour
{
    [Header("UI Variables")]
    public bool changeColor;
    public Color middleColor;
    public Color rightColor;
    public Color leftColor;

    public Slider slider;
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

    byte stepIndex = 6;
    bool canUpdateSlider = true;

    void Start()
    {
        handle = slider.handleRect;
        handleImage = handle.GetComponent<Image>();
        Reset();
        SetUI();
    }    

    void Update()
    {
        OVRInput.Update();

        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x > userDeadZone && canUpdateSlider || Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(InputCooldown());

            if (stepIndex < slider.maxValue)
                stepIndex++;

            UpdateUI();
        }
        else if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x < -userDeadZone && canUpdateSlider || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(InputCooldown());

            if (stepIndex > slider.minValue)
                stepIndex--;

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
        float incrementValue = sliderWidth / (slider.maxValue - slider.minValue);

        handle.sizeDelta = new Vector2(incrementValue, 0);

        float startingValue = -(sliderWidth / 2);
        for (int i = 0; i < slider.maxValue; i++)
        {
            var spawnedRectTransform = Instantiate(nodeImage, sliderArea.transform).GetComponent<RectTransform>();
            spawnedRectTransform.localPosition = new Vector2(startingValue, nodeImage.GetComponent<RectTransform>().localPosition.y);
            spawnedRectTransform.sizeDelta = new Vector2(incrementValue + sizeDif, sizeDif);
            spawnedRectTransform.anchorMin = new Vector2(.5f, 0);
            spawnedRectTransform.anchorMax = new Vector2(.5f, 1);
            startingValue += incrementValue;
        }

        if (isHandleBehind)
            slider.handleRect.gameObject.transform.SetAsLastSibling();
    }

    void UpdateUI()
    {        
        slider.value = stepIndex;

        if (changeColor)
        {
            if (stepIndex > Mathf.CeilToInt(slider.maxValue / 2.0f))
                handleImage.color = rightColor;
            else if (stepIndex == Mathf.CeilToInt(slider.maxValue / 2.0f))
                handleImage.color = middleColor;
            else
                handleImage.color = leftColor;
        }
    }

    public void Reset()
    {
        stepIndex = (byte)Mathf.CeilToInt(slider.maxValue / 2.0f);
        UpdateUI();
    }

    public int GetSliderValue()
    {
        return (int)slider.value;
    }

    public void SetSliderValue(int value)
    {
        slider.value = value;
    }
}
