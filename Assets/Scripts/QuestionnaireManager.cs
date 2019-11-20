using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OuterRimStudios.Utilities;
using System.Linq;

public class QuestionnaireManager : MonoBehaviour
{
    const int PREVIOUS_QUESTION = -1;
    const int NEXT_QUESTION = 1;
    public List<GameObject> questions;

    public OVRInput.Button nextQuestionButton;
    public OVRInput.Button previousQuestionButton;

    GameObject currentQuestion;
    int currentQuestionIndex;
    const string ANALYTICS_TITLE = "QuestionnaireResponses";

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
            if (currentQuestionIndex < questions.Count - 1)
                currentQuestionIndex++;
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
                currentQuestionIndex--;
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

        currentQuestion = questions[0];
        currentQuestion.SetActive(true);

        currentQuestionIndex = 0;

        //Reset Answer slider
    }

    void SendAnalytics()
    {
        var data = new List<QuestionnaireData> { new QuestionnaireData{ 
            UserID = 0,
            VideoID = ChicagoSceneTransition.Instance.GetLastVideo() != null ? ChicagoSceneTransition.Instance.GetLastVideo().videoID : -1,
            EmpathyAntwuan = 5,
            EmpathyTony = 5,
            Anger = 5
        } };

        AnalyticsUtilities.Event(ANALYTICS_TITLE, data);
        GetAnalytics();
    }

    List<QuestionnaireData> GetAnalytics()
    {
        return AnalyticsUtilities.GetData<QuestionnaireData>(ANALYTICS_TITLE);
    }

    int GetCurrentUserID()
    {
        List<QuestionnaireData> data = GetAnalytics();
        if (data != null && data.Count > 0)
            return data[data.Count - 1].UserID + 1;
        else
            return 0;
    }
}

public class QuestionnaireData
{
    public int UserID { get; set; }
    public int VideoID { get; set; }
    public int EmpathyAntwuan { get; set; }
    public int EmpathyTony { get; set; }
    public int Anger { get; set; }

    public string GetValues()
    {
        return $"UserID: {UserID} | VideoID: {VideoID} | EmpathyAntwuan: {EmpathyAntwuan} | EmpathyTony: {EmpathyTony} | Anger: {Anger}";
    }
}