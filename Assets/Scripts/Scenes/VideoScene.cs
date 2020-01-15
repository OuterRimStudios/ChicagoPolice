using System.IO;
using System.Collections;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using Hear360;
using TMPro;

public class VideoScene : BaseScene
{
    public MediaPlayer mediaPlayer;
    public string videoPath;
    public GameObject videoSphere;
    
    public AudioSource headlockedSource;
    public GameObject moodSlider;
    public bool trackingEnabled;
    public int videoID;
    public float audioDelay;

    public EightBallAudioController eightBallAudioController;

    [Space, Header("Countdown Variables")]
    public TextMeshProUGUI countdownText;
    public int countdownLength;
    public Material fadeMaterial;
    public GameObject countdownOBJ;
    public float fadeSpeed;
    public float dannyEntrance;
    public Collider dannyCollider;
    public GameObject perspectiveColliders;

    Coroutine countdownRoutine;
    int currentCount;

    //starts the countdown and loads the video
    public override void StartScene()
    {
        countdownRoutine = StartCoroutine(Countdown());
        //mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, Path.Combine(Application.persistentDataPath, videoPath), false);
    }

    void BeginVideo()
    {
        if(dannyCollider != null)
            StartCoroutine(DannyEntrance());
        if(perspectiveColliders != null)
            perspectiveColliders.SetActive(true);

        //ensures the fade is clear so it's not blocking the view
        fadeMaterial.SetColor("_Color", Color.clear);

        countdownOBJ.SetActive(false);
        StopCoroutine(countdownRoutine);

        //enables/plays the video and enables the mood slider
        videoSphere.SetActive(true);
        moodSlider.SetActive(true);
        mediaPlayer.Control.Play();
        StartCoroutine(IsPlaying());
    }

    IEnumerator IsPlaying()
    {
        //wait for the video to start playing before proceeding
        yield return new WaitUntil(() => mediaPlayer.Control.IsPlaying());
        //wait for the audio delay before proceeding
        yield return new WaitForSeconds(audioDelay);
        //playing the audio
        eightBallAudioController.Play();
        headlockedSource?.Play();
        yield return new WaitForSeconds(1);
        //wait for the video to finish playing before moving to the next scene
        yield return new WaitUntil(() => !mediaPlayer.Control.IsPlaying());
        ChicagoSceneTransition.Instance.NextScene();
    }

    //resets all of the scene objects associated with this video and unloads the video from memory
    public override void EndScene()
    {
        if (perspectiveColliders != null)
            perspectiveColliders.SetActive(false);
        if (dannyCollider != null)
            dannyCollider.enabled = false;
        mediaPlayer.Pause();
        videoSphere.SetActive(false);
        moodSlider.SetActive(false);
        mediaPlayer.CloseVideo();
    }

    //displays a countdown on screen and fades to black when the countdown reaches 0
    IEnumerator Countdown()
    {
        currentCount = countdownLength;
        countdownText.text = countdownLength.ToString();
        countdownOBJ.SetActive(true);
        CenterView();
        while (currentCount > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            currentCount--;
            countdownText.text = currentCount.ToString();
        }
        yield return new WaitUntil(Fade);
        BeginVideo();
    }

    //waits for Danny to enter the room in the video before enabling his collider for perception tracking
    IEnumerator DannyEntrance()
    {
        if (dannyCollider != null)
        {
            yield return new WaitForSeconds(dannyEntrance);
            dannyCollider.enabled = true;
        }
        else
            yield break;
    }

    bool Fade()
    {
        if (Mathf.Abs(fadeMaterial.GetColor("_Color").a - 1) <= 0.01f)
        {
            fadeMaterial.SetColor("_Color", Color.black);
            return true;
        }
        else
        {
            fadeMaterial.SetColor("_Color", Color.Lerp(fadeMaterial.GetColor("_Color"), Color.black, fadeSpeed * Time.deltaTime));
            return false;
        }
    }

    //calls normal CenterView, but also centers the countdown text
    protected override void CenterView()
    {
        base.CenterView();
        if(centerTarget)
            countdownOBJ.transform.rotation = Quaternion.Euler(countdownOBJ.transform.rotation.eulerAngles.x, centerTarget.rotation.eulerAngles.y, countdownOBJ.transform.rotation.eulerAngles.z);
    }
}
