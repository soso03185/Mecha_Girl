using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");

        if(prefab == null) 
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }
        
        return Object.Instantiate(prefab, parent);
    }

    public Sprite LoadSprite(string path)
    {
        Sprite sprite = Resources.Load<Sprite>($"Sprites/{path}");

        if(sprite == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        return sprite;
    }

    public Sprite[] LoadAllSprites(string path)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);

        return sprites;
    }

    public GameObject[] LoadAllGameObjects(string path)
    {
        GameObject[] goList = Resources.LoadAll<GameObject>("Prefabs/" + path);

        return goList;
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject == null)
            return;
        Object.Destroy(gameObject);
    }

    public string SubStringPath(string path)
    {
        string name = path;
        int index = name.LastIndexOf('/');
        if (index >= 0)
            name = name.Substring(index + 1);

        return name;
    }
}
