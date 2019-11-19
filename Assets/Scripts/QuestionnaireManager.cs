using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionnaireManager : MonoBehaviour
{
    const int PREVIOUS_QUESTION = -1;
    const int NEXT_QUESTION = 1;
    public List<GameObject> questions;

    GameObject currentQuestion;
    int currentQuestionIndex;
    bool acceptInput = true;

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
            if (currentQuestionIndex < questions.Count - 1)
                currentQuestionIndex++;
            else
            {
                //send analytics
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

        acceptInput = true;
    }

    IEnumerator ResetInput()
    {
        acceptInput = false;
        yield return new WaitForSeconds(0.25f);
        acceptInput = true;
    }
}