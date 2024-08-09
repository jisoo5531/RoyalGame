using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Elixir : MonoBehaviour
{
    public Image elixirImage;
    public Slider elixirSlider;
    public TextMeshProUGUI elixirText; 
    public float maxElixir = 10f;
    public float currentElixir;

    private float elixirRechargeRate = 1f;
    private int changeRate = 1; 

    private void Start()
    {
        currentElixir = 5f;        

        StartCoroutine(RechargeElixir());
    }
    private void Update()
    {
        UI_UpdateCurrentElixir();
    }

    private IEnumerator RechargeElixir()
    {
        while (true)
        {            
            yield return null;
            if (currentElixir < maxElixir)
            {
                currentElixir = Mathf.Lerp(currentElixir, currentElixir + elixirRechargeRate, Time.deltaTime * 0.3f);
                if (currentElixir > maxElixir)
                {
                    currentElixir = maxElixir;
                }         
            }
        }
    }
    private void UI_UpdateCurrentElixir()
    {
        elixirSlider.value = Mathf.MoveTowards(elixirSlider.value, currentElixir, changeRate * Time.deltaTime * 0.3f);
        if (elixirText != null)
        {
            elixirText.text = ((int)elixirSlider.value).ToString();
        }
    }    
    
    public void ElixirMinus(int elixirCost)
    {
        currentElixir -= elixirCost;
        changeRate = 20;
        Invoke("ReturnRate", 1f);
    }

    private void ReturnRate() => changeRate = 1;
    public bool IsSpawnUnitPossible(int elixirCost) => currentElixir >= elixirCost;
}
