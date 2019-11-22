﻿//Hector sux
using System.Collections;
using System.Collections.Generic;
using OuterRimStudios.Utilities;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class IATManager : MonoBehaviour
{
    public IATCollection[] collections;
    public Round[] rounds;

    [Header("UI Variables")]
    public Image choiceImage;
    public GameObject wrongAnswerPrompt;
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;

    List<Sprite> leftSprites = new List<Sprite>();
    List<Sprite> rightSprites = new List<Sprite>();
    List<Sprite> allSprites = new List<Sprite>();

    int nextSpriteIndex;
    int nextRoundIndex;

    [SerializeField, Header("Control Variables")]
    double userDeadZone = .2;

    private void OnEnable()
    {
        OVRInputManager.OnButtonDown += OnButtonDown;
    }

    private void OnDisable()
    {
        OVRInputManager.OnButtonDown -= OnButtonDown;
    }

    private void Start()
    {       
        NextRound();
    }

    void NextRound()
    {
        if (nextRoundIndex == rounds.Length)
            ChicagoSceneTransition.Instance.NextScene();
        else
            SetUpRound(rounds[nextRoundIndex]);

        nextRoundIndex++;
    }

    void SetUpRound(Round round)
    {
        rightSprites.Clear();
        leftSprites.Clear();
        rightText.text = "";
        leftText.text = "";

        foreach (IATKeys key in round.rightKeys)
        {
            rightSprites.AddRange(GetSpritesFromEnum(key));
            if (rightText.text == "")
            {
                rightText.text = key.ToString();
            }
            else
            {
                rightText.text += "\nor\n" + key.ToString();
            }
        }

        foreach (IATKeys key in round.leftKeys)
        {
            leftSprites.AddRange(GetSpritesFromEnum(key));
            if (leftText.text == "")
            {
                leftText.text = key.ToString();
            }
            else
            {
                leftText.text += "\nor\n" + key.ToString();
            }
        }            

        allSprites.Clear();
        allSprites.AddRange(rightSprites);
        allSprites.AddRange(leftSprites);

        allSprites = allSprites.OrderBy(a => Guid.NewGuid()).ToList();
        NextItem();
    }

    void NextItem()
    {
        wrongAnswerPrompt.SetActive(false);

        if (nextSpriteIndex == allSprites.Count)
        {
            nextSpriteIndex = 0;
            NextRound();
        }
        else
        {
            choiceImage.sprite = allSprites[nextSpriteIndex];
            nextSpriteIndex++;
        }
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))       //OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x > userDeadZone || 
        {
            if (rightSprites.Contains(choiceImage.sprite))
            {
                //Store Stats
                NextItem();
            }
            else
            {
                //Display "Nope"
                wrongAnswerPrompt.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))   //OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x < -userDeadZone || 
        {
            if (leftSprites.Contains(choiceImage.sprite))
            {
                //Store Stats
                NextItem();
            }
            else
            {
                wrongAnswerPrompt.SetActive(true);
            }
        }
    }
#endif

    void OnButtonDown(OVRInput.Button button)
    {
        if(button == OVRInput.Button.PrimaryThumbstickLeft)
        {
            if (leftSprites.Contains(choiceImage.sprite))
            {
                //Store Stats
                NextItem();
            }
            else
            {
                wrongAnswerPrompt.SetActive(true);
            }
        }
        else if(button == OVRInput.Button.PrimaryThumbstickRight)
        {
            if (rightSprites.Contains(choiceImage.sprite))
            {
                //Store Stats
                NextItem();
            }
            else
            {
                //Display "Nope"
                wrongAnswerPrompt.SetActive(true);
            }
        }
    }

    List<Sprite> GetSpritesFromEnum(IATKeys key)
    {
        var collection = collections.FirstOrDefault(x => x.key == key)?.IATobjects;
        return collection;
    }
}