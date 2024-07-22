using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCardAddTest : MonoBehaviour
{
    public void UnidAddTest(int number)
    {
        GameManager.m_Instance.uniData[number].cardInfo.unit_CurrentCardCount += 10;
    }
}
