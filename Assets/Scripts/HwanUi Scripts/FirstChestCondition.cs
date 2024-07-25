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
            box_Button.gameObject.GetComponent<Button>().interactable = true;
            boxImage.sprite = firstBoxSprite;
            boxImage.ImageTransparent(1f);

            box_Button.sprite = yellowButton;
        }
        else
        {
            box_Button.gameObject.GetComponent<Button>().interactable = false;
            boxImage.sprite = null;
            boxImage.ImageTransparent(0f);
            box_Button.sprite = grayButton;

            collectionCardInfo.SelectCardOrderByGrade();
            StartManager.m_Instance.InitializeCollectionImage();
        }
    }
}
