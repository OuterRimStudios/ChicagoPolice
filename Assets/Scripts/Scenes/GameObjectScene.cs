using UnityEngine;

public class GameObjectScene : BaseScene
{
    //turns on this gameobject and centers the rotation if it has a centerTarget
    public override void StartScene()
    {
        gameObject.SetActive(true);
        CenterView();       //(reference BaseScene for more info)
    }

    public override void EndScene()
    {
        gameObject.SetActive(false);
    }
}
