using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ObjectPool
{
    // �̱���
    //public static ObjectPool Instance;

    private GameObject poolingObjectPrefab;

    public List<GameObject> objectsList = new List<GameObject>();

    public int poolSize = 200;

    public Transform parentTransform;

    public int activeCount;

    // �� ���Ͽ� �ִ� ��� ������Ʈ ����
    public void CreateObjectsInFile(string path)
    {
        GameObject parentObj = new GameObject();
        parentObj.name = path + " Pool";
        parentTransform = parentObj.transform;

        GameObject[] objects = Managers.Resource.LoadAllGameObjects(path);

        for(int i = 0; i < objects.Length; i++)
        {
            GameObject obj = Object.Instantiate(objects[i], parentTransform);
            obj.SetActive(false);
            objectsList.Add(obj);
            Debug.Log(obj.name);
        }
    }

    // ���� ������Ʈ ���� �� ���� - �ڵ� �θ� ����
    public void CreateObject(string path, int size)
    {
        GameObject parentObj = new GameObject();
        GameObject.DontDestroyOnLoad(parentObj);
        parentObj.name = path + " Pool";
        parentTransform = parentObj.transform;

        for (int i = 0; i < size; i++)
        {
            poolingObjectPrefab = Managers.Resource.Instantiate(path, parentTransform);
            if (poolingObjectPrefab.name.Contains("(Clone)"))
            {
                poolingObjectPrefab.name = poolingObjectPrefab.name.Substring(0, poolingObjectPrefab.name.Length - 7);
            }
            poolingObjectPrefab.SetActive(false);
            objectsList.Add(poolingObjectPrefab);
        }
    }

    // ���� ������Ʈ ���� �� ���� �� ������ �θ� ������ �ֱ�
    public void CreateObject(string path, int size, Transform parent)
    {
        for (int i = 0; i < size; i++)
        {
            poolingObjectPrefab = Managers.Resource.Instantiate(path, parent);
            if (poolingObjectPrefab.name.Contains("(Clone)"))
            {
                poolingObjectPrefab.name = poolingObjectPrefab.name.Substring(0, poolingObjectPrefab.name.Length - 7);
            }

            poolingObjectPrefab.SetActive(false);
            objectsList.Add(poolingObjectPrefab);
        }
    }

    // �����ε�
    public GameObject GetGameObject(string path, Vector3 trans)
    {
        GameObject select = null;
        foreach(GameObject obj in objectsList) 
        {
            if(!obj.activeSelf)
            {
                select = obj;
                select.transform.position = trans;
                select.SetActive(true);
                activeCount++;
                break;
                
            }
        }

        if (!select)
        {
            select = Managers.Resource.Instantiate(path, parentTransform);
            if (select.name.Contains("(Clone)"))
            {
                select.name = select.name.Substring(0, select.name.Length - 7);
            }
            select.transform.position = trans;
            objectsList.Add(select);
            activeCount++;
        }

        return select;
    }

    // �����ε�
    public GameObject GetGameObject(string path, Transform trans)
    {
        GameObject select = null;
        foreach (GameObject obj in objectsList)
        {
            if (!obj.activeSelf)
            {
                select = obj;
                select.transform.SetParent(trans);
                select.SetActive(true);
                activeCount++;
                break;

            }
        }

        if (!select)
        {
            select = Managers.Resource.Instantiate(path, parentTransform);
            if (select.name.Contains("(Clone)"))
            {
                select.name = select.name.Substring(0, select.name.Length - 7);
            }
            select.transform.SetParent(trans);
            objectsList.Add(select);
            activeCount++;
        }

        return select;
    }

    public GameObject GetGameObject(string path)
    {
        GameObject select = null;

        foreach (GameObject obj in objectsList)
        {
            if (!obj.activeSelf)
            {
                select = obj;
                select.SetActive(true);
                activeCount++;
                break;
            }
        }

        if (!select)
        {
            select = Managers.Resource.Instantiate(path, parentTransform);
            if (select.name.Contains("(Clone)"))
            {
                select.name = select.name.Substring(0, select.name.Length - 7);
            }

            objectsList.Add(select);
            activeCount++;
        }
        return select;
    }

    public GameObject GetGameObjectByName(string name)
    {
        GameObject select = null;

        foreach (GameObject obj in objectsList)
        {
            if (obj.name.Contains("(Clone)"))
            {
                obj.name = obj.name.Substring(0, obj.name.Length - 7);
            }

            if (!obj.activeSelf && obj.name == name)
            {
                select = obj;
                select.SetActive(true);
                activeCount++;
                break;
            }
        }

        return select;
    }

    // return�Լ�
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);

        if(obj.GetComponent<MonsterScript>() == null)
        {
            return;
        }
        else // obj�� ������ ���
        {
            activeCount--;
        }
    }
}
