﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OuterRimStudios.Utilities;

public class QuestionnaireManager : MonoBehaviour
{
    const int PREVIOUS_QUESTION = -1;
    const int NEXT_QUESTION = 1;
    const string ANALYTICS_TITLE = "QuestionnaireResponses";
    public List<GameObject> questions;
    public BubbleSlider responseSlider;

    GameObject currentQuestion;
    int currentQuestionIndex;
    bool acceptInput = true;
    int[] responses;

    private void Start()
    {
        Reset();
    }

    private void Update()
    {
        OVRInput.Update();
        if (acceptInput)
        {
            if (OVRInput.Get(OVRInput.Button.Three))
                ChangeQuestion(NEXT_QUESTION);
            else if (OVRInput.Get(OVRInput.Button.Four))
                ChangeQuestion(PREVIOUS_QUESTION);
        }
    }

    //Cycles the questions based on the direction [-1 = previous | 1 = next]
    void ChangeQuestion(int direction)
    {
        StartCoroutine(ResetInput());
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
                //ChicagoSceneTransition.Instance.NextScene();
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

        acceptInput = true;
    }

    IEnumerator ResetInput()
    {
        acceptInput = false;
        yield return new WaitForSeconds(0.25f);
        acceptInput = true;
    }

    void SendAnalytics()
    {
        var data = new List<QuestionnaireData> { new QuestionnaireData{ 
            UserID = 0,
            VideoID = ChicagoSceneTransition.Instance.GetLastVideo() != null ? ChicagoSceneTransition.Instance.GetLastVideo().videoID : -1,
            EmpathyAntwuan = responses[0],
            EmpathyTony = responses[1],
            Anger = responses[2]
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