using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoyalEnemyTest : MonoBehaviour, IDamagable
{    
    public int HP { get; set; }
    public int maxHP { get; set; }
    public GameObject blastEffect;

    public Canvas unitCanvas;
    private bool isCanvasOn;

    private void Awake()
    {
        maxHP = 10000;
        HP = maxHP;

        //unitCanvas = GetComponentInChildren<Canvas>();
    }
    private void Update()
    {
        float yRot = -transform.rotation.y;

        unitCanvas.transform.rotation = Quaternion.Euler(new Vector3(0, yRot, 0));
    }

    public void GetDamage(int damage)
    {
        if (isCanvasOn)
        {
            return;
        }
        else
        {
            OnCanvas();
        }

        HP -= damage;
        if (HP <= 0)
        {
            GameObject effect = Instantiate(blastEffect, transform.position + new Vector3(0, transform.localScale.y, 0), transform.rotation);
            effect.transform.localScale = transform.localScale;
            Destroy(gameObject);
        }
    }

    private void OnCanvas()
    {
        unitCanvas.gameObject.SetActive(true);
    }
}
