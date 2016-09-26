using UnityEngine;

public class GameScreen
{
    public Bounds Bounds { get; private set; }

    public GameScreen(Camera gameCamera)
    {
        Vector3 center = gameCamera.transform.position;

        Vector2 size = new Vector2();
        size.y = gameCamera.orthographicSize * 2f;
        float screenAspect = ((float)Screen.width / Screen.height);
        size.x = screenAspect * size.y;

        Bounds = new Bounds(center, size);
    }
}