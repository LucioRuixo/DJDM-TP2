using UnityEngine.SceneManagement;

public class UIManager_MainMenu : UIManager
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    public void GoToGameplay()
    {
        SceneManager.LoadScene(SceneNameManager.Get().Gameplay);
    }

    public void DisplayHighscore()
    {
        HighscorePlugin.Get().ShowHighscoreDialog();
    }
}