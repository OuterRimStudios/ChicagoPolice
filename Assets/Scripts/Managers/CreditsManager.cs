using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    public SceneType sceneType = SceneType.Sundance;
    public Animator cameraAnimator;

    void OnEnable()
    {
        cameraAnimator.SetTrigger("fadeWhite");
        StartCoroutine(StartAnimation());
    }

    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        //yield return new WaitUntil(HasAnimationFinished);
        GetComponent<Animator>().SetTrigger("startCredits");
    }

    bool HasAnimationFinished()
    {
        return cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("FadeWhite");
    }

    public void SetCameraToBlue()
    {
        cameraAnimator.SetTrigger("fadeBlue");
    }

    public void ResetCredits()
    {       
        cameraAnimator.ResetTrigger("fadeWhite");
        GetComponent<Animator>().ResetTrigger("startCredits");

        if (sceneType == SceneType.Chicago)
            ChicagoSceneTransition.Instance.NextScene();
        else
            SundanceSceneTransition.Instance.NextScene();
    }
}
