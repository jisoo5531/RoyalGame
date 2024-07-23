using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCanvasInfo : MonoBehaviour
{
    public Canvas unitCanvas;
    public GameObject hpBarOBJ;
    public Image levelBackground;
    public Image hpBarFill;

    public bool isHpBarOn = false;

    private void Update()
    {
        unitCanvas.transform.rotation = Quaternion.Euler(0, -transform.rotation.y + 180, 0);

        //hpBarFill.fillAmount = (float)HP / (float)maxHP;
    }

    public void OnHPBar()
    {
        isHpBarOn = true;
        hpBarOBJ.SetActive(true);
    }
}
