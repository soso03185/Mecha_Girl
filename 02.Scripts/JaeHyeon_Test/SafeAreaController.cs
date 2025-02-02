using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모바일 환경의 노치 대응
/// </summary>
public class SafeAreaController : MonoBehaviour
{
    Vector2 minAnchor;
    Vector2 maxAnchor;

    private void Start()
    {
        var Myrect = this.GetComponent<RectTransform>();

        minAnchor = Screen.safeArea.min;
        maxAnchor = Screen.safeArea.max;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;

        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        Myrect.anchorMin = minAnchor;
        Myrect.anchorMax = maxAnchor;
    }
}
