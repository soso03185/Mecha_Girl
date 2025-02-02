using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{

    public Transform camera;
    public bool shakeRotate = false;

    private Vector3 originPos;
    private Quaternion originRot;

    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        originPos = camera.localPosition;
        originRot = camera.localRotation;

        
    }

    IEnumerator ShakeCamera(float startDelay = 0f, float duration = 0.05f, float magnitudePos = 5f, float magnitudeRot = 0.1f)
    {
        yield return new WaitForSeconds(startDelay);

        float passTime = 0.0f;

        originPos = camera.localPosition;
        originRot = camera.localRotation;

        while (passTime < duration)
        {
            Vector3 shakePos = Random.insideUnitSphere;

            camera.localPosition = shakePos * magnitudePos + originPos;

            if(shakeRotate)
            {
                Vector3 shakeRot = new Vector3(0, 0, Mathf.PerlinNoise(Time.time * magnitudeRot, 0.0f));

                camera.localRotation = Quaternion.Euler(shakeRot);
            }

            passTime += Time.deltaTime;

            yield return null;
        }

        camera.localPosition = originPos;
        camera.localRotation = originRot;
    }

    IEnumerator ZoomInCamera(float duration = 0.5f, float magnitudeZoom = 0.3f)
    {
        float passTime = 0.0f;

        originPos = camera.position;

        while (passTime < duration)
        {
            Vector3 zoomPos = player.position - camera.position;

            Vector3 targetPos = camera.position + zoomPos.normalized * magnitudeZoom;

            camera.position = Vector3.Lerp(camera.position, targetPos, duration);

            passTime += Time.deltaTime;

            yield return null;
        }

        camera.position = originPos;
    }

    IEnumerator ZoomOutCamera(float duration, float magnitudeZoom = 0.5f)
    {
        float passTime = 0.0f;

        originPos = camera.position;
        Vector3 v = new Vector3();
        while (passTime < duration)
        {
            Vector3 zoomPos = player.position - camera.position;

            Vector3 targetPos = camera.position + -zoomPos.normalized * magnitudeZoom;
            v = targetPos;
            camera.position = Vector3.Lerp(camera.position, targetPos, passTime / duration);

            passTime += Time.deltaTime;

            yield return null;
        }

        passTime = 0.0f;

       //while (passTime < 1)
       //{
       //    // 카메라를 원래 위치로 이동
       //    camera.position = Vector3.Lerp(v, camera.position, passTime / duration);

       //    passTime += Time.deltaTime * 2;

       //    yield return null;
       //}

        // 최종적으로 정확히 원래 위치로 설정
        //camera.position = originPos;
    }

    public void AnimEventShake(float startDelay, float duration, float magnitudePos, float magnitudeRot)
    {
        StartCoroutine(ShakeCamera(startDelay, duration, magnitudePos, magnitudeRot));
    }

    public void AnimEventZoomIn()
    {
        StartCoroutine(ZoomInCamera());
    }

    public void AnimEventZoomOut(float duration)
    {
        if (duration > 0)
        {
            StartCoroutine(ZoomOutCamera(duration));
        }
        else
        { 
            StartCoroutine(ZoomOutCamera(duration));
        }
    }
}
