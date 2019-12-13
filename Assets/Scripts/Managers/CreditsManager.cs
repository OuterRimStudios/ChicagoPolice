using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
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

    public void ResetCredits()
    {
        cameraAnimator.SetTrigger("fadeBlue");
        cameraAnimator.ResetTrigger("fadeWhite");
        GetComponent<Animator>().ResetTrigger("startCredits");
        ChicagoSceneTransition.Instance.NextScene();
    }
}
