using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

/// <summary>
/// 머테리얼에 사용되는 쉐이더 컨트롤러
/// </summary>
public class TestMaterialController : MonoBehaviour
{
    string m_BurnPropertyName = "_BurnAmount";
    string m_HitPropertyName = "_HitAmount";
    string m_HitDurationPropertyName = "_HitDuration";

    string m_RimInten = "_RimInten";
    string m_RimColor = "_RimColor";

    Renderer m_TargetRenderer;
    [HideInInspector] public Material m_Material;

    // burn
    public float m_BurnValue = 1.0f;

    // hit
    [HideInInspector] public float m_HitTimer = 0.0f;
    public float m_HitDuration;

    // RimLight
    public float m_RimValue = 20.0f;
    public float m_RimPower = 0.1f;

    private void Awake()
    {
        m_HitDuration = 0.4f;

        m_TargetRenderer = GetComponent<SkinnedMeshRenderer>();
        m_Material = m_TargetRenderer.material;

        // burn
        m_Material.SetFloat(m_BurnPropertyName, m_BurnValue);

        // hit
        m_Material.SetFloat(m_HitPropertyName, m_HitDuration);
        m_Material.SetFloat(m_HitDurationPropertyName, m_HitDuration);
    }

    public void RimOn()
    {
        m_RimValue = 20.0f;
        m_Material.SetFloat(m_RimInten, m_RimValue);
    }

    public void RimColor(Color _rimColor)
    {
          m_Material.SetColor(m_RimColor, _rimColor * 2);
    }

    public void RimWhiteColor()
    {
        m_Material.SetColor(m_RimColor, Color.white);
    }
    public void RimOff()
    {
        m_RimValue = 0.0f;
        m_Material.SetFloat(m_RimInten, m_RimValue);
    }

    public void HitReset()
    {
        m_HitTimer = 0;
    }

    public void HitUpdate()
    {
        if(m_Material != null)
        {
            m_HitTimer += Time.deltaTime;
            float MaxWhiteColorValue = 0.06f;

            if (m_HitTimer >= (m_HitDuration * 0.5f))
            { 
                float whiteIntensity = Mathf.Lerp(MaxWhiteColorValue, m_HitDuration, m_HitTimer / m_HitDuration) * 1.1f;
                m_Material.SetFloat(m_HitPropertyName, whiteIntensity);
            }
            else
                m_Material.SetFloat(m_HitPropertyName, MaxWhiteColorValue);
        }
    }

    public void SpawnUpdate(float _spawnSpeed)
    {
        if (m_Material != null)
        {
            if (m_BurnValue > 0)
                m_BurnValue -= _spawnSpeed;
            m_Material.SetFloat(m_BurnPropertyName, m_BurnValue);
        }
        else
        {
            m_TargetRenderer = GetComponent<SkinnedMeshRenderer>();
            m_Material = m_TargetRenderer.material;

            // reset value
            m_Material.SetFloat(m_BurnPropertyName, m_BurnValue);
        }
    }

    public void DeadUpdate(float _spawnSpeed)
    {
        if (m_Material != null)
        {
            if (m_BurnValue < 1)
                m_BurnValue += _spawnSpeed * 0.5f;
            m_Material.SetFloat(m_BurnPropertyName, m_BurnValue);
        }
    }

    public void BurnSetActive(bool isBurn)
    {
        if(isBurn)
        {
            // for Spawn 쉐이더 
            m_BurnValue = 0.0f;
            m_Material.SetFloat(m_BurnPropertyName, m_BurnValue);
        }
    }

    public void ResetValue()
    {
        if (m_Material != null)
        {
            m_BurnValue = 1.0f;
            m_Material.SetFloat(m_BurnPropertyName, m_BurnValue);

            m_RimValue = 0.0f; // On 20.0f , off 0.0f
            m_Material.SetFloat(m_RimInten, m_RimValue);
        }
    }
}
