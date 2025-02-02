using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GoogleSheet.Type;

public class CanvasSetting : MonoBehaviour
{
    public static CanvasSetting Instance;

    public List<Canvas> contentsCanvas = new List<Canvas>();

    public void Awake()
    {
        var obj = FindObjectsOfType<CanvasSetting>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
            SoundManager.Instance.SetBGMVolume(0.2f);
            SoundManager.Instance.PlayBGM("Glitch Rock");
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
   
}

