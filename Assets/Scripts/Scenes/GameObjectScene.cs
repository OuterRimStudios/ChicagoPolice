using UnityEngine;

public class GameObjectScene : BaseScene
{
    [Tooltip("The transform that this object will rotate to center on")]
    public Transform centerTarget;

    public override void StartScene()
    {
        gameObject.SetActive(true);
        CenterView();
    }

    public override void EndScene()
    {
        gameObject.SetActive(false);
    }

    void CenterView()
    {
        if(centerTarget)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, centerTarget.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
