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

    //���� ���� ����
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
    /// : ������ ������ ǥ��
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
                //����������� Ȯ�� ��
                //��� �����͸� �޾ƿ´�.
                itemData = ItemManager.Instance.equipment_Items[itemClass.itemDataName];
            }

            if (itemData == null)
                return;

            //������ �������� ǥ��
            imgIcon.sprite = itemData.itemSprite;
            imgIcon.color = Color.white;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���Կ� �ش��ϴ� �ִϸ��̼� ���� 
    ///      Show : ��Ÿ���� �ִϸ��̼�
    ///      Hide : ������� �ִϸ��̼�
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ShowAni(string ani)
    {
        if (animator)
        {
            animator.SetTrigger(ani);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ������ ������ ǥ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ShowItem()
    {
        if (thisItemClass == null)
            return;
        DecomposeUI.ShowItem(thisItemClass);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ������ ������ ������ Ŭ���� �̺�Ʈ
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ClickSlot()
    {
        if (thisItemClass == null)
            return;

        //����/���� ���
        select = !select;

        if (select)
        {
            //���û��¸� ���ø���Ʈ���� �߰�
            DecomposeUI.AddSelectList(thisItemClass);
        }
        else
        {
            //���û��¸� ���¸���Ʈ���� ����
            DecomposeUI.RemoveSelectList(thisItemClass);
        }

        //������ ����ǥ��
        ShowItem();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : Ʃ�丮�� Ŭ��ǥ�� Ȱ��/��Ȱ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnOffTutorialBox(bool state)
    {
        tutorialBox.SetActive(state);
    }
}
