﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using OuterRimStudios.Utilities;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    public OVRInput.Button acceptedInput;
    List<OVRInput.Button> activeInput = new List<OVRInput.Button>();

    private void Start()
    {
        IEnumerable<OVRInput.Button> enumerable = OVRInputManager.Instance.GetUniqueFlags(acceptedInput);
        activeInput = enumerable.ToList();
    }

    private void OnEnable()
    {
        OVRInputManager.OnButtonDown += OnButtonDown;
    }

    private void OnDisable()
    {
        OVRInputManager.OnButtonDown -= OnButtonDown;
    }

    void OnButtonDown(OVRInput.Button button)
    {
        if (activeInput.Contains(button))
            ChicagoSceneTransition.Instance.NextScene();
    }
}
