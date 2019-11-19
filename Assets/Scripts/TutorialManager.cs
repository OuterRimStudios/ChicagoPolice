using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    void Update()
    {
        if (ChicagoSceneTransition.Instance.GoNext)
            ChicagoSceneTransition.Instance.NextScene();
    }
}
