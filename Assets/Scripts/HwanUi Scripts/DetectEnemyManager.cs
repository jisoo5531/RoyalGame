using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 포톤으로 생성할 컴포넌트 매니저
/// </summary>
public class DetectEnemyManager : MonoBehaviour
{
    public static DetectEnemyManager instance;
    public GameObject[] towerArr;
    public GameObject[] enemyUnit;

    private void Awake()
    {
        instance = this;
    }

    public int CheckEnemyDistance(Transform currentTransform, GameObject[] detectObj)
    {
        float minDistance = 700f;
        int objIndex = 0;

        if (detectObj.Length > 0)
        {
            for (int i = 0; i < detectObj.Length; i++)
            {
                float distance = Vector3.Distance(detectObj[i].transform.position, currentTransform.position);

                if (minDistance > distance)
                {
                    minDistance = distance;
                    objIndex = i;
                }
            }
            return objIndex;
        }
        return -1;
    }
}
