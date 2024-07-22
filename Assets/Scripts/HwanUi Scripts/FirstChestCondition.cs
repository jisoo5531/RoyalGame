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
        Debug.Log("체크");
        // 처음 게임을 들어오면
        if(SettingCardInfoManager.instance.IsNewbie())
        {
            Debug.Log("체크1");
            // 상자 생성
            boxImage.sprite = firstBoxSprite;
            boxImage.ImageTransparent(1f);

            box_Button.sprite = yellowButton;
        }
        else
        {
            boxImage.sprite = null;
            boxImage.ImageTransparent(0f);
            box_Button.sprite = grayButton;

            // 상자 없애기
            collectionCardInfo.SelectCardOrderByGrade();
            StartManager.m_Instance.InitializeCollectionImage();
        }
    }
}
