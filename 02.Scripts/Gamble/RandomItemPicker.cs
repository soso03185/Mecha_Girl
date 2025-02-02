using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class RandomItemPicker<T> where T : IGamble
{
    private Dictionary<Define.Rarity, Dictionary<T, float>> m_itemRarity =
        new Dictionary<Define.Rarity, Dictionary<T, float>>();

    private readonly Dictionary<T, float> m_itemC = new Dictionary<T, float>();
    private readonly Dictionary<T, float> m_itemB = new Dictionary<T, float>();
    private readonly Dictionary<T, float> m_itemA = new Dictionary<T, float>();
    private readonly Dictionary<T, float> m_itemS = new Dictionary<T, float>();


    // private int m_gambleLevel = 1;
    private readonly List<float> m_sumOfWeight = new List<float>(4) { 0, 0, 0, 0 };

    public RandomItemPicker() { }

    public RandomItemPicker(Dictionary<T, float> itemDictionary)
    {
        AddDictionary(itemDictionary);
    }

    public void AddDictionary(Dictionary<T, float> itemDictionary)
    {
        foreach (var item in itemDictionary)
        {
            Add(item.Key, item.Value);
        }
    }

    public void AddDictionaryWithList(List<KeyValuePair<T, float>> itemList)
    {
        foreach (var item in itemList)
        {
            Add(item.Key, item.Value);
        }
    }

    public void Add(T item, float weight)
    {
        if (weight < 0)
        {
            Debug.LogWarning("Weight must be positive");
        }

        switch (item.Rarity)
        {
            case Define.Rarity.C:
                Add(m_itemC, item, weight, 0);
                break;
            case Define.Rarity.B:
                Add(m_itemB, item, weight, 1);
                break;
            case Define.Rarity.A:
                Add(m_itemA, item, weight, 2);
                break;
            case Define.Rarity.S:
                Add(m_itemS, item, weight, 3);
                break;
        }
    }

    public void Remove(T item)
    {
        switch (item.Rarity)
        {
            case Define.Rarity.C:
                Remove(m_itemC, item, 0);
                break;
            case Define.Rarity.B:
                Remove(m_itemB, item, 1);
                break;
            case Define.Rarity.A:
                Remove(m_itemA, item, 2);
                break;
            case Define.Rarity.S:
                Remove(m_itemS, item, 3);
                break;
        }
    }

    private void Add(Dictionary<T, float> itemDictionary, T item, float weight, int index)
    {
        if (itemDictionary.ContainsKey(item))
        {
            Debug.LogWarning("Item Duplication");
        }

        itemDictionary.Add(item, weight);
        m_sumOfWeight[index] += weight;
    }
    private void Remove(Dictionary<T, float> itemDictionary, T item, int index)
    {
        if (itemDictionary.TryGetValue(item, out float weight))
        {
            m_sumOfWeight[index] -= weight;
            itemDictionary.Remove(item);
        }
        else
        {
            Debug.LogError("Item not found");
        }
    }
    public T GetRandomItem()
    {
        float rarity = Random.Range(0f, 100f);
        if (rarity is >= 0 and <= 60)
        {
            float result = Random.Range(0f, m_sumOfWeight[0]);
            return GetRandomItem(m_itemC, result);
        }
        if (rarity <= 90)
        {
            float result = Random.Range(0f, m_sumOfWeight[1]);
            return GetRandomItem(m_itemB, result);
        }
        if (rarity <= 99)
        {
            float result = Random.Range(0f, m_sumOfWeight[2]);
            return GetRandomItem(m_itemA, result);
        }
        if (!(rarity <= 100)) return default;
        {
            float result = Random.Range(0f, m_sumOfWeight[3]);
            return GetRandomItem(m_itemS, result);
        }

    }

    public T GetRandomItem(Dictionary<T, float> itemDictionary, float result)
    {
        foreach (var item in itemDictionary)
        {
            if (result < item.Value)
            {
                return item.Key;
            }
            result -= item.Value;
        }

        return default;
    }
}
