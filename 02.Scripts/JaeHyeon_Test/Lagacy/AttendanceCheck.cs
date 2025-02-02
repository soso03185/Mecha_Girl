using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttendanceCheck : MonoBehaviour
{
    [SerializeField] GameObject m_CardPrefab;
    [SerializeField] int m_CardCount;
   
    string m_CardName = "Attendance_Card";

    private void Start()
    {
        for(int i = 0; i < m_CardCount; i++)
        {
           Managers.Pool.GetPool(m_CardName).GetGameObject("UI/UI_JaeHyeon/" + m_CardName).transform.SetParent(this.transform);
          //  Instantiate(m_CardPrefab, this.transform);
        }
    }
}
