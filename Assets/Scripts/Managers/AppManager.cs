using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    string url = "ftp://ftpupload.net/htdocs/ProjectPerspective/appControl.json";
    NetworkCredential credential = new NetworkCredential("epiz_24876763", "Wr6f38F0XBubb");
    TestGroup data;
    Coroutine getData;

    void OnEnable()
    {
        getData = StartCoroutine(Get());
    }

    private void OnDisable()
    {
        StopCoroutine(getData);
    }

    IEnumerator Get()
    {
        WebClient request = new WebClient();
        request.Credentials = credential;
        for (; ; )
        {
            yield return new WaitForSeconds(5f);
            try
            {

                byte[] newFileData = request.DownloadData(url);
                string fileString = System.Text.Encoding.UTF8.GetString(newFileData);
                data = JsonUtility.FromJson(fileString, typeof(TestGroup)) as TestGroup;

                if (data.canStart)
                {
                    foreach (UserInfo page in data.users)
                    {
                        Debug.Log($"OculusID: {page.questID} | UserID: {page.userID} | GroupID: {page.groupID}");
                    }
                    string userID = data.users[ChicagoSceneTransition.Instance.HeadsetID - 1].userID;
                    string groupID = data.users[ChicagoSceneTransition.Instance.HeadsetID - 1].groupID;
                    ChicagoSceneTransition.Instance.InitializeUser(userID, groupID);
                    data.users[ChicagoSceneTransition.Instance.HeadsetID - 1].questReady = true;
                    Post();
                    yield break;
                }
            }
            catch (WebException e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    void Post()
    {
        WebClient client = new WebClient();
        client.Credentials = credential;
        client.Headers[HttpRequestHeader.ContentType] = "application/json";
        try
        {
            string jsonData = JsonUtility.ToJson(data);
            client.UploadString(url, jsonData);
        }
        catch (WebException e)
        {
            Debug.LogError(e.Message);
        }
    }
}

[System.Serializable]
public class TestGroup
{
    public bool canStart;
    public List<UserInfo> users;
}

[System.Serializable]
public class UserInfo
{
    public string questID;
    public string userID;
    public string groupID;
    public bool questReady;
}