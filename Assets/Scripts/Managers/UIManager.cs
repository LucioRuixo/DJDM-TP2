using UnityEngine;

public class UIManager : MonoBehaviour
{
    float cameraHeight;

    Vector2 cameraSize;

    GameObject backgroundContainer;
    SpriteRenderer backgroundSR;

    protected virtual void Awake()
    {
        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);

        backgroundContainer = GameObject.Find("Background");
        backgroundSR = backgroundContainer.GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        AdjustBackground();
    }

    void AdjustBackground()
    {
        backgroundContainer.transform.localScale = Vector3.one;
        Vector2 spriteSize = backgroundSR.sprite.bounds.size;
        backgroundContainer.transform.localScale *= cameraSize.x >= cameraSize.y ? cameraSize.x / spriteSize.x : cameraSize.y / spriteSize.y;
    }
}