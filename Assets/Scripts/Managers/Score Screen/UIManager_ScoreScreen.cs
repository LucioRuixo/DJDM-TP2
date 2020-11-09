using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager_ScoreScreen : UIManager
{
    [SerializeField] Text finalScore = null;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        finalScore.text = "Puntaje final: " + FinalScoreContainer.Get().FinalScore;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(SceneNameManager.Get().MainMenu);
    }
}