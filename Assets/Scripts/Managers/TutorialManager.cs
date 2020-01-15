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
#if UNITY_ANDROID
        OVRInputManager.OnButtonDown += OnButtonDown;
#endif
        if (controllerAnimator)
        {
            controllerAnimator.ResetTrigger("none");
            controllerAnimator.SetTrigger(controllerAnimation.ToString());
        }
    }

    private void OnDisable()
    {
#if UNITY_ANDROID
        OVRInputManager.OnButtonDown -= OnButtonDown;
#endif
        if(controllerAnimator)
            controllerAnimator.SetTrigger("none");
    }
#if UNITY_EDITOR || UNITY_STANDALONE
    private void Update()
    {
        if (acceptedInput != OVRInput.Button.None)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextScene();
            }
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
