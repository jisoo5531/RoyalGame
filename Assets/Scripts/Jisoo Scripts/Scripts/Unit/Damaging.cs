using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damaging : MonoBehaviour
{
    public int damage;

    public Transform target;
    public LayerMask targetLayerMask;
    public Range rangeType;
    public Type unitType;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"¹º°¡ ¸Â¾Ò´Ù {other.gameObject.name}");
        Debug.Log($"{gameObject.name} ÀÌ ¹º°¡ ¸ÂÇû´Ù");
        if ((targetLayerMask | (1 << other.gameObject.layer)) != targetLayerMask)
        {
            if (rangeType == Range.Ranged)
            {

                Destroy(gameObject);
            }
            return;
        }
        

        if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            if (unitType == Type.Magic)
            {
                damagable.GetDamage(damage);

                Destroy(gameObject);

                return;
            }

            Debug.Log("Å×½ºÆ®.");
            if (rangeType == Range.Ranged)
            {                
                if (other.gameObject.name == target.gameObject.name)
                {
                    damagable.GetDamage(damage);
                    Destroy(gameObject);
                }                

                return;
            }

            damagable.GetDamage(damage);
        }

        if (unitType == Type.Magic)
        {
            transform.parent.GetComponent<MagicTest>().OnCollider();
            Destroy(gameObject);
            return;
        }
    }
}
