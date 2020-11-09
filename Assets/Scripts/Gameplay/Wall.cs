using UnityEngine;

public class Wall : MonoBehaviour
{
    enum Side
    {
        Left,
        Right
    }

    [SerializeField] Side side = Side.Left;

    void Awake()
    {
        float screenX = side == Side.Left ? 0f : Screen.width;
        Vector3 screenPosition = new Vector3(screenX, 0f, 10f);

        Vector2 position = Vector2.zero;
        position.x = Camera.main.ScreenToWorldPoint(screenPosition).x;
        transform.position = position;
    }
}