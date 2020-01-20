using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OuterRimStudios.Utilities;

public class BackstoryManager : MonoBehaviour
{
    const int PREVIOUS_BACKSTORY = -1;
    const int NEXT_BACKSTORY = 1;

    public List<BackstorySelector> backstorySelectors;

    public Transform cameraTransform;

    public BaseScene creditsScene;

    BackstorySelector currentBackstory;
    int currentBackstoryIndex;

    private void OnEnable()
    {
        OVRInputManager.OnButtonDown += OnButtonDown;
        backstorySelectors[0].gameObject.SetActive(true);
        currentBackstory = backstorySelectors[0];
        currentBackstoryIndex = 0;
    }

    private void OnDisable()
    {
        OVRInputManager.OnButtonDown -= OnButtonDown;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
            ChangeBackstory(PREVIOUS_BACKSTORY);
        if(Input.GetKeyDown(KeyCode.RightArrow))
            ChangeBackstory(NEXT_BACKSTORY);
    }

    private void OnButtonDown(OVRInput.Button key)
    {
        if (key == OVRInput.Button.Three)
        {
            currentBackstory.SelectBackground();
            gameObject.SetActive(false);
        }

        if (key == OVRInput.Button.Four)
        {
            SundanceSceneTransition.Instance.ChangeScene(creditsScene);
        }

        if (key == OVRInput.Button.PrimaryThumbstickLeft)        
            ChangeBackstory(PREVIOUS_BACKSTORY);
        
        if (key == OVRInput.Button.PrimaryThumbstickRight)
            ChangeBackstory(NEXT_BACKSTORY);
    }

    void ChangeBackstory(int direction)
    {
        if (direction == NEXT_BACKSTORY)
        {
            currentBackstoryIndex = currentBackstoryIndex.IncrementLoop(backstorySelectors.Count - 1);
        }
        else if (direction == PREVIOUS_BACKSTORY)
        {
            currentBackstoryIndex = currentBackstoryIndex.DecrementLoop(0, backstorySelectors.Count - 1);
        }

        if (currentBackstory != backstorySelectors[currentBackstoryIndex])
        {
            currentBackstory.gameObject.SetActive(false);
            currentBackstory = backstorySelectors[currentBackstoryIndex];
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, cameraTransform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            currentBackstory.gameObject.SetActive(true);
        }
    }
}
