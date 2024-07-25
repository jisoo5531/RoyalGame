using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballTest : MonoBehaviour
{
    public LayerMask mapLayer;

    private void OnTriggerEnter(Collider other)
    {
        if ((mapLayer | (1 << other.gameObject.layer)) != mapLayer)
        {
            return;
        }
        Debug.Log("¶¥¸Â¾Ò´Ù");
        transform.parent.GetComponent<MagicTest>().OnCollider();
        Destroy(gameObject);
        return;

    }
}
