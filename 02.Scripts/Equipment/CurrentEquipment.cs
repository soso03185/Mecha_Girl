using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentEquipment : MonoBehaviour
{
    [SerializeField] private int m_index;
    // Start is called before the first frame update
    void Start()
    {
        //EquipmentUpdate();
    }

    public void EquipmentUpdate()
    {
        gameObject.GetComponent<Image>().sprite = Managers.Equipment.m_currentEquipment[m_index].m_image; 
    }
}
