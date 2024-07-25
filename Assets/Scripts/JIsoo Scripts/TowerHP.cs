using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerHP : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public int hp;

    private void Update()
    {
        hpText.text = hp.ToString();
    }
}
