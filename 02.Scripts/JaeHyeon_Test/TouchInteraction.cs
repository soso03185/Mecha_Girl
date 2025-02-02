using Coffee.UIExtensions;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ȭ�� ��ġ ��ȣ�ۿ� ��Ʈ�ѷ�
/// </summary>
public class TouchInteraction : MonoBehaviour
{
    public UIParticleAttractor m_Attractor;
    public List<GameObject> m_PopTexts = new List<GameObject>();
    DG.Tweening.Sequence m_scaleSequence;

    [SerializeField] Transform m_ManaBubble;

    Vector3 m_originscale;

    private void Start()
    {
        m_scaleSequence = DOTween.Sequence();

        foreach(var child in m_Attractor.particleSystems)
        {
            child.gameObject.SetActive(false);
        }

        m_originscale = m_ManaBubble.localScale;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // ��� Collider ����
            RaycastHit[] hits = Physics.RaycastAll(ray);

            // ������ Collider�� ��� ��ȸ
            foreach (RaycastHit hit in hits)
            {
                // �±װ� "HEAL"�� ������Ʈ�� ó��
                if (hit.transform.CompareTag("MedKit"))
                {
                    if (TutorialQuest.Instance != null) TutorialQuest.Instance.AlertTextSetActive(false);

                 //   Debug.Log("Touched MedKit object: " + hit.transform.name);
                    hit.transform.gameObject.SetActive(false);

                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(hit.point);

                    // UI ������Ʈ ����
                    EnableParticle(screenPosition);
                    return;
                }
            } 
        }
    }

    public void EnableParticle(Vector3 particlePosition) // MAX 21 particles & can more
    {
        foreach (var child in m_Attractor.particleSystems)
        {
            if (child.gameObject.activeSelf == true)
                continue;

            child.transform.position = particlePosition;
            child.gameObject.SetActive(true);
            return;
        }
    }

    public void GetMedKit()
    {
        foreach (var child in m_PopTexts)
        {
            if (child.gameObject.activeSelf == true)
                continue;

            child.gameObject.SetActive(true);
            break;
        }
         
        m_scaleSequence = DOTween.Sequence();
        m_scaleSequence.Append(m_ManaBubble.DOScale(m_originscale * 1.2f, 0.15f).SetEase(Ease.InOutSine))  // 1.1��� ������ Ŀ��
                     .Append(m_ManaBubble.DOScale(m_originscale, 0.05f).SetEase(Ease.InOutSine)) // 1��� ������ �پ��
                     .SetUpdate(true)  // TimeScale �����ϰ� ������Ʈ
                     .OnComplete(() => transform.DOKill());

        // player
        GameManager.Instance.player.GetComponent<Player>().Mana++;
    } 
}