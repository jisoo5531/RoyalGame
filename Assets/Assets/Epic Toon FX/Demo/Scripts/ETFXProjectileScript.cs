using UnityEngine;
using System.Collections;

namespace EpicToonFX
{    
    public class ETFXProjectileScript : MonoBehaviour
    {
        public Transform magicTrans;

        public GameObject impactParticle;
        public GameObject projectileParticle;
        public GameObject muzzleParticle;
        [Header("Adjust if not using Sphere Collider")]
        public float colliderRadius = 1f;
        [Range(0f, 1f)]
        public float collideOffset = 0.15f;

        void Start()
        {
            // 드래그 시 오류
            projectileParticle = Instantiate(projectileParticle, magicTrans.position, magicTrans.rotation);
            projectileParticle.transform.parent = transform;
            if (muzzleParticle)
            {
                muzzleParticle = Instantiate(muzzleParticle, magicTrans.position, magicTrans.rotation);
                Destroy(muzzleParticle, 1.5f);
            }
        }
		
        void FixedUpdate()
        {	
			if (GetComponent<Rigidbody>().velocity.magnitude != 0)
			{
			    transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
			}
			
            RaycastHit hit;
			
            float radius;
            if (transform.GetComponent<SphereCollider>())
                radius = transform.GetComponent<SphereCollider>().radius;
            else
                radius = colliderRadius;

            Vector3 direction = transform.GetComponent<Rigidbody>().velocity;
            if (transform.GetComponent<Rigidbody>().useGravity)
                direction += Physics.gravity * Time.deltaTime;
            direction = direction.normalized;

            float detectionDistance = transform.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime;

            if (Physics.SphereCast(transform.position, radius, direction, out hit, detectionDistance))
            {
                transform.position = hit.point + (hit.normal * collideOffset);

                GameObject impactP = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;

                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
                for (int i = 1; i < trails.Length; i++)
                {
                    ParticleSystem trail = trails[i];

                    if (trail.gameObject.name.Contains("Trail"))
                    {
                        trail.transform.SetParent(null);
                        Destroy(trail.gameObject, 2f);
                    }
                }

                Destroy(projectileParticle, 3f);
                Destroy(impactP, 3.5f);
                Destroy(gameObject);
            }
        }
    }
}