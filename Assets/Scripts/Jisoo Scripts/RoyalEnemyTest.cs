using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ImagePrefab
{
    public Sprite levelBackground;
    public Sprite hpBarFill;
}

public class RoyalEnemyTest : MonoBehaviour, IDamagable
{
    public ImagePrefab imagePrefab;

    public int HP { get; set; }
    public int maxHP { get; set; }
    public GameObject blastEffect;


    public Canvas unitCanvas;
    public GameObject hpBarOBJ;
    public Image leverBackground;
    public Image hpBarFill;

    private bool isHPBarOn = false;    

    private void Awake()
    {
        maxHP = 10000;
        HP = maxHP;

        leverBackground.sprite = imagePrefab.levelBackground;
        hpBarFill.sprite = imagePrefab.hpBarFill;        
    }
    private void Update()
    {                
        unitCanvas.transform.rotation = Quaternion.Euler(0, -transform.rotation.y + 180, 0);
        
        hpBarFill.fillAmount = (float)HP / (float)maxHP;
    }

    public void GetDamage(int damage)
    {        
        HP -= damage;
        if (HP <= 0)
        {
            GameObject effect = Instantiate(blastEffect, transform.position + new Vector3(0, transform.localScale.y, 0), transform.rotation);
            effect.transform.localScale = transform.localScale;
            Destroy(gameObject);
        }
        if (isHPBarOn)
        {
            return;
        }
        else
        {
            OnHPBar();
        }
    }

    private void OnHPBar()
    {
        isHPBarOn = true;
        hpBarOBJ.SetActive(true);        
    }
}
