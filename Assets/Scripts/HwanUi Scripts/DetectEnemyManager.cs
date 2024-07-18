using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DetectEnemyManager : MonoBehaviour
{
    #region public 변수
    public static DetectEnemyManager instance;
    public GameObject[] towerArr;
    public GameObject[] enemyUnitArr;
    public Transform enemyTower;
    #endregion

    Transform enemyUnit;

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

    public void CheckDetectEnemy(float detectionRange, Transform character, TargetFollowUnit targetFollowUnit, bool isMove, AttackTarget thisAttackTarget)
    {
        int towerIndex = CheckEnemyDistance(character, towerArr);
        int unitIndex = -1;

        if (thisAttackTarget == AttackTarget.All)
        {
            unitIndex = CheckEnemyDistance(character, enemyUnitArr);
        }

        Transform enemyTarget = null;
        float enemyDistance = float.MaxValue;

        if (towerArr[towerIndex] != null)
        {
            enemyTarget = towerArr[towerIndex].transform;
            enemyDistance = Vector3.Distance(enemyTarget.position, character.position);

            if(targetFollowUnit.target == null) // 버그 유발 가능성 있는 코드
            {
                targetFollowUnit.target = enemyTarget;
                targetFollowUnit.targetCollider = targetFollowUnit.target?.GetComponent<Collider>();

                if (isMove)
                {
                    PathRequestManager.RequestPath(character.position, targetFollowUnit.target.position, targetFollowUnit.OnPathFound);
                    targetFollowUnit.isMove = true;
                }
                return;
            }
        }

        if (unitIndex != -1)
        {
            Transform enemyUnitTarget = enemyUnitArr[unitIndex].transform;
            float enemyUnitDistance = Vector3.Distance(enemyUnitTarget.position, character.position);
            if (enemyDistance >= enemyUnitDistance)
            {
                enemyTarget = enemyUnitTarget;
                enemyDistance = enemyUnitDistance;
            }
        }

        if (enemyTarget != null && enemyDistance <= detectionRange && targetFollowUnit.target != enemyTarget)
        {
            print(enemyTarget);
            targetFollowUnit.target = enemyTarget;
            targetFollowUnit.targetCollider = targetFollowUnit.target?.GetComponent<Collider>();

            if (isMove)
            {
                PathRequestManager.RequestPath(character.position, targetFollowUnit.target.position, targetFollowUnit.OnPathFound);
                targetFollowUnit.isMove = true;
            }
        }
    }
}
