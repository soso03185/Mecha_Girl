using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraMovement : MonoBehaviour
{
    public GameObject m_player;
    public float m_interpolationRatio = 0.05f;
    private Vector3 m_correction;

    void Start()
    {
        m_player = GameManager.Instance.player;
        m_correction = transform.position - m_player.transform.position;
    }

    void Update()
    {
        if(m_player != null)
        {
            Vector3 cameraDest = m_player.transform.position + m_correction;
            transform.position = Vector3.Lerp(transform.position, cameraDest, m_interpolationRatio);
        }
    }

    private void LateUpdate()
    {
        float transformX = Mathf.Clamp(transform.position.x, -28, 28);
        float transformZ = Mathf.Clamp(transform.position.z, -44, 7.5f);
        transform.position = new Vector3(transformX, transform.position.y, transformZ);
    }
}
