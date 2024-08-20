using Mysqlx.Crud;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviourPunCallbacks
{
    public int damage { get; private set; }
    public int towerDamage { get; private set; }
    public Vector3 targetPos;

    public LayerMask floorLayer;
    public LayerMask targetLayer;
    private float speed = 45f;
    private float launchAngle = 8f;
    private Rigidbody rb;
    Vector3 direction;

    List<GameObject> triggerObjs = new();

    private void Awake()
    {
        if (photonView.IsMine)
        {
            InitializeUnitData(UnitSpawner.instance.selectedUnit);
        }
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            rb = this.GetComponent<Rigidbody>();
            float launchAngleRad = launchAngle * Mathf.Deg2Rad;

            direction = (targetPos - transform.position).normalized;

            Vector3 velocity = new Vector3(
                direction.x * Mathf.Cos(launchAngleRad) * speed,
                Mathf.Sin(launchAngleRad) * speed,
                direction.z * Mathf.Cos(launchAngleRad) * speed
        );

            rb.velocity = velocity;
            rb.useGravity = true;
        }
    }

    private void InitializeUnitData(AllCardData cardData)
    {
        damage = cardData.damage;

        if (cardData is MAGICInfoData md)
        {
            towerDamage = md.tower_Damage;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine && !other.isTrigger)
        {
            if (!triggerObjs.Contains(other.gameObject) && (targetLayer | (1 << other.gameObject.layer)) == targetLayer)
            {
                triggerObjs.Add(other.gameObject);
            }

            if ((floorLayer | (1 << other.gameObject.layer)) == floorLayer)
            {
                for (int i = 0; i < triggerObjs.Count; i++)
                {
                    CheckDamagable(triggerObjs[i].gameObject);
                }
                ExplosionParticle();
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (photonView.IsMine && triggerObjs.Contains(other.gameObject))
        {
            triggerObjs.Remove(other.gameObject);
        }
    }

    private void CheckDamagable(GameObject other)
    {
        GameObject childObj = null;

        if (other.transform.childCount > 0)
        {
            childObj = other.transform.GetChild(0).gameObject;
        }

        if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            CheckRange(damagable, other, other.layer);
        }
        else if (childObj != null && childObj.TryGetComponent<IDamagable>(out IDamagable childDamagable))
        {
            CheckRange(childDamagable, childObj, childObj.layer);
        }
        Debug.Log(other.name);
        Debug.Log(other.layer);
    }

    private void CheckRange(IDamagable damagable, GameObject other, int layer)
    {
        PhotonView targetPV = other.GetComponent<PhotonView>();
        if (targetPV == null) return;

        int targetViewID = targetPV.ViewID;
        if (layer == 9)
        {
            photonView.RPC("RPC_SendDamage", RpcTarget.Others, towerDamage, targetViewID);
        }
        else
        {
            photonView.RPC("RPC_SendDamage", RpcTarget.Others, damage, targetViewID);
        }
    }

    [PunRPC]
    public void RPC_SendDamage(int damage, int targetViewID)
    {
        PhotonView targetPV = PhotonView.Find(targetViewID);
        if (targetPV != null)
        {
            IDamagable targetDamagable = targetPV.GetComponent<IDamagable>();
            targetDamagable?.GetDamage(damage);
        }
    }

    private void ExplosionParticle()
    {
        PhotonNetwork.Instantiate("ExplosionFireballSharpFire", transform.position, transform.rotation);
    }
}
