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
    public GameObject[] enemyUnitArr;
    Transform enemyUnit;
    Transform enemyTower;

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

    public void CheckDetectAllEnemy(float detectionRange, Transform character, TargetFollowUnit targetFollowUnit, bool isMove)
    {
        int towerIndex = CheckEnemyDistance(this.transform, towerArr);
        int unitIndex = CheckEnemyDistance(this.transform, enemyUnitArr);

        if (enemyUnitArr[unitIndex] == null && towerArr[towerIndex] == null)
            return;

        enemyTower = towerArr[towerIndex]?.transform;
        enemyUnit = enemyUnitArr[unitIndex]?.transform;

        float distance = Vector3.Distance(enemyUnit.position, transform.position);

        if (distance <= detectionRange && targetFollowUnit.target != enemyUnit)
        {
            targetFollowUnit.target = enemyUnit;
        }
        else if (distance > detectionRange && targetFollowUnit.target != enemyTower)
        {
            targetFollowUnit.target = enemyTower;
        }
        else
        {
            return;
        }
        targetFollowUnit.targetCollider = targetFollowUnit.target?.GetComponent<Collider>();

        if (isMove)
        {
            PathRequestManager.RequestPath(character.position, targetFollowUnit.target.position, targetFollowUnit.OnPathFound);
            targetFollowUnit.isMove = true;
        }
    }

    public void CheckDetectEnemyTower(float detectionRange, Transform character, TargetFollowUnit targetFollowUnit, bool isMove)
    {
        int towerIndex = CheckEnemyDistance(this.transform, towerArr);

        if (towerArr[towerIndex] == null)
            return;

        enemyTower = towerArr[towerIndex]?.transform;

        if (targetFollowUnit.target == enemyTower) return;

        targetFollowUnit.target = enemyTower;
        targetFollowUnit.targetCollider = targetFollowUnit.target?.GetComponent<CharacterController>();

        if (isMove)
        {
            PathRequestManager.RequestPath(character.position, targetFollowUnit.target.position, targetFollowUnit.OnPathFound);
            targetFollowUnit.isMove = true;
        }
    }
}
