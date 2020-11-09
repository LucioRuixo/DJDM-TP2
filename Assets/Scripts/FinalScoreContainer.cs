using UnityEngine;
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
        if (scene.name == SceneNameManager.Get().ScoreScreen && FinalScore > HighscorePlugin.Get().GetHighscore())
        {
            if (Application.platform == RuntimePlatform.Android)
                HighscorePlugin.Get().SetHighscore(FinalScore);
            else 
                Debug.LogWarning("Highscore could not be saved because application platform is not Android4");
        }
    }

    void CheckUnloadedScene(Scene scene)
    {
        if (scene.name == SceneNameManager.Get().ScoreScreen)
            Destroy(gameObject);
    }
}