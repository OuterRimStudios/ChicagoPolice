using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    public Transform raycastPosition;
    public Transform handPoint;
    public LayerMask interactionLayer;
    LineRenderer line;
    Selectable selectable;
    public GraphicRaycaster graphicRaycaster;
    EventSystem eventSystem;
    PointerEventData pointer;

    private void OnEnable()
    {
        eventSystem = EventSystem.current;
        OVRInputManager.OnButtonDown += OnButtonDown;
    }

    private void OnDisable()
    {
        OVRInputManager.OnButtonDown -= OnButtonDown;
    }

    private void OnButtonDown(OVRInput.Button key)
    {
        if (key == OVRInput.Button.SecondaryIndexTrigger && selectable != null)
        {
            BaseEventData data = new BaseEventData(eventSystem);
            ExecuteEvents.Execute(selectable.gameObject, data, ExecuteEvents.submitHandler);
        }
    }

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        CastRay();
    }

    void CastRay()
    {
        RaycastHit hit;
        Ray ray = new Ray(raycastPosition.position, raycastPosition.forward);

        line.enabled = true;
        line.SetPosition(0, raycastPosition.position);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactionLayer))
        {
            Debug.LogError(hit.transform.name);
            if (hit.transform.tag.Equals("UI"))
            {
                line.SetPosition(1, hit.point);
                selectable = hit.transform.GetComponent<Selectable>();                
            }
        }
        else
        {
            line.SetPosition(1, ray.GetPoint(20));
            selectable = null;
        }
    }
}