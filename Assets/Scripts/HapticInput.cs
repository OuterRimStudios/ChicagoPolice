using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HapticInput : MonoBehaviour
{
    public XRNode controllerInput = XRNode.LeftHand;
    [Range(0, 1f)]
    public float amplitude = .2f;
    [Range(0, 1f)]
    public float duration = .2f;

    public void PerformHapticRumble()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerInput);
        HapticCapabilities capabilities;
        if (device.TryGetHapticCapabilities(out capabilities))
        {
            if (capabilities.supportsImpulse)
            {
                uint channel = 0;
                device.SendHapticImpulse(channel, amplitude, duration);
            }
        }
        else
        {
            Debug.LogError("These controllers don't actually rumble");
        }
    }
}

