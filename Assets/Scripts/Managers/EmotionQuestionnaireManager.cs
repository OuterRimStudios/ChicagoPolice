using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using OuterRimStudios.Utilities;

public class EmotionQuestionnaireManager : QuestionnaireManager
{
    private void Awake()
    {
        ANALYTICS_TITLE = "EmotionQuestionnaireResponses";
    }

    protected override void SendAnalytics()
    {
        int videoID = ChicagoSceneTransition.Instance.GetLastVideo() != null ? ChicagoSceneTransition.Instance.GetLastVideo().videoID : -1;
        EmotionQuestionnaireData questionnaireData = new EmotionQuestionnaireData(userID, headsetID, testTimestamp, videoID, responses[0], responses[1], responses[2], responses[3]);
        var data = new List<EmotionQuestionnaireData> { questionnaireData };

        AnalyticsUtilities.Event(ANALYTICS_TITLE, data);

        Analytics.CustomEvent(ANALYTICS_TITLE, new Dictionary<string, object>
        {
            { "UserID", questionnaireData.UserID},
            { "HeadsetID", questionnaireData.HeadsetID},
            { "TestTimestamp", questionnaireData.TestTimestamp},
            { "VideoID", questionnaireData.VideoID},
            { "Happy", questionnaireData.Happy},
            { "Sad", questionnaireData.Sad},
            { "Angry", questionnaireData.Angry},
            { "Anxious", questionnaireData.Anxious}
        });
    }
}

public class EmotionQuestionnaireData
{
    public string UserID { get; set; }
    public int HeadsetID { get; set; }
    public string TestTimestamp { get; set; }
    public int VideoID { get; set; }
    public int Happy { get; set; }
    public int Sad { get; set; }
    public int Angry { get; set; }
    public int Anxious { get; set; }

    public EmotionQuestionnaireData(string userID, int headsetID, string testTimestamp, int videoID, int happy, int sad, int angry, int anxious)
    {
        UserID = userID;
        HeadsetID = headsetID;
        TestTimestamp = testTimestamp;
        VideoID = videoID;
        Happy = happy;
        Sad = sad;
        Angry = angry;
        Anxious = anxious;
    }
}