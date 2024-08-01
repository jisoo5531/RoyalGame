using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstChestCondition : MonoBehaviour
{
    public CollectionCardInfo collectionCardInfo;
    public Sprite yellowButton;
    public Sprite grayButton;
    public Sprite firstBoxSprite;
    public GameObject collectionObj;
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
            collectionObj.SetActive(false);
        }
        else
        {
            box_Button.gameObject.GetComponent<Button>().interactable = false;
            boxImage.sprite = null;
            boxImage.ImageTransparent(0f);
            box_Button.sprite = grayButton;
            collectionObj.SetActive(true);
            collectionCardInfo.SelectCardOrderByGrade();
            StartManager.m_Instance.InitializeCollectionImage();
        }
    }
}
