using UnityEngine;

public class GameField : MonoBehaviour
{
    private Transform _tr = null;

    void Awake()
    {
        _tr = transform;
    }

    public void Fit(Bounds bounds)
    {
        _tr.localScale = new Vector3(bounds.size.x / 10f, bounds.size.y, 1f); // 10 bacause width of Plane is 10 units
    }
}