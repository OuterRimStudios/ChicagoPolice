﻿using System.Collections.Generic;
using OuterRimStudios.Utilities;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class IATManager : MonoBehaviour
{
    const string ANALYTICS_TITLE = "IATResults";
    public IATCollection[] collections;
    public Round[] rounds;

    [Header("UI Variables")]
    public Image choiceImage;
    public GameObject wrongAnswerPrompt;
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;

    [Space]
    public GameObject introPanel;
    public GameObject iatBase;
    public TextMeshProUGUI tutorialText;

    List<Sprite> leftSprites = new List<Sprite>();
    List<Sprite> rightSprites = new List<Sprite>();
    List<Sprite> allSprites = new List<Sprite>();

    int nextSpriteIndex;
    int nextRoundIndex;
    int occasionIndex;

    List<IATInfo> iatInfos = new List<IATInfo>();
    string userID = "0";
    float itemStartTime;    //Time when the current image was displayed

    bool waitingToStart;

    private void OnEnable()
    {
        iatInfos.Clear();
        itemStartTime = 0;
        OVRInputManager.OnButtonDown += OnButtonDown;
    }

    private void OnDisable()
    {
        OVRInputManager.OnButtonDown -= OnButtonDown;
    }

    private void Start()
    {
        waitingToStart = true;
        SetActiveTextObjects(false);
    }

    //Really only being used for local testing
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && waitingToStart)
        {
            if (introPanel.activeInHierarchy)
            {
                introPanel.SetActive(false);
                ActivateTutorial();
            }
            else
            {
                waitingToStart = false;
                NextRound();
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (rightSprites.Contains(choiceImage.sprite))
            {
                //Store Stats
                CreateIATInfo(choiceImage.sprite.name, GetItemKey(choiceImage.sprite).ToString());
                NextItem();
            }
            else
            {
                //Display "Nope"
                wrongAnswerPrompt.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (leftSprites.Contains(choiceImage.sprite))
            {
                //Store Stats
                CreateIATInfo(choiceImage.sprite.name, GetItemKey(choiceImage.sprite).ToString());
                NextItem();
            }
            else
            {
                wrongAnswerPrompt.SetActive(true);
            }
        }
    }

    void OnButtonDown(OVRInput.Button button)
    {
        if (button == OVRInput.Button.Three && waitingToStart)
        {
            if (introPanel.activeInHierarchy)
            {
                introPanel.SetActive(false);
                ActivateTutorial();
            }
            else
            {
                waitingToStart = false;
                NextRound();
            }
        }
        else if (button == OVRInput.Button.PrimaryThumbstickLeft)
        {
            if (leftSprites.Contains(choiceImage.sprite))
            {
                //Store Stats
                CreateIATInfo(choiceImage.sprite.name, GetItemKey(choiceImage.sprite).ToString());
                NextItem();
            }
            else
            {
                wrongAnswerPrompt.SetActive(true);
            }
        }
        else if (button == OVRInput.Button.PrimaryThumbstickRight)
        {
            if (rightSprites.Contains(choiceImage.sprite))
            {
                //Store Stats
                CreateIATInfo(choiceImage.sprite.name, GetItemKey(choiceImage.sprite).ToString());
                NextItem();
            }
            else
            {
                //Display "Nope"
                wrongAnswerPrompt.SetActive(true);
            }
        }
    }

    void ActivateTutorial()
    {
        waitingToStart = true;

        tutorialText.text = "Push the joystick <b>left</b> to match the items that belong to the category " + ConvertKeysToText(rounds[nextRoundIndex].leftKeys) + "." 
                             +"\n\nPush the joystick <b>right</b> to match the items that belong to the category " + ConvertKeysToText(rounds[nextRoundIndex].rightKeys) + ".";

        tutorialText.transform.parent.gameObject.SetActive(true);
        UpdateTextDisplays(rounds[nextRoundIndex]);
        iatBase.SetActive(true);
        choiceImage.gameObject.SetActive(false);
        SetActiveTextObjects(true);
    }

    void NextRound()
    {
        tutorialText.transform.parent.gameObject.SetActive(false);
        SetUpRoundSprites(rounds[nextRoundIndex]);
        nextRoundIndex++;
    }

    void SetUpRoundSprites(Round round)
    {  
        //=====SetUpTheSprites=====
        rightSprites.Clear();
        leftSprites.Clear();        

        foreach (IATKey key in round.rightKeys)
        {
            rightSprites.AddRange(GetSpritesFromEnum(key));
        }

        foreach (IATKey key in round.leftKeys)
        {
            leftSprites.AddRange(GetSpritesFromEnum(key));
        }

        allSprites.Clear();
        allSprites.AddRange(rightSprites);
        allSprites.AddRange(leftSprites);

        //======Shuffle Order of Sprites=======
        allSprites = allSprites.OrderBy(a => Guid.NewGuid()).ToList();
        NextItem();
    }

    void NextItem()
    {
        wrongAnswerPrompt.SetActive(false);

        if (nextSpriteIndex == allSprites.Count)
        {
            nextSpriteIndex = 0;
            choiceImage.gameObject.SetActive(false);
            CheckIfTestEnded();            
        }
        else
        {
            choiceImage.gameObject.SetActive(true);
            choiceImage.sprite = allSprites[nextSpriteIndex];
            nextSpriteIndex++;
        }

        itemStartTime = Time.time;
    }

    void UpdateTextDisplays(Round round)
    {
        rightText.text = ConvertKeysToText(round.rightKeys);
        leftText.text = ConvertKeysToText(round.leftKeys);
    }

    void SetActiveTextObjects(bool isActive)
    {
        rightText.gameObject.SetActive(isActive);
        leftText.gameObject.SetActive(isActive);
    }

    void ResetIAT()
    {
        introPanel.SetActive(true);
        waitingToStart = true;
        nextRoundIndex = 0;
    }

    void CheckIfTestEnded()
    {
        //========Check if the IAT test has ended==========
        if (nextRoundIndex == rounds.Length)
        {
            SendAnalytics();
            occasionIndex++;
            ChicagoSceneTransition.Instance.NextScene();
            ResetIAT();
        }
        else
            ActivateTutorial();
    }

    IATKey GetItemKey(Sprite item)
    {
        foreach (IATCollection collection in collections)
            if (collection.IATobjects.Contains(item))
                return collection.key;

        return IATKey.None;
    }

    string ConvertKeysToText(List<IATKey> keys)
    {
        string text = "";
        foreach (IATKey key in keys)
        {            
            if (text == "")
            {
                text = $"<b>{key.ToString()}</b>";
            }
            else
            {
                text += $" or <b>{key.ToString()}</b>";
            }
        }

        return text;
    }

    List<Sprite> GetSpritesFromEnum(IATKey key)
    {
        var collection = collections.FirstOrDefault(x => x.key == key)?.IATobjects;
        return collection;
    }

    void CreateIATInfo(string imageID, string answer)
    {
        float responseTime = Time.time - itemStartTime;
        iatInfos.Add(new IATInfo(userID, "a", occasionIndex, imageID, nextRoundIndex - 1, answer, responseTime));
    }

    void SendAnalytics()
    {
        AnalyticsUtilities.Event(ANALYTICS_TITLE, iatInfos);
    }
}

public class IATInfo
{
    public string UserID { get; set; }
    public string GroupID { get; set; }
    public int OccassionID { get; set; }
    public string ImageID { get; set; }
    public int RoundID { get; set; }
    public string Answer { get; set; }
    public float ResponseTime { get; set; }

    public IATInfo(string userID, string groupID, int occasionID, string imageID, int roundID, string answer, float responseTime)
    {
        UserID = userID;
        GroupID = groupID;
        OccassionID = occasionID;
        ImageID = imageID;
        RoundID = roundID;
        Answer = answer;
        ResponseTime = responseTime;
    }
}