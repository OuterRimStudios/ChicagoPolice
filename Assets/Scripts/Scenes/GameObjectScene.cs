using UnityEngine;

public class GameObjectScene : BaseScene
{
    public override void StartScene()
    {
        gameObject.SetActive(true);
        CenterView();
    }

    public override void EndScene()
    {
        gameObject.SetActive(false);
    }
}
