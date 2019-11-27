using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public virtual void NextScene() { }
}

public enum SceneType
{
    Chicago,
    Sundance
};