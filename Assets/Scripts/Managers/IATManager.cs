using System;
using System.Collections.Generic;
using System.Linq;
using OuterRimStudios.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class IATManager : MonoBehaviour 
{
    const string ANALYTICS_TITLE = "IATResults";
    public IATCollection[] practiceCollections;
    public IATCollection[] picCollections;
    public IATCollection[] wordCollections;
    public Round[] rounds;

    [Header("UI Variables")]
    public Image choiceImage;
    public GameObject wrongAnswerPrompt;
    public GameObject renderController;

    [Space]
    public GameObject iatBase;
    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI choiceText;

    List<Sprite> picSprites = new List<Sprite>();
    List<Sprite> wordSprites = new List<Sprite>();
    List<Sprite> introWordSprites = new List<Sprite>();

    int answerCount;
    int roundIndex;
    int roundSpriteCount;
    bool waitingToStart;    
    bool isDisplayPic = true;

    //IAT Analytics Variables 
    List<IATInfo> iatInfos = new List<IATInfo>();
    float itemStartTime; //Time when the current image was displayed    
    bool isFirstTest = true;
    string userID;
    string groupID;
    int headsetID;
    string testTimestamp;

    private void OnEnable() {
        iatInfos.Clear();
        ActivateTutorial(true);

    #if UNITY_ANDROID
        OVRInputManager.OnButtonDown += OnButtonDown;
    #endif
        userID = ChicagoSceneTransition.Instance.UserID;
        groupID = ChicagoSceneTransition.Instance.GroupID;
        headsetID = ChicagoSceneTransition.Instance.HeadsetID;
        testTimestamp = ChicagoSceneTransition.Instance.TestTimestamp;
    }

    private void OnDisable() {
    #if UNITY_ANDROID
        OVRInputManager.OnButtonDown -= OnButtonDown;
    #endif
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && waitingToStart)
        {
            ActivateTutorial(false);
            SetUpNextRound();
            NextItem();
            
        } else if (Input.GetKeyDown(KeyCode.LeftArrow) && !waitingToStart) {
            CheckIfCorrect(rounds[roundIndex].yesKeys);
        } else if (Input.GetKeyDown(KeyCode.RightArrow) && !waitingToStart) {
            CheckIfCorrect(rounds[roundIndex].noKeys);
        }
    }
#endif
#if UNITY_ANDROID
    void OnButtonDown(OVRInput.Button button) 
    {
        if (button == OVRInput.Button.Three && waitingToStart)
        {           
            ActivateTutorial(false);
            SetUpNextRound();
            NextItem();            
        }
        else if (button == OVRInput.Button.PrimaryThumbstickLeft && !waitingToStart) 
        {
            CheckIfCorrect(rounds[roundIndex].yesKeys);            
        } 
        else if (button == OVRInput.Button.PrimaryThumbstickRight && !waitingToStart) 
        {
            CheckIfCorrect(rounds[roundIndex].noKeys);            
        }
    }
#endif
    void ActivateTutorial(bool isActive) 
    {
        waitingToStart = isActive;

        if (isActive) 
        {
            tutorialText.text = "";

            if (roundIndex == IATConstants.TUTORIAL_ROUND)
            {
                tutorialText.text += "For example: \n";
            }

            tutorialText.text += $"Push the joystick <b>left</b> to match the items that belong to the category {ConvertKeysToText(rounds[roundIndex].yesKeys)}." +
                $"\n\nPush the joystick <b>right</b> if they do not.";
        }

        choiceText.text = ConvertKeysToText(rounds[roundIndex].yesKeys);
        tutorialText.transform.parent.gameObject.SetActive(isActive);
        choiceImage.gameObject.SetActive(!isActive);
        renderController.SetActive(isActive);
    }

    void SetUpNextRound() 
    {
        picSprites.Clear();
        wordSprites.Clear();

        if (roundIndex == IATConstants.TUTORIAL_ROUND)
        {
            foreach (IATCollection collection in practiceCollections)
            {
                picSprites.AddRange(CollectionUtilities.GetRandomItems(collection.IATobjects, 4));
            }
        }
        else
        {
            foreach (IATCollection collection in picCollections)
            {
                picSprites.AddRange(CollectionUtilities.GetRandomItems(collection.IATobjects, 4));
            }

            foreach (IATCollection collection in wordCollections)
            {
                introWordSprites.AddRange(CollectionUtilities.GetRandomItems(collection.IATobjects, 2));
                wordSprites.AddRange(CollectionUtilities.GetRandomItems(collection.IATobjects, 4));
            }
        }

        roundSpriteCount = picSprites.Count + wordSprites.Count + introWordSprites.Count;
        isDisplayPic = true;
    }

    void NextItem() 
    {
        wrongAnswerPrompt.SetActive(false);

        if (answerCount == roundSpriteCount) 
        {
            answerCount = 0;
            roundIndex++;
            choiceImage.gameObject.SetActive(false);
            CheckIfTestEnded();
        } 
        else
        {
            choiceImage.gameObject.SetActive(true);

            if (answerCount < IATConstants.SOLO_WORD_INDEX && roundIndex != IATConstants.TUTORIAL_ROUND)
            {
                choiceImage.sprite = GetRandomSprite(introWordSprites);
            }
            else
            {
                if (isDisplayPic)
                {
                    choiceImage.sprite = GetRandomSprite(picSprites);
                }
                else
                {
                    choiceImage.sprite = GetRandomSprite(wordSprites);
                }

                if (wordSprites.Any())
                    isDisplayPic = !isDisplayPic;
            }    

            answerCount++;
        }

        itemStartTime = Time.time;
    }

    void CheckIfCorrect(List<IATKey> matchingKeys)
    {
        var itemKey = GetItemKey(choiceImage.sprite);
        if (matchingKeys.Contains(itemKey))
        {
            //Store Stats
            CreateIATInfo(choiceImage.sprite.name, itemKey.ToString());
            NextItem();
        }
        else
        {
            //Display "Nope"
            wrongAnswerPrompt.SetActive(true);
        }
    }

    void CheckIfTestEnded() 
    {
        //========Check if the IAT test has ended==========
        if (roundIndex == rounds.Length) 
        {
            roundIndex = 0;
            SendAnalytics();
            isFirstTest = !isFirstTest;
            ChicagoSceneTransition.Instance.NextScene();
        } 
        else
            ActivateTutorial(true);
    }

    Sprite GetRandomSprite(List<Sprite> spriteList)
    {
        int randomIndex = Random.Range(0, spriteList.Count);
        var sprite = spriteList[randomIndex];
        spriteList.RemoveAt(randomIndex);
        return sprite;
    }

    IATKey GetItemKey(Sprite item) 
    {
        List<IATCollection> _collections = picCollections.Concat(wordCollections).Concat(practiceCollections).ToList();
        foreach (IATCollection collection in _collections)
            if (collection.IATobjects.Contains(item))
                return collection.key;

        return IATKey.None;
    }

    string ConvertKeysToText(List<IATKey> keys) 
    {
        string text = "";
        foreach (IATKey key in keys) {
            if (text == "") {
                text = $"<b>{key.ToString()}</b>";
            } else {
                text += $" or <b>{key.ToString()}</b>";
            }
        }

        return text;
    }

    void CreateIATInfo(string imageID, string answer) 
    {
        string occurance = isFirstTest ? "pre" : "post";
        int roundID = roundIndex;
        float responseTime = Time.time - itemStartTime;
        IATInfo iatInfo = new IATInfo(userID, groupID, headsetID, testTimestamp, occurance, imageID, roundID, answer, responseTime);
        iatInfos.Add(iatInfo);

        Analytics.CustomEvent(ANALYTICS_TITLE, new Dictionary<string, object> { { "UserID", iatInfo.UserID },
            { "GroupID", iatInfo.GroupID },
            { "HeadsetID", iatInfo.HeadsetID },
            { "TestTimestamp", iatInfo.TestTimestamp },
            { "Occurance", iatInfo.Occurance },
            { "ImageID", iatInfo.ImageID },
            { "RoundID", iatInfo.RoundID },
            { "Answer", iatInfo.Answer },
            { "ResponseTime", iatInfo.ResponseTime.ToString("N3") }
        });
    }

    void SendAnalytics()
    {
        AnalyticsUtilities.Event(ANALYTICS_TITLE, iatInfos);
    }
}

public class IATInfo {
    public string UserID { get; set; }
    public string GroupID { get; set; }
    public int HeadsetID { get; set; }
    public string TestTimestamp { get; set; }
    public string Occurance { get; set; }
    public string ImageID { get; set; }
    public int RoundID { get; set; }
    public string Answer { get; set; }
    public float ResponseTime { get; set; }

    public IATInfo(string userID, string groupID, int headsetID, string timestamp, string occurance, string imageID, int roundID, string answer, float responseTime) {
        UserID = userID;
        GroupID = groupID;
        HeadsetID = headsetID;
        TestTimestamp = timestamp;
        Occurance = occurance;
        ImageID = imageID;
        RoundID = roundID;
        Answer = answer;
        ResponseTime = responseTime;
    }
}

internal class IATConstants
{
    public const int TUTORIAL_ROUND= 0;
    public const int SOLO_WORD_INDEX = 4;
}