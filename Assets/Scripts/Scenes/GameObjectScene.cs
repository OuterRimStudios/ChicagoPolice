using UnityEngine;

public class GameObjectScene : BaseScene
{
    public override void StartScene()
    {
        gameObject.SetActive(true);
    }

    public override void EndScene()
    {
        gameObject.SetActive(false);
    }
}
