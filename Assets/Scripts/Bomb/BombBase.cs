using UnityEngine;
using System;
using System.Collections.Generic;

public class BombBase : MonoBehaviour
{
    public float radius = 1.5f;

    protected Transform _cachedTransform = null;

    private float _delay = 0f;
    private bool _isBegun = false;

    private Action<UnitBase[]> _onExplosionDoneHandler = null;

    void Awake()
    {
        _cachedTransform = transform;
    }

    public void Init(Vector3 startPosition, float delay, Action<UnitBase[]> onExplosionDoneHandler)
    {
        _onExplosionDoneHandler = onExplosionDoneHandler;

        _delay = delay;
        _cachedTransform.position = startPosition;
    }

    public void Begin()
    {
        _isBegun = true;
    }

    void Update()
    {
        if (_isBegun)
        {
            _delay -= Time.deltaTime;

            if (_delay <= 0f)
            {
                _isBegun = false;
                Explode();
            }
        }
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(_cachedTransform.position, radius);

        List<UnitBase> units = new List<UnitBase>();

        for (int i = 0; i < colliders.Length; i++)
        {
            UnitBase unit = colliders[i].GetComponent<UnitBase>();
            if (unit)
            {
                units.Add(unit);
            }
        }

        if (_onExplosionDoneHandler != null)
        {
            _onExplosionDoneHandler(units.ToArray());
            _onExplosionDoneHandler = null;
        }

        Destroy(gameObject);
    }
}