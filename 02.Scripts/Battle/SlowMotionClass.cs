using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionClass : MonoBehaviour
{
    public static SlowMotionClass instance {  get; private set; }

    public float slowFactor = 0.05f;
    public float slowLength = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    Time.timeScale += (1f / slowLength) * Time.unscaledDeltaTime;
    //    Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    //    Time.fixedDeltaTime = Time.timeScale * 0.02f;
    //}

    public void DoSlowMotion()
    {
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
