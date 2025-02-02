using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ∞‘¿” æ¿¿« ≈È¥œπŸƒ˚ æ∆¿Ãƒ‹¿« ¥Â∆Æ¿© ø¨√‚
/// </summary>
public class TweenUI : MonoBehaviour
{
    [SerializeField] RectTransform m_Gear_Small;
    [SerializeField] RectTransform m_Gear_Small_Shadow;
    [SerializeField] RectTransform m_Gear_Mid;
    [SerializeField] RectTransform m_Gear_Mid_Shadow;
    [SerializeField] RectTransform m_Gear_Big;
    [SerializeField] RectTransform m_Gear_Big_Shadow;

    private void Start()
    {
        if(m_Gear_Small && m_Gear_Small_Shadow)
        {
            m_Gear_Small.DORotate(new Vector3(0, 0, 360), 28, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Yoyo);
            m_Gear_Small_Shadow.DORotate(new Vector3(0, 0, 360), 28 , RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Yoyo);
        }

        if (m_Gear_Mid && m_Gear_Mid_Shadow)
        {
            m_Gear_Mid.DORotate(new Vector3(0, 0, 360), 40, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Yoyo);
            m_Gear_Mid_Shadow.DORotate(new Vector3(0, 0, 360), 40, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Yoyo);
        }

        if (m_Gear_Big && m_Gear_Big_Shadow)
        {
            m_Gear_Big.DORotate(new Vector3(0, 0, -360), 60, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Yoyo);
            m_Gear_Big_Shadow.DORotate(new Vector3(0, 0, -360), 60, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
