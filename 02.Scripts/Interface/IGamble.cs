using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGamble
{
    public Define.Rarity Rarity { get; set; }
    public float Chance { get; set; }

}
