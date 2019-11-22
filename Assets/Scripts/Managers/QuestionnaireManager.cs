using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using OuterRimStudios.Utilities;

public class QuestionnaireManager : MonoBehaviour
{
    const int PREVIOUS_QUESTION = -1;
    const int NEXT_QUESTION = 1;
    const string ANALYTICS_TITLE = "QuestionnaireResponses";
    public List<GameObject> questions;
    public BubbleSlider responseSlider;

    public OVRInput.Button nextQuestionButton;
    public OVRInput.Button previousQuestionButton;

    GameObject currentQuestion;
    int currentQuestionIndex;
    int[] responses;
    string userID = "0";

    private void Start()
    {
        Reset();
    }

    private void OnEnable()
    {
        OVRInputManager.OnButtonDown += OnButtonDown;
    }

    private void OnDisable()
    {
        OVRInputManager.OnButtonDown -= OnButtonDown;
    }

    void OnButtonDown(OVRInput.Button button)
    {
        if (button == nextQuestionButton)
            ChangeQuestion(NEXT_QUESTION);
        else if(button == previousQuestionButton)
            ChangeQuestion(PREVIOUS_QUESTION);
    }

    //Cycles the questions based on the direction [-1 = previous | 1 = next]
    void ChangeQuestion(int direction)
    {
        if(direction == NEXT_QUESTION)
        {
            responses[currentQuestionIndex] = responseSlider.GetSliderValue();
            if (currentQuestionIndex < questions.Count - 1)
            {
                currentQuestionIndex++;
                if (responses[currentQuestionIndex] == -1)
                    responseSlider.Reset();
                else
                    responseSlider.SetSliderValue(responses[currentQuestionIndex]);
            }
            else
            {
                //send analytics
                SendAnalytics();
                //scene transition 
                ChicagoSceneTransition.Instance.NextScene();
                Reset();
            }
        }
        else if(direction == PREVIOUS_QUESTION)
        {
            if (currentQuestionIndex > 0)
            {
                currentQuestionIndex--;
                responseSlider.SetSliderValue(responses[currentQuestionIndex]);
            }
        }

        if (currentQuestion != questions[currentQuestionIndex])
        {
            currentQuestion.SetActive(false);
            currentQuestion = questions[currentQuestionIndex];
            currentQuestion.SetActive(true);
        }
    }

    private void Reset()
    {
        foreach (GameObject go in questions)
            go.SetActive(false);

        responses = new int[questions.Count];
        for(int i = 0; i < responses.Length; i++)
            responses[i] = -1;

        currentQuestion = questions[0];
        currentQuestion.SetActive(true);

        currentQuestionIndex = 0;

        //Reset Answer slider
        responseSlider.Reset();
    }
    

    void SendAnalytics()
    {
        int videoID = ChicagoSceneTransition.Instance.GetLastVideo() != null ? ChicagoSceneTransition.Instance.GetLastVideo().videoID : -1;
        var data = new List<QuestionnaireData> { new QuestionnaireData{ 
            UserID = userID,
            VideoID = videoID,
            EmpathyAntwuan = responses[0],
            EmpathyTony = responses[1],
            Anger = responses[2]
        } };

        AnalyticsUtilities.Event(ANALYTICS_TITLE, data);

        Analytics.CustomEvent(ANALYTICS_TITLE, new Dictionary<string, object>
        {
            { "UserID", userID},
            { "VideoID", videoID},
            { "EmpathyAntwaun", responses[0]},
            { "EmpathyTony", responses[1]},
            { "Anger", responses[2]}
        });
    }

    List<QuestionnaireData> GetAnalytics()
    {
        return AnalyticsUtilities.GetData<QuestionnaireData>(ANALYTICS_TITLE);
    }
}

public class QuestionnaireData
{
    public string UserID { get; set; }
    public int VideoID { get; set; }
    public int EmpathyAntwuan { get; set; }
    public int EmpathyTony { get; set; }
    public int Anger { get; set; }

    public string GetValues()
    {
        return $"UserID: {UserID} | VideoID: {VideoID} | EmpathyAntwuan: {EmpathyAntwuan} | EmpathyTony: {EmpathyTony} | Anger: {Anger}";
    }
}