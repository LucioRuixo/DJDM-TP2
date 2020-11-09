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

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody;
    new Collider2D collider;

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
                if (cuts >= necessaryCuts) Cut();
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

        Destroy(gameObject);
    }

    void Cut()
    {
        spriteRenderer.color = Color.black;
        cut = true;

        Vibration.Vibrate(cutVibrationDuration);

        OnCut?.Invoke();
    }

    public void SetFall(float force, Vector2 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        rigidBody.AddForce(transform.up * force, ForceMode2D.Impulse);
    }
}