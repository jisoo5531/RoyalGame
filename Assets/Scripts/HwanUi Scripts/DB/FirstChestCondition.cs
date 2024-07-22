using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstChestCondition : MonoBehaviour
{
    public CollectionCardInfo collectionCardInfo;
    private void Start()
    {
        CheckNewbie();
    }
    public void CheckNewbie()
    {
        if(SettingCardInfoManager.instance.IsNewbie())
        {

        }
        else
        {
            collectionCardInfo.SelectCardOrderByGrade();
            StartManager.m_Instance.InitializeCollectionImage();
        }
    }
}
