using UnityEngine;
using System.Collections.Generic;

public class UnitsPool
{
    private Dictionary<UnitType, List<UnitBase>> _units = null;

    public UnitsPool(UnitType[] createForStart)
    {
        // Initialize dictionary and lists
        _units = new Dictionary<UnitType, List<UnitBase>>();
        for (UnitType type = UnitType.begin + 1; type < UnitType.end; type++)
        {
            _units.Add(type, new List<UnitBase>());
        }

        // Create few units for start
        if (createForStart != null)
        {
            for (int i = 0; i < createForStart.Length; i++)
            {
                UnitBase unit = Get(createForStart[i]);
                unit.Reset();
            }
        }
    }

    public UnitBase Get(UnitType type)
    {
        List<UnitBase> units = null;
        if (_units.TryGetValue(type, out units))
        {
            if (units.Count > 0)
            {
                //Debug.LogFormat("Get unit from pool. Objects in pool {0}", units.Count);
                UnitBase unit = units[0];
                unit.Use();
                units.RemoveAt(0);
                return unit;
            }
            else
            {
                //Debug.LogFormat("Create new unit. Objects in pool {0}", units.Count);
                UnitBase newUnit = UnitsFactory.Create(type);
                newUnit.Use();
                return newUnit;
            }
        }
        else
        {
            Debug.LogErrorFormat("Unit type {0} does not found.", type);
            return null;
        }
    }

    public void Return(UnitBase unit)
    {
        List<UnitBase> units = null;
        if (_units.TryGetValue(unit.Type, out units))
        {
            unit.Reset();
            units.Add(unit);
        }
        else
        {
            Debug.LogErrorFormat("Unit type {0} does not found.", unit.Type);
        }
    }
}