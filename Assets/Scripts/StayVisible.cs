using UnityEngine;

public class StayVisible : MonoBehaviour
{
    public Transform target;
    public bool centerOnce;

    private void Start()
    {
        if(centerOnce && target)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, target.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    void Update()
    {
        if (target && !centerOnce)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, target.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}