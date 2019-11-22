﻿using System.Collections;
using System.Collections.Generic;
using OuterRimStudios.Utilities;
using UnityEngine;
using System.Linq;

public class IATManager : MonoBehaviour
{
    public OVRInput.Button acceptedInput;
    public IATCollection[] iATCollection;
    public Round[] rounds;

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
        if(activeInput.Contains(button))
            ChicagoSceneTransition.Instance.NextScene();
    }
}
