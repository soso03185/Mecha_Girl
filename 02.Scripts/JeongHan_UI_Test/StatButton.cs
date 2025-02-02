using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatButton : MonoBehaviour
{
    public string statName;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(AbilityUp);
    }

    public void AbilityUp()
    {
        GameManager.Instance.player.GetComponent<Player>().AbilityUp(statName);
    }
}
