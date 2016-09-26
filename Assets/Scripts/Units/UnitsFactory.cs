using UnityEngine;

public class UnitsFactory
{
    public static UnitBase Create(UnitType type)
    {
        return LoadAndInstantiate(type);
    }

    private static UnitBase LoadAndInstantiate(UnitType type)
    {
        string path = string.Format("Prefabs/Characters/{0}", type);
        Object[] resources = Resources.LoadAll(path);

        if (resources.Length > 0)
        {
            GameObject obj = Object.Instantiate(resources[Random.Range(0, resources.Length)]) as GameObject;
            return obj.GetComponent<UnitBase>();
        }
        else
        {
            Debug.LogErrorFormat("Resource file for type {0} does not found.", type);
            return null;
        }
    }
}