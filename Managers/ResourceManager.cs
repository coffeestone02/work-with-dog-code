using UnityEngine;

public class ResourceManager
{
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Resources.Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.LogError($"Failed to load prefab : Prefabs/{path}");
            return null;
        }

        GameObject go = Object.Instantiate(original, parent);
        return go;
    }

    public Sprite LoadIcon(string iconName)
    {
        Sprite icon = Resources.Load<Sprite>($"Visual/Icons/{iconName}");
        if (icon == null)
        {
            Debug.LogError($"Failed to load icon : Visual/Icons/{iconName}");
            return null;
        }

        return icon;
    }
}
