using System;
using UnityEngine;

public class BombsController
{
    private Camera _camera = null;

    public BombsController(Camera gameCamera)
    {
        _camera = gameCamera;
    }

    public bool Spawn(Vector2 position, float explodeDelay, Action<UnitBase[]> onBombExplodeHandler)
    {
        Ray ray = _camera.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("GameField"))
            {
                BombBase bomb = BombsFactory.Create(BombType.Simple);
                bomb.Init(hit.point, explodeDelay, onBombExplodeHandler);
                bomb.Begin();

                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
