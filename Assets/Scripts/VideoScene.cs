using RenderHeads.Media.AVProVideo;
public class VideoScene : BaseScene
{
    public MediaPlayer mediaPlayer;
    public override void StartScene()
    {
        mediaPlayer.Play();
    }

    public override void EndScene()
    {
        mediaPlayer.Stop();
    }
}
