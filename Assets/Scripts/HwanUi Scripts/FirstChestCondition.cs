using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstChestCondition : MonoBehaviour
{
    public CollectionCardInfo collectionCardInfo;
    // 노란색 배경
    public Sprite yellowButton;
    // 회색 배경
    public Sprite grayButton;
    // 뉴비 상자 이미지
    public Sprite firstBoxSprite;

    public Image box_Button;
    public Image boxImage;

    private void Start()
    {
        CheckNewbie();
    }
    public void CheckNewbie()
    {
        if(SettingCardInfoManager.instance.IsNewbie())
        {
            for(int i = 0; i<SettingCardInfoManager.instance.slots.Length; i++)
            {
                SettingCardInfoManager.instance.slots[i].SetActive(false);
            }
            boxImage.sprite = firstBoxSprite;
            boxImage.ImageTransparent(1f);

            box_Button.sprite = yellowButton;
        }
        else
        {
            for (int i = 0; i < SettingCardInfoManager.instance.slots.Length; i++)
            {
                SettingCardInfoManager.instance.slots[i].SetActive(true);
            }
            boxImage.sprite = null;
            boxImage.ImageTransparent(0f);
            box_Button.sprite = grayButton;

            collectionCardInfo.SelectCardOrderByGrade();
            StartManager.m_Instance.InitializeCollectionImage();
        }
    }
}
