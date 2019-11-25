using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using OuterRimStudios.Utilities;

public class OVRInputManager : MonoBehaviour
{
    public static OVRInputManager Instance;
    public delegate void ButtonEvents(OVRInput.Button key);
    public static event ButtonEvents OnButtonDown;
    public static event ButtonEvents OnButtonUp;

    public OVRInput.Button activeButtons;
    public float onButtonDownWaitTime = .1f;

    //Value refers to the accept input state. If its true that but can be pressed.
    Dictionary<OVRInput.Button, bool> inputStates = new Dictionary<OVRInput.Button, bool>();
    List<OVRInput.Button> buttons = new List<OVRInput.Button>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        IEnumerable<OVRInput.Button> enumerable = GetUniqueFlags(activeButtons);
        buttons = enumerable.ToList();

        foreach (OVRInput.Button button in buttons)
            inputStates.Add(button, true);
    }

    private void Update()
    {
        foreach (OVRInput.Button button in buttons)
        {
            if (inputStates[button] && OVRInput.Get(button))
            {
                inputStates[button] = false;
                OnButtonDown?.Invoke(button);
            }
            else if (!inputStates[button] && !OVRInput.Get(button))
            {
                inputStates[button] = true;
                OnButtonUp?.Invoke(button);
            }
        }
    }

    public IEnumerable<OVRInput.Button> GetUniqueFlags(Enum flags)
    {
        int flag = 1;
        foreach (var value in Enum.GetValues(flags.GetType()).Cast<OVRInput.Button>())
        {
            long bits = Convert.ToInt64(value);
            while (flag < bits)
                flag <<= 1;

            if (flag == bits && flags.HasFlag(value))
                yield return value;
        }
    }
}
