using UnityEngine;
using System;
using System.Collections.Generic;

using Random = UnityEngine.Random;

public class UnitsController
{
    public event Action<UnitBase> OnUnitPassedBottomLine;
    public event Action<UnitBase> OnUnitKilled;

    private UnitsPool _pool = null;
    private List<UnitBase> _activeUnits = null;

    private Camera _camera = null;
    private float _halfOfGameScrWidth, _topGameScrPoint;

    public UnitsController(Camera gameCamera, Bounds gameScreenBounds)
    {
        _camera = gameCamera;

        _halfOfGameScrWidth = gameScreenBounds.size.x / 2f;
        _topGameScrPoint = gameScreenBounds.center.y + gameScreenBounds.extents.y;

        _activeUnits = new List<UnitBase>();
        _pool = new UnitsPool(new UnitType[]
            {
                UnitType.Forward,
                UnitType.Forward,
                UnitType.Forward,

                UnitType.ForwardDiagonal,
                UnitType.ForwardDiagonal,
                UnitType.ForwardDiagonal,

                UnitType.ForwardDiagonalFriend,
                UnitType.ForwardDiagonalFriend,
                UnitType.ForwardDiagonalFriend,
            });
    }

    public void ReturnAllToPool(bool onlyAlife)
    {
        for (int i = _activeUnits.Count - 1; i >= 0; i--)
        {
            if (onlyAlife)
            {
                if (_activeUnits[i].IsDead)
                {
                    continue;
                }
            }

            ReturnToPool(_activeUnits[i]);
        }
    }

    public void Spawn()
    {
        UnitType randomType = (UnitType)Random.Range((int)UnitType.begin + 1, (int)UnitType.end);
        UnitBase unit = _pool.Get(randomType);
        unit.Init(GetRandomPosition(), -_topGameScrPoint,
            unitPassedLine =>
            {
                if (OnUnitPassedBottomLine != null)
                {
                    OnUnitPassedBottomLine(unitPassedLine);
                }

                ReturnToPool(unitPassedLine);
            });

        _activeUnits.Add(unit);
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(-_halfOfGameScrWidth / 2f, _halfOfGameScrWidth / 2f);
        return new Vector3(randomX, 0f, _topGameScrPoint);
    }

    public void OnTouchReleased(Vector2 position)
    {
        Ray ray = _camera.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            UnitBase unit = hit.collider.GetComponent<UnitBase>();

            if (unit)
            {
                KillUnit(unit);
            }
        }
    }

    private void KillUnit(UnitBase unit)
    {
        unit.Dead(() => ReturnToPool(unit));

        if (OnUnitKilled != null)
        {
            OnUnitKilled(unit);
        }
    }

    public void KillUnits(UnitBase[] units)
    {
        for (int i = 0; i < units.Length; i++)
        {
            KillUnit(units[i]);
        }
    }

    private void ReturnToPool(UnitBase unit)
    {
        _activeUnits.Remove(unit);
        _pool.Return(unit);
    }
}
