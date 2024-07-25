using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMagic : MonoBehaviour
{
    public GameObject fireBallPrefab;
    public Transform startTrans;
    public Transform targetTrans;
    public Vector3 clickPos;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {                                
                clickPos = hit.point;
                clickPos.y = -1;
                SpawnFireBall();
            }
        }
    }

    private void SpawnFireBall()
    {        

        GameObject fireball = Instantiate(fireBallPrefab, startTrans);
        Projectile fireballProjectile = fireball.AddComponent<Projectile>();
        fireballProjectile.targetPos = clickPos;
    }
}
