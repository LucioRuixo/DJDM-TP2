using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGenerator : MonoBehaviour
{
    [Serializable]
    public class FruitPool
    {
        public Queue<GameObject> queue = new Queue<GameObject>();

        public GameObject SpawnFruit(Vector3 position, Quaternion rotation)
        {
            GameObject newFruit = queue.Dequeue();

            newFruit.transform.position = position;
            newFruit.transform.rotation = rotation;

            newFruit.SetActive(true);
            return newFruit;
        }

        public void DespawnFruit(GameObject fruit)
        {
            fruit.SetActive(false);
            queue.Enqueue(fruit);
        }
    }

    [SerializeField] bool generationActive = true;

    float impulse;

    [Header("Fruit Pools: ")]
    [SerializeField] int fruitsPerPool = 1;

    [SerializeField] List<GameObject> fruitPrefabs = new List<GameObject>();
    List<FruitPool> fruitPools = new List<FruitPool>();

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
        UIManager_Gameplay.OnGameplayStart += StartGeneration;
    }

    void Start()
    {
        foreach (GameObject fruitPrefab in fruitPrefabs)
        {
            FruitPool newPool = new FruitPool();
            GameObject fruitContainer = new GameObject(fruitPrefab.name + " Container");
            fruitContainer.transform.SetParent(transform);

            for (int i = 0; i < fruitsPerPool; i++)
            {
                Food newFruit = Instantiate(fruitPrefab, fruitContainer.transform).GetComponent<Food>();
                newFruit.SetPool(newPool);

                newPool.queue.Enqueue(newFruit.gameObject);
                newFruit.gameObject.SetActive(false);
            }

            fruitPools.Add(newPool);
        }

        impulse = initialImpulse;
    }

    void OnDisable()
    {
        GameManager.OnDifficultyIncrease -= IncreaseImpulse;
        UIManager_Gameplay.OnGameplayStart -= StartGeneration;
    }

    void StartGeneration()
    {
        StartCoroutine(Generate());
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
            position.x = UnityEngine.Random.Range(minX, maxX);

            float addedAngle = UnityEngine.Random.Range(-(angleRange / 2f), angleRange / 2f);
            Quaternion addedRotation = Quaternion.Euler(0f, 0f, addedAngle);
            Quaternion rotation = transform.rotation * addedRotation;

            int poolIndex = UnityEngine.Random.Range(0, fruitPools.Count - 1);
            Food newFood = fruitPools[poolIndex].SpawnFruit(position, rotation).GetComponent<Food>();
            newFood.SetFall(impulse, position, rotation);

            float waitTime = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}