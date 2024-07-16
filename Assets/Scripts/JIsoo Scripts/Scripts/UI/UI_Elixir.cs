using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Elixir : MonoBehaviour
{
    public Image elixirImage;
    public Slider elixirSlider;
    public TextMeshProUGUI elixirText;      // 엘릭서를 표시할 UI 텍스트    
    public float maxElixir = 10f;
    public float currentElixir;

    private float elixirRechargeRate = 1f;  // 초당 엘릭서 충전량
    private int changeRate = 1; 

    private void Start()
    {
        currentElixir = maxElixir;        

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
                currentElixir = Mathf.Lerp(currentElixir, currentElixir + elixirRechargeRate, Time.deltaTime * 0.5f);
                //currentElixir += (int)elixirRechargeRate;
                if (currentElixir > maxElixir)
                {
                    currentElixir = maxElixir;
                }         
            }
        }
    }
    private void UI_UpdateCurrentElixir()
    {
        //elixirImage.fillAmount = currentElixir / maxElixir;        
        //elixirImage.fillAmount = Mathf.Lerp(elixirImage.fillAmount, currentElixir / maxElixir, Time.deltaTime);

        elixirSlider.value = Mathf.MoveTowards(elixirSlider.value, currentElixir, changeRate * Time.deltaTime * 0.5f);
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
