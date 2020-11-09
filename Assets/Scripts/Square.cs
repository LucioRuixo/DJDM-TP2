using UnityEngine;

public class Square : MonoBehaviour
{
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        rect.Rotate(Vector3.forward, -1f);
    }
}