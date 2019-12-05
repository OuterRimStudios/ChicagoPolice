using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OuterRimStudios.Utilities;


public class AppManager : MonoBehaviour
{
    string url = "ftp://ftpupload.net/htdocs/ProjectPerspective/appControl.json";
    NetworkCredential credential = new NetworkCredential("epiz_24876763", "Wr6f38F0XBubb");
    TestGroup data;
    Coroutine getData;

    //ManualInput
    public float holdTime = 1.5f;
    public GameObject manualInput;
    HapticInput hapticInput;
    private bool buttonHeld;
    private bool isManual;
    float timer;

    void Start()
    {
        manualInput.SetActive(false);
        hapticInput = GetComponent<HapticInput>();
    }

    void OnEnable()
    {
        getData = StartCoroutine(Get());
        OVRInputManager.OnButtonDown += OnButtonDown;
        OVRInputManager.OnButtonUp += OnButtonUp;
    }

    private void OnDisable()
    {
        StopCoroutine(getData);
        OVRInputManager.OnButtonDown -= OnButtonDown;
        OVRInputManager.OnButtonUp -= OnButtonUp;
    }

    private void Update()
    {
        CheckCountdown();
    }

    void OnButtonDown(OVRInput.Button button)
    {
        if (button == OVRInput.Button.Two || Input.GetKeyDown(KeyCode.B))
        {
            buttonHeld = true;
        }
    }

    void OnButtonUp(OVRInput.Button button)
    {
        if (button == OVRInput.Button.Two || Input.GetKeyUp(KeyCode.B))
        {
            buttonHeld = false;
        }            
    }

    void CheckCountdown()
    {
        if (buttonHeld)
        {
            if (MathUtilities.Timer(ref timer))
            {                
                isManual = true;
                gameObject.SetActive(false);
                manualInput.SetActive(true);
            }
            else
                hapticInput.PerformHapticRumble();

            //Debug.LogError("Check Countdown:");
        }
        else
            ResetTime();
    }

    void ResetTime()
    {
        timer = holdTime;
    }

    IEnumerator Get()
    {
        WebClient request = new WebClient();
        request.Credentials = credential;
        while (!isManual)
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