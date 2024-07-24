using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 적일 땐 빨간 색으로 바꾸기
[System.Serializable]
public class EnemyImagePrefab
{
    public Sprite levelBackground;
    public Sprite hpBarFill;
}
[System.Serializable]
public class AllyImagePrefab
{
    public Sprite levelBackground;
    public Sprite hpBarFill;
}


public class UnitCanvasInfo : MonoBehaviour, IDamagable
{
    public EnemyImagePrefab EnemyPrefab;
    public AllyImagePrefab allyPrefab;

    public int HP { get; set; }
    public int maxHP { get; set; }

    // 유닛 죽을 때 엘릭서 터지는 파티클
    public GameObject blastEffect;

    public Canvas unitCanvas;
    public GameObject hpBarOBJ;
    public Image levelBackground;
    public Image hpBarFill;

    private bool isHPBarOn = false;    

    private void Awake()
    {
        maxHP = 10000;
        HP = maxHP;                        
    }
    private void Start()
    {
        // Ally
        if (gameObject.layer == 11)
        {
            levelBackground.sprite = allyPrefab.levelBackground;
            hpBarFill.sprite = allyPrefab.hpBarFill;
        }
        // Enemy
        else if (gameObject.layer == 10)
        {
            levelBackground.sprite = EnemyPrefab.levelBackground;
            hpBarFill.sprite = EnemyPrefab.hpBarFill;
        }
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
    // 맞으면 HPBar On
    private void OnHPBar()
    {
        isHPBarOn = true;
        hpBarOBJ.SetActive(true);        
    }
}
