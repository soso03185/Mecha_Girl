using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseEffect : MonoBehaviour
{
    public GameObject touchPrefab;

    public RectTransform canvasTrans;

    float spawnTime;
    public float defaultTime = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        canvasTrans = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && spawnTime >= defaultTime)
        {
            CreateEffect();
            SoundManager.Instance.PlaySFX("ButtonClickSound");
            spawnTime = 0;
        }

        spawnTime += Time.deltaTime;
    }

    public void CreateEffect()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasTrans,
        Input.mousePosition,
            this.GetComponent<Canvas>().worldCamera,
            out localPoint
        );

        Vector3 worldPosition = this.GetComponent<Canvas>().transform.TransformPoint(localPoint);

        var effect = Managers.Pool.GetPool("TouchEffect").GetGameObject("UI/TouchEffect", worldPosition);

        StartCoroutine(ReturnEffect(effect));
    }

    IEnumerator ReturnEffect(GameObject effect)
    {
        yield return new WaitForSeconds(1f);

        Managers.Pool.GetPool("TouchEffect").ReturnObject(effect);
    }
 
}
