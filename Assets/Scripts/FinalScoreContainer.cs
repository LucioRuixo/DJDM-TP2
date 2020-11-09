using UnityEngine.SceneManagement;

public class FinalScoreContainer : MonoBehaviourSingleton<FinalScoreContainer>
{
    public float FinalScore { set; get; }

    public override void Awake()
    {
        base.Awake();

        SceneManager.sceneLoaded += CheckLoadedScene;
        SceneManager.sceneUnloaded += CheckUnloadedScene;
    }

    void CheckLoadedScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SceneNameManager.Get().ScoreScreen)
        {
            if (FinalScore > HighscorePlugin.Get().GetHighscore())
                HighscorePlugin.Get().SetHighscore(FinalScore);
        }
    }

    void CheckUnloadedScene(Scene scene)
    {
        if (scene.name == SceneNameManager.Get().ScoreScreen)
            Destroy(gameObject);
    }
}