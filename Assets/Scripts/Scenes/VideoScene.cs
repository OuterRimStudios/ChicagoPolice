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

    public override void StartScene()
    {
        countdownRoutine = StartCoroutine(Countdown());
        mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, Path.Combine(Application.persistentDataPath, videoPath), false);
    }

    void BeginVideo()
    {
        StartCoroutine(DannyEntrance());
        perspectiveColliders.SetActive(true);
        fadeMaterial.SetColor("_Color", Color.clear);
        countdownOBJ.SetActive(false);
        StopCoroutine(countdownRoutine);
        videoSphere.SetActive(true);
        moodSlider.SetActive(true);
        mediaPlayer.Control.Play();
        StartCoroutine(IsPlaying());
    }

    IEnumerator IsPlaying()
    {
        yield return new WaitUntil(() => mediaPlayer.Control.IsPlaying());
        yield return new WaitForSeconds(audioDelay);
        eightBallAudioController.Play();
        headlockedSource?.Play();
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => !mediaPlayer.Control.IsPlaying());
        ChicagoSceneTransition.Instance.NextScene();
    }

    public override void EndScene()
    {
        perspectiveColliders.SetActive(false);
        dannyCollider.enabled = false;
        mediaPlayer.Pause();
        videoSphere.SetActive(false);
        moodSlider.SetActive(false);
        mediaPlayer.CloseVideo();
    }

    IEnumerator Countdown()
    {
        currentCount = countdownLength;
        countdownText.text = countdownLength.ToString();
        countdownOBJ.SetActive(true);
        while (currentCount > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            currentCount--;
            countdownText.text = currentCount.ToString();
        }
        yield return new WaitUntil(Fade);
        BeginVideo();
    }

    IEnumerator DannyEntrance()
    {
        yield return new WaitForSeconds(dannyEntrance);
        dannyCollider.enabled = true;
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
}
