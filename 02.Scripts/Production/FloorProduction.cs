using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum FloorStatus
{
    Idle,
    Up,
    Down
}

public class FloorProduction : MonoBehaviour
{
    FloorStatus m_status;
    // Start is called before the first frame update
    private void Awake()
    {
        GameManager.Instance.PlayerAppearance += new EventHandler(FloorUp);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_status == FloorStatus.Up)
        {
            if(gameObject.transform.position.y < - 2)
            {
                gameObject.transform.position += new Vector3(0,Time.deltaTime * 32,0);
            }
        }
    }

    void FloorUp(object sender, EventArgs e)
    {
        StartCoroutine(FloorUpAfter1Second());   
    }

    IEnumerator FloorUpAfter1Second()
    {
        yield return new WaitForSeconds(1);
        m_status = FloorStatus.Up;
    }
}
