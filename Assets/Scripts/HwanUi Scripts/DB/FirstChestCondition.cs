using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstChestCondition : MonoBehaviour
{
    public CollectionCardInfo collectionCardInfo;
    private void Start()
    {
        CheckNewbie();
        print("oooooo");
    }
    public void CheckNewbie()
    {
        if(SettingCardInfoManager.instance.IsNewbie())
        {

        }
        else
        {
            print("asdf");
            collectionCardInfo.SelectCardOrderByGrade();
            StartManager.m_Instance.InitializeCollectionImage();
        }
        StartManager.m_Instance.InitList();
    }
}
