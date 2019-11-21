using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

public class VideoManager : MonoBehaviour
{
    public float audioDelay = 1;
    public float switchTimer = 120f;

    [Tooltip("The X value refers to the start of the safe time in seconds. The Y value refers to the length of time the user has to switch perspectives.")]
    public Vector2[] safeTimers;
    const float MARGIN_OF_ERROR = 1000f;
    public Slider timerBar;

    [Space]
    public MediaPlayer playerOne;
    public MeshRenderer sphereOne;
    public string path1;
    public AudioSource source1;

    [Space]
    public MediaPlayer playerTwo;
    public MeshRenderer sphereTwo;
    public string path2;
    public AudioSource source2;

    [Space]
    public MediaPlayer interview;
    public MeshRenderer interviewSphere;
    public string path3;
    public AudioSource source3;

    bool acceptInput = true;
    bool allowSwitch = true;
    int safeTimerIndex = 0;
    float timerTime;

    private void Awake()
    {
        //playerOne.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, Path.Combine(Application.persistentDataPath, path1), true);
        //playerTwo.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, Path.Combine(Application.persistentDataPath, path2), false);
        //interview.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, Path.Combine(Application.persistentDataPath, path3), false);
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => (playerOne.Control.IsPlaying() || playerTwo.Control.IsPlaying()));
        yield return new WaitForSeconds(audioDelay);
        source1.Play();
        source2.Play();
        //yield return new WaitForSeconds(switchTimer);
        //allowSwitch = false;
    }

    void Update()
    {
        if (playerOne.Control.IsFinished() || playerTwo.Control.IsFinished())
        {
            sphereOne.enabled = false;
            sphereTwo.enabled = false;
            interview.Control.Play();
            interviewSphere.enabled = true;

            source3.Play();
        }

        MediaPlayer currentPlayer = playerOne.Control.IsPlaying() ? playerOne : playerTwo;
        if (Mathf.Abs((safeTimers[safeTimerIndex].x * 1000) - currentPlayer.Control.GetCurrentTimeMs()) < MARGIN_OF_ERROR)
        {
            timerBar.value = 0;
            timerBar.maxValue = safeTimers[safeTimerIndex].y;
            timerBar.gameObject.SetActive(true);
            StartCoroutine(TimerBar());
            allowSwitch = true;
            safeTimerIndex++;
        }

        if (!allowSwitch)
            return;

        OVRInput.Update();

        if (acceptInput && OVRInput.Get(OVRInput.Button.One))
        {
            acceptInput = false;
            if (playerOne.Control.IsPlaying() && !playerTwo.Control.IsFinished())
            {
                float time = playerOne.Control.GetCurrentTimeMs();
                playerOne.Control.Pause();
                sphereOne.enabled = false;
                playerTwo.Control.SeekFast(time);
                playerTwo.Control.Play();
                sphereTwo.enabled = true;

                source1.mute = true;
                source2.mute = false;
            }
            else if (playerTwo.Control.IsPlaying() && !playerOne.Control.IsFinished())
            {
                float time = playerTwo.Control.GetCurrentTimeMs();
                playerTwo.Control.Pause();
                sphereTwo.enabled = false;
                playerOne.Control.SeekFast(time);
                playerOne.Control.Play();
                sphereOne.enabled = true;

                source1.mute = false;
                source2.mute = true;
            }

            StartCoroutine(ResetInput());
        }
    }

    IEnumerator ResetInput()
    {
        yield return new WaitForSeconds(0.25f);
        acceptInput = true;
    }

    IEnumerator TimerBar()
    {
        timerTime = safeTimers[safeTimerIndex].y;
        yield return new WaitUntil(()=> Timer(ref timerTime));
        allowSwitch = false;
        timerBar.gameObject.SetActive(false);
        timerBar.value = 0;
    }

    public bool Timer(ref float currentTime)
    {
        if (currentTime > 0)
        {
            if (currentTime - Time.deltaTime > 0)
            {
                currentTime -= Time.deltaTime;
                timerBar.value += Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                timerBar.value = timerBar.maxValue;
            }

            return false;
        }
        else
            return true;
    }
}