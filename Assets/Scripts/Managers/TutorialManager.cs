using System.Collections;
using System.Collections.Generic;
using OuterRimStudios.Utilities;
using UnityEngine;
using System.Linq;

public class TutorialManager : MonoBehaviour
{
    public SceneType sceneType = SceneType.Chicago;
    public OVRInput.Button acceptedInput;
    public Animator controllerAnimator;
    public ControllerAnimation controllerAnimation;

    List<OVRInput.Button> activeInput = new List<OVRInput.Button>();

    private void Start()
    {
        IEnumerable<OVRInput.Button> enumerable = OVRInputManager.Instance.GetUniqueFlags(acceptedInput);
        activeInput = enumerable.ToList();
    }

    private void OnEnable()
    {
        OVRInputManager.OnButtonDown += OnButtonDown;

        controllerAnimator.ResetTrigger("none");
        controllerAnimator.SetTrigger(controllerAnimation.ToString());
    }

    private void OnDisable()
    {
        OVRInputManager.OnButtonDown -= OnButtonDown;

        controllerAnimator.SetTrigger("none");
    }
#if UNITY_EDITOR || UNITY_STANDALONE
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextScene();
        }
    }
#endif
#if UNITY_ANDROID
    void OnButtonDown(OVRInput.Button button)
    {
        if (activeInput.Contains(button))
        {
            NextScene();
        }
    }
#endif
    void NextScene()
    {
        if (sceneType == SceneType.Chicago)
            ChicagoSceneTransition.Instance.NextScene();
        else
            SundanceSceneTransition.Instance.NextScene();
    }
}

public enum ControllerAnimation
{
    joystick,
    xButton,
    yButton,
    xyButtons,
    none
};
