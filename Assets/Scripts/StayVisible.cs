using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayVisible : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, target.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }
}
