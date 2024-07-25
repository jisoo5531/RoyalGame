using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_UpgradeStat : MonoBehaviour
{
    public TextMeshProUGUI value;
    public GameObject upgradePanel;
    public TextMeshProUGUI upgradeValue;

    private Image background;

    private void Awake()
    {
        background = GetComponent<Image>();
    }

    public void SetUpgrade(int amount)
    {
        upgradePanel.SetActive(true);        

        background.ColorGreen();
        upgradeValue.ColorGreen();
        upgradeValue.text = $"+ {amount}";
    }    
    public void SetNotUpgrade()
    {
        upgradePanel.SetActive(false);

        background.ColorWhite();        
        upgradeValue.ColorWhite();
    }
}
