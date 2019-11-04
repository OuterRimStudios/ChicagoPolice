using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public OVROverlay clip1;
    public OVROverlay clip2;
    bool toggle;
    bool acceptInput = true;

    //long frame;

    void Update()
    {
        OVRInput.Update();

        if (acceptInput && OVRInput.Get(OVRInput.Button.One))
        {
            acceptInput = false;
            toggle = !toggle;
            if (!toggle)
            {
                clip1.colorScale = Color.white;
                clip2.colorScale = Color.clear;
            }
            else
            {
                clip1.colorScale = Color.clear;
                clip2.colorScale = Color.white;
            }
            StartCoroutine(ResetInput());
        }
    }

    IEnumerator ResetInput()
    {
        yield return new WaitForSeconds(0.5f);
        acceptInput = true;
    }
}
