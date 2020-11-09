using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGenerator : MonoBehaviour
{
    [SerializeField] bool generationActive = true;

    float impulse;

    [SerializeField] List<GameObject> foodPrefabs = new List<GameObject>();

    [Header("Generation Properties: ")]
    [SerializeField] float initialImpulse = 1f;
    [SerializeField] float angleRange = 1f;

    [Header("Generation Wait Time: ")]
    [SerializeField] float initialWaitTime = 1f;
    [SerializeField] float minWaitTime = 1f;
    [SerializeField] float maxWaitTime = 1f;

    [Header("Initial Position: ")]
    [SerializeField] float minX = 1f;
    [SerializeField] float maxX = 1f;

    void OnEnable()
    {
        GameManager.OnDifficultyIncrease += IncreaseImpulse;
    }

    void Start()
    {
        impulse = initialImpulse;
        StartCoroutine(Generate());
    }

    void OnDisable()
    {
        GameManager.OnDifficultyIncrease += IncreaseImpulse;
    }

    void IncreaseImpulse(float impulseIncrease)
    {
        impulse += impulseIncrease;
    }

    IEnumerator Generate()
    {
        yield return new WaitForSeconds(initialWaitTime);

        while (generationActive)
        {
            Vector2 position = transform.position;
            position.x = Random.Range(minX, maxX);

            float addedAngle = Random.Range(-(angleRange / 2f), angleRange / 2f);
            Quaternion addedRotation = Quaternion.Euler(0f, 0f, addedAngle);
            Quaternion rotation = transform.rotation * addedRotation;

            GameObject prefab = foodPrefabs[Random.Range(0, foodPrefabs.Count)];
            Food newFood = Instantiate(prefab, position, rotation, transform).GetComponent<Food>();
            newFood.SetFall(impulse, position, rotation);

            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}