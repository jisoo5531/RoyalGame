using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamagedTest : MonoBehaviour
{
    public GameObject enemy;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            enemy.GetComponent<UnitCanvasInfo>().GetDamage(10);
        }
    }
}
