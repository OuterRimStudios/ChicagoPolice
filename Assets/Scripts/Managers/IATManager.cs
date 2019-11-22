//Hector sux
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
    const string ANALYTICS_TITLE = "IATResults";
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

    List<IATInfo> iatInfos = new List<IATInfo>();
    string userID = "0";
    float itemStartTime;    //Time when the current image was displayed

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
        NextRound();
    }

    void NextRound()
    {
        if (nextRoundIndex == rounds.Length)
        {
            SendAnalytics();
            ChicagoSceneTransition.Instance.NextScene();
        }
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

        itemStartTime = Time.time;
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
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
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
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
                CreateIATInfo(choiceImage.sprite.name, "answer");
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
                CreateIATInfo(choiceImage.sprite.name, "answer");
                NextItem();
            }
            else
            {
                //Display "Nope"
                wrongAnswerPrompt.SetActive(true);
            }
        }
    }

    void CreateIATInfo(string imageID, string answer)
    {
        float responseTime = Time.time - itemStartTime;
        iatInfos.Add(new IATInfo(userID, "a", imageID, nextRoundIndex - 1, answer, responseTime));
    }

    void SendAnalytics()
    {
        AnalyticsUtilities.Event(ANALYTICS_TITLE, iatInfos);
    }

    List<Sprite> GetSpritesFromEnum(IATKeys key)
    {
        var collection = collections.FirstOrDefault(x => x.key == key)?.IATobjects;
        return collection;
    }
}

public class IATInfo
{
    public string UserID { get; set; }
    public string GroupID { get; set; }
    public string ImageID { get; set; }
    public int RoundID { get; set; }
    public string Answer { get; set; }
    public float ResponseTime { get; set; }

    public IATInfo(string userID, string groupID, string imageID, int roundID, string answer, float responseTime)
    {
        UserID = userID;
        GroupID = groupID;
        ImageID = imageID;
        RoundID = roundID;
        Answer = answer;
        ResponseTime = responseTime;
    }
}