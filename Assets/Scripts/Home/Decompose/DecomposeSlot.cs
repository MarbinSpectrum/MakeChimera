using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecomposeSlot : MonoBehaviour
{
    [SerializeField] private Image imgIcon;
    [SerializeField] Sprite defaultIcon;

    [SerializeField] private Image selectImg;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject tutorialBox;

    private ItemClass thisItemClass;

    //슬롯 선택 여부
    private bool m_select = false;
    public bool select
    {
        get { return m_select; }
        set
        {
            m_select = value;
            selectImg.enabled = m_select;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 아이템 정보를 표시
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetData(ItemClass itemClass)
    {
        select = false;
        if (itemClass == null)
        {
            thisItemClass = null;
            imgIcon.color = new Color(0, 0, 0, 0);
        }
        else
        {
            thisItemClass = itemClass;

            ItemManager itemManager = ItemManager.Instance;
            ItemData itemData = null;

            if (ItemManager.IsItemType(itemClass.itemDataName) == ItemType.equitment)
            {
                //장비데이터인지 확인 후
                //장비 데이터를 받아온다.
                itemData = ItemManager.Instance.equipment_Items[itemClass.itemDataName];
            }

            if (itemData == null)
                return;

            //아이템 아이콘을 표시
            imgIcon.sprite = itemData.itemSprite;
            imgIcon.color = Color.white;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 슬롯에 해당하는 애니메이션 실행 
    ///      Show : 나타나는 애니메이션
    ///      Hide : 사라지는 애니메이션
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ShowAni(string ani)
    {
        if (animator)
        {
            animator.SetTrigger(ani);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 아이템 정보를 표시
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ShowItem()
    {
        if (thisItemClass == null)
            return;
        DecomposeUI.ShowItem(thisItemClass);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 슬롯의 아이템 아이콘 클릭시 이벤트
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ClickSlot()
    {
        if (thisItemClass == null)
            return;

        //선택/비선택 토글
        select = !select;

        if (select)
        {
            //선택상태면 선택리스트에서 추가
            DecomposeUI.AddSelectList(thisItemClass);
        }
        else
        {
            //비선택상태면 선태리스트에서 제거
            DecomposeUI.RemoveSelectList(thisItemClass);
        }

        //아이템 정보표시
        ShowItem();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 튜토리얼 클릭표시 활성/비활성
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnOffTutorialBox(bool state)
    {
        tutorialBox.SetActive(state);
    }
}
