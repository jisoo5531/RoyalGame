using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DetectEnemyManager : MonoBehaviour
{
    #region public 변수
    public static DetectEnemyManager instance;

    public List<GameObject> towerList = new List<GameObject>();
    public List<GameObject> enemyList = new List<GameObject>();
    #endregion

    Transform enemyUnit;

    private void Awake()
    {
        instance = this;
    }

    public int CheckEnemyDistance(Transform currentTransform, List<GameObject> enemyList)
    {
        float minDistance = 700f;
        int objIndex = 0;

        try
        {
            if (enemyList.Count > 0)
            {
                for (int i = 0; i < enemyList.Count; i++)
                {
                    if (enemyList[i] != null)
                    {
                        float distance = Vector3.Distance(enemyList[i].transform.position, currentTransform.position);

                        if (minDistance > distance)
                        {
                            minDistance = distance;
                            objIndex = i;
                        }
                    }
                }
                return objIndex;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        return -1;
    }

    public void FirstMovePath(Transform character, TargetFollowUnit targetFollowUnit)
    {
        PathRequestManager.RequestPath(character.position, targetFollowUnit.target.position, targetFollowUnit.OnPathFound);
    }

    public void CheckDetectEnemy(float detectionRange, Transform character, TargetFollowUnit targetFollowUnit, bool isMove, string thisAttackTarget)
    {
        try
        {
            if (targetFollowUnit == null) return;

            int towerIndex = CheckEnemyDistance(character, towerList);
            int unitIndex = -1;

            if (!thisAttackTarget.Equals("건물"))
            {
                unitIndex = CheckEnemyDistance(character, enemyList);
            }

            Transform enemyTarget = null;
            float enemyDistance = float.MaxValue;
            if (towerList[towerIndex] != null)
            {
                enemyTarget = towerList[towerIndex].transform;
                enemyDistance = Vector3.Distance(enemyTarget.position, character.position);

                if (targetFollowUnit.target == null)
                {
                    targetFollowUnit.target = enemyTarget;
                    targetFollowUnit.targetCollider = targetFollowUnit.target?.GetComponent<Collider>();

                    if (isMove)
                    {
                        PathRequestManager.RequestPath(character.position, targetFollowUnit.target.position, targetFollowUnit.OnPathFound);
                        return;
                    }
                }
            }

            if (unitIndex != -1)
            {
                Transform enemyUnitTarget = enemyList[unitIndex].transform;
                float enemyUnitDistance = Vector3.Distance(enemyUnitTarget.position, character.position);
                if (enemyDistance >= enemyUnitDistance)
                {
                    enemyTarget = enemyUnitTarget;
                    enemyDistance = enemyUnitDistance;
                }
            }

            if (enemyTarget != null && enemyDistance <= detectionRange && targetFollowUnit.target != enemyTarget)
            {
                targetFollowUnit.target = enemyTarget;
                targetFollowUnit.targetCollider = targetFollowUnit.target?.GetComponent<Collider>();

                if (isMove)
                {
                    PathRequestManager.RequestPath(character.position, targetFollowUnit.target.position, targetFollowUnit.OnPathFound);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
}
