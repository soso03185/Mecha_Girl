using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShaderController : MonoBehaviour
{
    [SerializeField] Renderer[] m_TargetRenderers;
    [SerializeField] string m_FloatPropertyName = "_EffectValue";
    [SerializeField] GameObject[] m_ParticleSystems;

    [SerializeField] List<Material> m_Materials;
    float m_Value = 0;
    float m_D = 0.02f;

    public bool m_IsAppear = false;
    public bool m_IsDisappear = false;

    private void Start()
    {
        foreach(var child in m_TargetRenderers)
        {
            m_Materials.Add(child.material);
        }
    }
    private void Update()
    {
        if (m_IsAppear) StartCoroutine("CoAppear");
        if (m_IsDisappear) StartCoroutine("CoDisappear");
    }

    public IEnumerator CoAppear()
    {
        m_Value += m_D;

        foreach (var child in m_Materials)
        {
            child.SetFloat(m_FloatPropertyName, m_Value);
        }
        
        yield return new WaitForSeconds(1.5f);
        m_Value = 1;

        foreach (var child in m_Materials)
        {
            child.SetFloat(m_FloatPropertyName, m_Value);
        }
        m_IsAppear = false;
    }

    public IEnumerator CoDisappear()
    {
        foreach(var child in m_ParticleSystems)
        {
            child.SetActive(true);
        }

        m_Value = -1.0f;

        foreach (var child in m_Materials)
        {
            child.SetFloat(m_FloatPropertyName, m_Value);
        }

        yield return new WaitForSeconds(1.5f);
        m_IsDisappear = false;

        foreach (var child in m_ParticleSystems)
        {
            child.SetActive(false);
        }
        //   this.gameObject.SetActive(false);
    }
}
