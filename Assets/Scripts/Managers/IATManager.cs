using System.Collections.Generic;
using OuterRimStudios.Utilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
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
    bool isFirstTest = true;

    List<IATInfo> iatInfos = new List<IATInfo>();
    string userID = "0";
    float itemStartTime;    //Time when the current image was displayed

    bool waitingToStart;

    private void OnEnable()
    {
        iatInfos.Clear();
        ActivateIntro(true);

        waitingToStart = true;        

        OVRInputManager.OnButtonDown += OnButtonDown;
    }

    private void OnDisable()
    {
        OVRInputManager.OnButtonDown -= OnButtonDown;
    }

    //Really only being used for local testing
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && waitingToStart)
        {
            if (introPanel.activeInHierarchy)
            {
                ActivateIntro(false);
                ActivateTutorial(true);
            }
            else
            {
                ActivateTutorial(false);
                NextRound();
                NextItem();
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !waitingToStart)
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
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !waitingToStart)
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
                ActivateIntro(false);
                ActivateTutorial(true);
            }
            else
            {
                ActivateTutorial(false);
                NextRound();
                NextItem();
            }
        }
        else if (button == OVRInput.Button.PrimaryThumbstickLeft && !waitingToStart)
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
        else if (button == OVRInput.Button.PrimaryThumbstickRight && !waitingToStart)
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

    void ActivateIntro(bool isActive)
    {
        introPanel.SetActive(isActive);
        iatBase.SetActive(!isActive);
    }

    void ActivateTutorial(bool isActive)
    {
        waitingToStart = isActive;

        if (isActive)
        {
            tutorialText.text = $"Push the joystick <b>left</b> to match the items that belong to the category {ConvertKeysToText(rounds[nextRoundIndex].leftKeys)}."
                             + $"\n\nPush the joystick <b>right</b> to match the items that belong to the category {ConvertKeysToText(rounds[nextRoundIndex].rightKeys)}.";

            UpdateTextDisplays(rounds[nextRoundIndex]);
        }        

        tutorialText.transform.parent.gameObject.SetActive(isActive);
        
        choiceImage.gameObject.SetActive(!isActive);        
    }

    void NextRound()
    {        
        SetUpRoundSprites(rounds[nextRoundIndex]);
        nextRoundIndex++;
    }

    void SetUpRoundSprites(Round round)
    {  
        rightSprites.Clear();

        foreach (IATKey key in round.rightKeys)
        {
            rightSprites.AddRange(GetSpritesFromEnum(key));
        }

        leftSprites.Clear();

        foreach (IATKey key in round.leftKeys)
        {
            leftSprites.AddRange(GetSpritesFromEnum(key));
        }

        allSprites.Clear();
        allSprites.AddRange(rightSprites);
        allSprites.AddRange(leftSprites);

        //======Shuffle Order of Sprites=======
        allSprites = allSprites.OrderBy(a => Guid.NewGuid()).ToList();        
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

    void CheckIfTestEnded()
    {
        //========Check if the IAT test has ended==========
        if (nextRoundIndex == rounds.Length)
        {
            nextRoundIndex = 0;
            SendAnalytics();
            isFirstTest = !isFirstTest;
            ChicagoSceneTransition.Instance.NextScene();
        }
        else
            ActivateTutorial(true);
    }

    void UpdateTextDisplays(Round round)
    {
        rightText.text = ConvertKeysToText(round.rightKeys);
        leftText.text = ConvertKeysToText(round.leftKeys);        
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
        string occurance = isFirstTest ? "pre" : "post";
        int roundID = nextRoundIndex - 1;
        float responseTime = Time.time - itemStartTime;
        
        iatInfos.Add(new IATInfo(userID, "a", occurance, imageID, roundID, answer, responseTime));
        Analytics.CustomEvent(ANALYTICS_TITLE, new Dictionary<string, object>
        {
            { "UserID", userID},
            { "GroupID", "a"},
            { "Occurance", occurance},
            { "ImageID", imageID},
            { "RoundID", roundID},
            { "Answer", answer},
            { "ResponseTime", responseTime}
        });
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
    public string Occurance { get; set; }
    public string ImageID { get; set; }
    public int RoundID { get; set; }
    public string Answer { get; set; }
    public float ResponseTime { get; set; }

    public IATInfo(string userID, string groupID, string occurance, string imageID, int roundID, string answer, float responseTime)
    {
        UserID = userID;
        GroupID = groupID;
        Occurance = occurance;
        ImageID = imageID;
        RoundID = roundID;
        Answer = answer;
        ResponseTime = responseTime;
    }
}