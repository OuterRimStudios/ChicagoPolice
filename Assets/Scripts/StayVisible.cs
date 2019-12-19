using UnityEngine;

public class StayVisible : MonoBehaviour
{
    public Transform target;
    public bool centerOnce;

    //centers the objects rotation based on the target object
    private void Start()
    {
        if(centerOnce && target)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, target.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    //keeps the object rotated relative to the target
    void Update()
    {
        if (target && !centerOnce)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, target.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}