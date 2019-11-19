using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class MenuManager : MonoBehaviour
{
    void Update()
    {
        if (ChicagoSceneTransition.Instance.GoNext)
            ChicagoSceneTransition.Instance.NextScene();
    }
}
