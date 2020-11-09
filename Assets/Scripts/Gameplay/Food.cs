using System;
using UnityEngine;

public class Food : MonoBehaviour
{
    bool cut = false;

    [SerializeField] int necessaryCuts = 1;
    int cuts = 0;

    float height;
    float minY;

    [SerializeField] long cutVibrationDuration = 100;

    [SerializeField] new ParticleSystem particleSystem = null;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody;
    new Collider2D collider;
    FoodGenerator.FruitPool pool;

    static public event Action OnCut;
    static public event Action OnUncut;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        height = spriteRenderer.bounds.size.y;
        minY = Camera.main.ScreenToWorldPoint(Vector2.zero).y - height;
    }

    void Update()
    {
        if (!cut && Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if (collider == Physics2D.OverlapPoint(touchPosition))
            {
                cuts++;
                if (cuts >= necessaryCuts) GetCut();
            }
        }

        if (transform.position.y < minY)
        {
            FallOffScreen();
            return;
        }
    }

    void FallOffScreen()
    {
        if (!cut) OnUncut?.Invoke();

        Despawn();
    }

    void GetCut()
    {
        cut = true;

        Vibration.Vibrate(cutVibrationDuration);

        Despawn();
        OnCut?.Invoke();
    }

    void Despawn()
    {
        cut = false;
        cuts = 0;

        pool.DespawnFruit(gameObject);
    }

    public void SetPool(FoodGenerator.FruitPool _pool)
    {
        pool = _pool;
    }

    public void SetFall(float force, Vector2 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        rigidBody.AddForce(transform.up * force, ForceMode2D.Impulse);
    }
}