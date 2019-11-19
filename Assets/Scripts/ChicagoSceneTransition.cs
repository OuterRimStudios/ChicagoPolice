using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChicagoSceneTransition : MonoBehaviour
{
    public delegate void SceneEvents(BaseScene baseScene);
    public static event SceneEvents OnSceneStarted;

    public static ChicagoSceneTransition Instance;

    public List<BaseScene> testA;
    public List<BaseScene> testB;

    public bool GoNext { get; private set; }

    int sceneIndex;
    bool isTestB;
    bool acceptInput = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        OVRInput.Update();
        if (sceneIndex == 0 && OVRInput.GetDown(OVRInput.Button.One))
            isTestB = false;
        else if (sceneIndex == 0 && OVRInput.GetDown(OVRInput.Button.Two))
            isTestB = true;

        if(acceptInput && OVRInput.Get(OVRInput.Button.Three))
        {
            GoNext = true;
            acceptInput = false;
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        GoNext = false;
        yield return new WaitForSeconds(1);
        acceptInput = true;
    }

    public void NextScene()
    {
        List<BaseScene> baseScene = GetActiveScene();
        baseScene[sceneIndex].EndScene();

        if (sceneIndex < baseScene.Count)
            sceneIndex++;
        else
            sceneIndex = 0;

        baseScene[sceneIndex].StartScene();
        OnSceneStarted?.Invoke(baseScene[sceneIndex]);
    }

    List<BaseScene> GetActiveScene()
    {
        return isTestB ? testB : testA;
    }
}
