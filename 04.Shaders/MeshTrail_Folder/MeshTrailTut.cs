using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrailTut : MonoBehaviour
{
    public float activeTime = 2.0f;

    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 3.0f;
    public Transform positionToSpawn;

    [Header("Shader Related")]
    public Material mat;
    public string shaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;

    private bool isTrailActive = false;
    private SkinnedMeshRenderer skinnedMeshRenderers;

    void OnEnable()
    {
        isTrailActive = true;
        StartCoroutine(ActivateTrail(activeTime));
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        while(timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if(skinnedMeshRenderers == null)
                skinnedMeshRenderers = positionToSpawn.GetChild(1).GetComponent<SkinnedMeshRenderer>();

            //for(int i = 0; i<skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderers.BakeMesh(mesh);

                mf.mesh = mesh;
                mr.material = mat;

                StartCoroutine(AnimateMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy(gObj, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        isTrailActive = false;
    }

    IEnumerator AnimateMaterialFloat (Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);

        while(valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
