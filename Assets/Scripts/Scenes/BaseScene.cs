using UnityEngine;

public class BaseScene : MonoBehaviour
{
    public bool rotateToView = true;
    [ConditionalHide("rotateToView", true), Tooltip("The transform that this object will rotate to center on")] 
    public Transform centerTarget;
    public virtual void StartScene() { }
    public virtual void EndScene() { }
    protected virtual void CenterView()
    {
        if (centerTarget)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, centerTarget.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
