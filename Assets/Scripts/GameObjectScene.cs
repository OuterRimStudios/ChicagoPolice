using UnityEngine;

public class GameObjectScene : BaseScene
{
    public GameObject sceneObject;

    public override void StartScene()
    {
        sceneObject.SetActive(true);
    }

    public override void EndScene()
    {
        sceneObject.SetActive(false);
    }
}
