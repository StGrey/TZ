using UnityEngine;

public class BombsFactory
{
    public static BombBase Create(BombType type)
    {
        return LoadAndInstantiate(type);
    }

    private static BombBase LoadAndInstantiate(BombType type)
    {
        string path = string.Format("Prefabs/Bombs/{0}", type);
        Object[] resources = Resources.LoadAll(path);

        if (resources.Length > 0)
        {
            GameObject obj = Object.Instantiate(resources[Random.Range(0, resources.Length)]) as GameObject;
            return obj.GetComponent<BombBase>();
        }
        else
        {
            Debug.LogErrorFormat("Resource file for type {0} does not found.", type);
            return null;
        }
    }
}
