using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

[System.Serializable]
public class AllyImagePrefab
{
    public Sprite hpBarFill;
    public Sprite levelSprite;
}


public class UnitCanvasInfo : MonoBehaviourPunCallbacks, IDamagable
{

    #region public 변수
    public AllyImagePrefab allyPrefab;

    public int HP { get; set; }
    public int maxHP { get; set; }

    public GameObject blastEffect;

    public GameObject unitCanvas;
    public GameObject hpBarOBJ;
    public Image hpBarFill;
    public Image level;
    public TextMeshProUGUI hpBarvalue;
    public Image spawnTimeUI;
    public GameObject clock;
    public TextMeshProUGUI levelValue;

    public bool isTower;
    public bool isKingTower;
    public bool isPrince;

    public int objIndex;
    #endregion

    #region private 변수
    Tower tower;
    GameObject effect;
    private bool isHPBarOn = false;
    private float spawnTime;
    #endregion

    private void Start()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_Init", RpcTarget.Others, HP, maxHP, objIndex);
            this.hpBarFill.sprite = allyPrefab.hpBarFill;

            if (!isTower)
            {
                spawnTimeUI.color = Color.cyan;
                this.level.sprite = allyPrefab.levelSprite;
                photonView.RPC("SpawnTime", RpcTarget.All);
            }
            if (isKingTower)
            {
                tower = this.transform.parent.GetComponent<Tower>();
            }
        }
    }

    public void UIInit(int level, float time)
    {
        photonView.RPC("RPC_UIINIT", RpcTarget.All, level, time);
    }

    [PunRPC]
    private void RPC_Init(int hp, int maxHp, int index)
    {
        this.HP = hp;
        this.maxHP = maxHp;
        objIndex = index;
    }

    [PunRPC]
    private void RPC_UIINIT(int level, float time)
    {
        levelValue.text = level.ToString();
        spawnTime = time;
    }

    [PunRPC]
    private void SpawnTime()
    {
        StartCoroutine(FillTimer());
    }

    IEnumerator FillTimer()
    {
        float elapsedTime = 0f;
        while (elapsedTime < spawnTime)
        {
            elapsedTime += Time.deltaTime;
            spawnTimeUI.fillAmount = elapsedTime / spawnTime;
            yield return null;
        }

        spawnTimeUI.fillAmount = 1f;
        clock.SetActive(false);
        level.gameObject.SetActive(true);
        if (gameObject.TryGetComponent<MovableUnit>(out MovableUnit mu))
        {
            mu.isWait = false;
        }
        else if (gameObject.TryGetComponent<DeffenseTower>(out DeffenseTower tower))
        {
            tower.isWait = false;
        }
    }

    public void GetDamage(int damage)
    {
        if (this == null) return;

        photonView.RPC("RPC_damage", RpcTarget.All, damage, this.HP, this.maxHP);
    }

    private void RPC_Death(int hp)
    {
        if (hp <= 0)
        {
            if(photonView.IsMine)
            {
                IsMineTrueDeath();
            }
            else
            {
                IsMineFalseDeath();
                if(isTower)
                {
                    ShowLimit(objIndex);
                }
            }
            Death(isTower);
        }
    }

    private void IsMineFalseDeath()
    {
        if (isTower)
        {
            DetectEnemyManager.instance.towerList.Remove(this.transform.root.gameObject);
            ScoreManager.instance.allyCount++;
            ScoreManager.instance.SettingScore();
        }
        else
        {
            DetectEnemyManager.instance.enemyList.Remove(this.gameObject);
        }
    }

    private void IsMineTrueDeath()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (isTower)
            {
                MasterManager.instance.RemoveTower(this.transform.root.gameObject);

                if (TimeManager.instance.isTimeZero)
                {
                    TimeManager.instance.GameEnd();
                }
            }
        }
        else
        {
            if (isTower)
            {
                NonMasterManager.instance.RemoveTower(this.transform.root.gameObject);

                if (TimeManager.instance.isTimeZero)
                {
                    TimeManager.instance.GameEnd();
                }
            }
        }
    }

    [PunRPC]
    private void RPC_damage(int damage, int currentHp, int currentMaxHp)
    {
        if (this == null) return;

        if (isTower)
        {
            if (isKingTower && tower != null && tower.isNotOnCannon)
            {
                tower.isNotOnCannon = false;
            }
        }
        else
        {
            if (!isHPBarOn)
            {
                OnHPBar();
            }
        }

        this.HP -= damage;
        this.HP = Mathf.Max(HP, 0);
        hpBarFill.fillAmount = (float)HP / maxHP;
        if (isTower)
        {
            hpBarvalue.text = HP.ToString();
        }

        if(HP <= 0)
        {
            RPC_Death(HP);
        }
    }

    private void Death(bool isTower)
    {
        if (isTower)
        {
            effect = Instantiate(blastEffect, transform.position + new Vector3(0, 5, 0), transform.rotation);
            effect.transform.localScale = new Vector3(8, 8, 8);
            Destroy(gameObject.transform.root.gameObject);
        }
        else
        {
            effect = Instantiate(blastEffect, transform.position + new Vector3(0, transform.localScale.y, 0), transform.rotation);
            effect.transform.localScale = transform.localScale;
            Destroy(gameObject);
        }
        Destroy(effect, 1f);
    }

    private void ShowLimit(int index)
    {
        GameManager.instance.spawnLimits.DisableIndexTowerLimit(index);
        GameManager.instance.spawnLimits.TowerDestroyEnable();
    }

    private void OnHPBar()
    {
        isHPBarOn = true;
        hpBarOBJ.SetActive(true);
    }
}
