using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static Define;

public class EffectManager : MonoBehaviour
{
    public VisualEffect[] effects;
    public ParticleSystem[] particleSystems;

    public List<GameObject> particles = new List<GameObject>();
    public GameObject skillPrefab;

    public EffectType effectType;

    private Vector3 originalScale;
    private void OnEnable()
    {
        if(effectType == EffectType.VE)
        {
            effects = GetComponentsInChildren<VisualEffect>();
        }
        else if(effectType == EffectType.PS)
        {
            particleSystems = GetComponentsInChildren<ParticleSystem>();
        }
        originalScale = transform.localScale;
    }

    private void OnDisable()
    {
       transform.localScale = originalScale;
    }

    public void PlayEffects(Transform trans = null, float angle = 0)
    {
        if(effectType == EffectType.VE)
        {
            foreach (var ve in effects)
            {
                ve.Play();
            }
        }
        else if( effectType == EffectType.PS)
        {
            foreach (var ps in particleSystems)
            {
                ps.Play();
            }
        }
        else if(effectType == EffectType.Script)
        {
            GameObject obj = GameObject.Instantiate(skillPrefab);
            obj.transform.position = trans.position;
            obj.transform.rotation = Quaternion.Euler(0, angle, 0);
            particles.Add(obj);

            Destroy(obj, 5f);
        }
    }

    public void StopParticle()
    {
        if (effectType == EffectType.VE)
        {
            foreach (var ve in effects)
            {
                ve.Stop();
            }
        }
        else if (effectType == EffectType.PS)
        {
            foreach (var ps in particleSystems)
            {
                ps.Stop();
            }
        }
    }
}
