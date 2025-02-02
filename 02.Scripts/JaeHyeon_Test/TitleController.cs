using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UIElements;

/// <summary>
/// 타이틀 화면 컨트롤러
/// </summary>
public class TitleController : MonoBehaviour
{
    [SerializeField] Transform m_ImageGear_1;
    [SerializeField] Transform m_ImageGear_2;
    [SerializeField] Transform m_ImageGear_3;
    [SerializeField] Transform m_ImageGear_4;
    [SerializeField] CanvasGroup m_ImageStartBtn;

    public void MoveToLoadingScene()
    {
        SceneManager.LoadSceneAsync("01_JaeHyeonLoading");
    }

    public void Start()
    {
        m_ImageGear_1.DORotate(new Vector3(0, 0, 3600), 280, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Yoyo);
        m_ImageGear_2.DORotate(new Vector3(0, 0, -3600), 280, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Yoyo);
        m_ImageGear_3.DORotate(new Vector3(0, 0, 3600), 280, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Yoyo);
        m_ImageGear_4.DORotate(new Vector3(0, 0, -3600), 280, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Yoyo);
        m_ImageStartBtn.DOFade(0, 1.5f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnDisable()
    {
        m_ImageGear_1.DOKill();
        m_ImageGear_2.DOKill();
        m_ImageStartBtn.DOKill();
    }

    public void StageResetBtn()
    {
        Managers.Data.currentStageLevel = 1;
    }
}
