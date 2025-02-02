using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStageReStart : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.Init();
    }
}
