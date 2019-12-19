using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public int dataRate = 5;
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
        while (true)
        {
            //wait the data rate before requesting the data again
            yield return new WaitForSeconds(dataRate);
            try
            {
                //downloads the data from the hosted file and tries to format it into a string
                byte[] newFileData = request.DownloadData(url);
                string fileString = System.Text.Encoding.UTF8.GetString(newFileData);
                //formatting the data from the hosted file into the type TestGroup so it can be used easier
                data = JsonUtility.FromJson(fileString, typeof(TestGroup)) as TestGroup;

                //check if the hosted file has given the signal to start the experience
                if (data.canStart)
                {
                    //setting the values for this user from the data pulled from the hosted file
                    string userID = data.users[ChicagoSceneTransition.Instance.HeadsetID - 1].userID;
                    string groupID = data.users[ChicagoSceneTransition.Instance.HeadsetID - 1].groupID;
                    ChicagoSceneTransition.Instance.InitializeUser(userID, groupID);
                    data.users[ChicagoSceneTransition.Instance.HeadsetID - 1].questReady = true;
                    //post back to the file that this user is ready
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

    //posts data to hosted file
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