using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public int countdownLength;
    public Material fadeMaterial;
    public GameObject fadeSphere;
    public float fadeSpeed;

    Coroutine countdownRoutine;
    int currentCount;

    private void OnEnable()
    {
        countdownRoutine = StartCoroutine(Countdown());
    }

    private void OnDisable()
    {
        fadeMaterial.SetColor("_Color", Color.clear);
        StopCoroutine(countdownRoutine);
    }

    IEnumerator Countdown()
    {
        currentCount = countdownLength;
        countdownText.text = countdownLength.ToString();
        while(currentCount > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            currentCount--;
            countdownText.text = currentCount.ToString();
        }
        yield return new WaitUntil(Fade);
        ChicagoSceneTransition.Instance.NextScene();
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