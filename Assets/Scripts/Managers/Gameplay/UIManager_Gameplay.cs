using UnityEngine;
using UnityEngine.UI;

public class UIManager_Gameplay : UIManager
{
    [SerializeField] Text score = null;
    [SerializeField] Slider bar = null;

    void OnEnable()
    {
        GameManager.OnScoreUpdate += UpdateScore;
        GameManager.OnFruitBarUpdate += UpdateBar;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        UpdateScore(0f);
    }

    void OnDisable()
    {
        GameManager.OnScoreUpdate -= UpdateScore;
        GameManager.OnFruitBarUpdate -= UpdateBar;
    }

    void UpdateScore(float newScore)
    {
        score.text = "Puntaje: " + newScore;
    }

    void UpdateBar(float newValue)
    {
        bar.value = newValue;
    }
}