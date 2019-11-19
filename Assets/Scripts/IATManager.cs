using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IATManager : MonoBehaviour
{
    void Update()
    {
        if (ChicagoSceneTransition.Instance.GoNext)
            ChicagoSceneTransition.Instance.NextScene();
    }
}
