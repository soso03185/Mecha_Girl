using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private GameObject m_monster;
    private RectTransform m_rectTransform;
    public TextMeshPro textMeshProUGUI;

    Color alpha;

    public Vector3 fontsize;

    public Vector3 InitFontSize;


    public float moveSpeed;
    public float disableTime;
    public float alphaSpeed;
    public float fontSpeed;

    public float duration = 1f;    // �ؽ�Ʈ�� �ö󰡴� ���� �ð�
    private float elapsedTime = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        textMeshProUGUI = GetComponent<TextMeshPro>();
        alpha = textMeshProUGUI.color;
        fontsize = textMeshProUGUI.transform.localScale;
        InitFontSize = fontsize;
    }

    private void OnEnable()
    {
        if(m_monster != null)
        {
            m_rectTransform.position =
           Camera.main.WorldToScreenPoint(m_monster.transform.position) + new Vector3(0, 100, 0);
        }
        textMeshProUGUI.transform.localScale = fontsize;
        alpha.a = 1f;
        elapsedTime = 0f;
    }

    public void OnDisable()
    {
        fontsize = InitFontSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_monster != null)
        {
            // ������ ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ
            Vector3 monsterScreenPosition = Camera.main.WorldToScreenPoint(m_monster.transform.position);

            // ȭ���� �߰� ��ǥ ��� (ȭ�� �ʺ��� ����)
            float screenMiddle = Screen.width / 2f;

            // ���Ͱ� ȭ�� ���ʿ� �ִ��� �����ʿ� �ִ��� �Ǵ�
            Vector3 moveDirection;
            if (monsterScreenPosition.x < screenMiddle)
            {
                // ���ʿ� ������ ���� �� �밢������ �̵�
                moveDirection = new Vector3(-.5f, .5f, 0).normalized;
            }
            else
            {
                // �����ʿ� ������ ������ �� �밢������ �̵�
                moveDirection = new Vector3(.5f, .5f, 0).normalized;
            }

            // ���� �̵�
            m_rectTransform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); // �ؽ�Ʈ ���İ�

   
        if(!this.gameObject.name.Contains("ImmuneText"))
        {
            textMeshProUGUI.transform.localScale = Vector3.Lerp(textMeshProUGUI.transform.localScale, new Vector3(1,1,1) * 1.05f, fontSpeed * Time.deltaTime);
        }
        textMeshProUGUI.color = alpha;

        // ���� �ð����� Pool�� �������ֱ�
        if (elapsedTime >= duration)
        {
            ReturnDamageText(this.gameObject);
        }
        elapsedTime += Time.deltaTime;
    }

    public void ShowDamageText(GameObject monster, string damage)
    {
        SetMonster(monster);
        textMeshProUGUI.text = damage;
    }

    public void ShowImmuneText(GameObject monster)
    {
        SetMonster(monster);
    }

    public void SetMonster(GameObject monster)
    {
        m_monster = monster;
    }

    public void ReturnDamageText(GameObject obj)
    {
        m_monster = null;
        Managers.Pool.GetPool("DamageText").ReturnObject(obj);
    }
}
