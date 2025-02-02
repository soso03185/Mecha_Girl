using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject m_target;
    public Skill m_skill;
    public float m_speed;

    public int m_firstSlot;
    public int m_secondSlot;
    public int m_thirdSlot;

    public bool isHit = false;

    public virtual void SetTripod(int tripodStep, int num)
    {
        switch (tripodStep)
        {
            case 1:
                {
                    m_firstSlot = num;
                    break;
                }
            case 2:
                {
                    m_secondSlot = num;
                    break;
                }
            case 3:
                {
                    m_thirdSlot = num;
                    break;
                }
        }
    }
}
