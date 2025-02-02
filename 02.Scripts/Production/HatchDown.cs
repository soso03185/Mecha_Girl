using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchDown : MonoBehaviour
{
    HatchStatus m_currentStatus;
    public float m_hatchSpeed;
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.Instance.PlayerAppearance += new EventHandler(HatchOpen);
    }

    private void Start()
    {
        GameManager.Instance.player.GetComponent<Player>().playerJump += new EventHandler(HatchClose);
    }
    // Update is called once per frame
    void Update()
    {
        if (m_currentStatus == HatchStatus.Open)
        {
            if (gameObject.transform.localPosition.z > 3)
            {
                gameObject.transform.localPosition -= new Vector3(0, 0, Time.deltaTime * m_hatchSpeed);
            }
        }
        else if (m_currentStatus == HatchStatus.Close)
        {
            if (gameObject.transform.localPosition.z <7)
            {
                gameObject.transform.localPosition += new Vector3(0, 0, Time.deltaTime * m_hatchSpeed);
            }
        }
    }

    public void HatchOpen(object sender, EventArgs e)
    {
        m_currentStatus = HatchStatus.Open;
    }

    public void HatchClose(object sender, EventArgs e)
    {
        m_currentStatus = HatchStatus.Close;
    }
}
