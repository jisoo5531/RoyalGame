using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fireball : MonoBehaviourPunCallbacks
{
    public int damage { get; private set; }
    public Vector3 targetPos;


    private float speed = 50;
    private Rigidbody rb;
    private ClickMagic fireBall;
    Vector3 direction;
    private void Awake()
    {
        fireBall = FindObjectOfType<ClickMagic>();
        if (photonView.IsMine)
        {
            InitializeUnitData(UnitSpawner.instance.selectedUnit);
            SendDamage(damage);
        }
        //this.GetComponent<ParticleSystem>
        //ParticleSystem[] particleSystems = this.GetComponentsInChildren<ParticleSystem>();
        //foreach (ParticleSystem particle in particleSystems)
        //{
        //    particle.Play();
        //}
    }
    private void Start()
    {
        if (photonView.IsMine)
        {
            rb = this.GetComponent<Rigidbody>();
            direction = (targetPos - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    private void InitializeUnitData(AllCardData cardData)
    {
        damage = cardData.damage;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    public void SendDamage(int damage)
    {
        //GetComponent<Damaging>().damage = damage;
    }
}
