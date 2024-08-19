using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fireball : MonoBehaviour
{
    public int damage { get; private set; }
    public Vector3 targetPos;
    private float speed = 7f;

    private ClickMagic fireBall;

    private void Awake()
    {
        fireBall = FindObjectOfType<ClickMagic>();
        InitializeUnitData(UnitSpawner.instance.selectedUnit);
        SendDamage(damage);

    }
    private void InitializeUnitData(AllCardData cardData)
    {
        damage = cardData.damage;
        targetPos.y = -1;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(targetPos), out hit, 100f))
        {
            transform.LookAt(targetPos);
            this.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        }
    }

    private void Start()
    {
        //SpawnFireBall();
    }

    private void SpawnFireBall()
    {
        fireBall.SpawnFireBall(gameObject);
    }


    public void SendDamage(int damage)
    {
        //GetComponent<Damaging>().damage = damage;
    }
}
