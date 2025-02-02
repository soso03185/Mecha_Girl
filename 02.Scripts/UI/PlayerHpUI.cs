using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_hpText;
    Player m_player;
    Slider hpSlider;

    private void Start()
    {
        m_player = GameManager.Instance.player.GetComponent<Player>();
        m_player.playerHpBar = this.gameObject;

        hpSlider = GetComponent<Slider>();
        hpSlider.value = (float)(m_player.Health / m_player.MaxHealth);
    }

    public void ResetValue()
    {
        hpSlider.value = (float)(m_player.Health / m_player.MaxHealth);
        m_hpText.text = Mathf.FloorToInt(hpSlider.value * 100.0f).ToString() + "%";
    } 
}
