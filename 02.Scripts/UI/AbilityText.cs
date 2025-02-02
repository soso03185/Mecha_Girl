using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityText : MonoBehaviour
{
    public GameManager m_manager;

    public TextMeshProUGUI text;

    public string ability;
    void Awake()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        text.text = Utility.ToCurrencyString(m_manager.player.GetComponent<Player>().GetAbility(ability)).ToString();

    }
}
