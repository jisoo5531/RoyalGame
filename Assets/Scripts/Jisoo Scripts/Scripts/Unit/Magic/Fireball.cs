using Photon.Pun;
using UnityEngine;

public class Fireball : MonoBehaviourPunCallbacks
{
    public int damage { get; private set; }
    public Vector3 targetPos;

    public LayerMask floorLayer;
    public LayerMask targetLayer;
    private float speed = 35;
    private float launchAngle = 15f;
    private Rigidbody rb;
    Vector3 direction;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            InitializeUnitData(UnitSpawner.instance.selectedUnit);
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine && (targetLayer | (1 << other.gameObject.layer)) == targetLayer)
        {
            Debug.Log(other.name);
        }

        if (photonView.IsMine && (floorLayer | (1 << other.gameObject.layer)) == floorLayer)
        {
            ExplosionParticle();
            photonView.RPC("DestroyObj", RpcTarget.All);
        }
    }

    [PunRPC]
    private void DestroyObj()
    {
        Destroy(gameObject);
    }

    private void ExplosionParticle()
    {
        PhotonNetwork.Instantiate("ExplosionFireballSharpFire", transform.position, transform.rotation);
    }
}
