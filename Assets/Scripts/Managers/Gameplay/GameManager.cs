using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int fruitBarValue;
    int cutFruitCount = 0;

    float score = 0f;

    [SerializeField] long loseVibrationDuration = 500;

    [SerializeField] FinalScoreContainer finalScoreContainer = null;

    static public event Action<float> OnScoreUpdate;
    static public event Action<float> OnFruitBarUpdate;
    static public event Action<float> OnDifficultyIncrease;

    [Header("Difficulty Increment: ")]
    [SerializeField] float scoreIncrease = 1f;

    [Header("Difficulty Increment: ")]
    [SerializeField] int difficultyIncrementThreshold = 1;
    [SerializeField] float impulseIncrease = 1f;

    [Header("Losing Condition: ")]
    [SerializeField] int fallenFruitLimit = 1;
    [SerializeField] int fallenFruitDecrease = 1;
    [SerializeField] int cutFruitIncrease = 1;

    void OnEnable()
    {
        Food.OnCut += IncreaseScore;
        Food.OnCut += IncreaseFruitBar;
        Food.OnCut += IncreaseCutFruitCount;
        Food.OnUncut += DecreaseFruitBar;
    }

    void OnDisable()
    {
        Food.OnCut -= IncreaseScore;
        Food.OnCut -= IncreaseFruitBar;
        Food.OnCut -= IncreaseCutFruitCount;
        Food.OnUncut -= DecreaseFruitBar;
    }

    void Start()
    {
        fruitBarValue = fallenFruitLimit;
    }

    void IncreaseScore()
    {
        score += scoreIncrease;

        OnScoreUpdate?.Invoke(score);
    }

    void IncreaseCutFruitCount()
    {
        cutFruitCount++;
        if (cutFruitCount >= difficultyIncrementThreshold)
            OnDifficultyIncrease?.Invoke(impulseIncrease);
    }

    void IncreaseFruitBar()
    {
        if (fruitBarValue < fallenFruitLimit)
        {
            fruitBarValue += cutFruitIncrease;
            if (fruitBarValue > fallenFruitLimit) fruitBarValue = fallenFruitLimit;

            float normalizedValue = NormalizeBarValue(fruitBarValue);
            OnFruitBarUpdate?.Invoke(normalizedValue);
        }
    }

    void DecreaseFruitBar()
    {
        fruitBarValue -= fallenFruitDecrease;
        if (fruitBarValue <= 0)
        {
            if (fruitBarValue < 0) fruitBarValue = 0;

            Lose();
        }

        float normalizedValue = NormalizeBarValue(fruitBarValue);
        OnFruitBarUpdate?.Invoke(normalizedValue);
    }

    float NormalizeBarValue(float barValue)
    {
        return barValue / fallenFruitLimit;
    }

    void Lose()
    {
        finalScoreContainer.FinalScore = score;
        Vibration.Vibrate(loseVibrationDuration);

        SceneManager.LoadScene(SceneNameManager.Get().ScoreScreen);
    }
}