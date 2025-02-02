using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class ShadowThresholdCustomEffectScript : MonoBehaviour
{
    public Material shadowMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, shadowMaterial);
    }
}
