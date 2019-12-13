using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayVisible : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 3f;
    public float rotationSpeed = 3f;

    Transform reference;

    void Update()
    {
        if (target)
        {
            reference.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); // only Y axis, meaning right & left turns.

            Vector3 v3_Dir = transform.position - target.position;
            float f_AngleBetween = Vector3.Angle(reference.forward, v3_Dir);
            transform.RotateAround(target.position, Vector3.up, f_AngleBetween);
        }
    }
}
