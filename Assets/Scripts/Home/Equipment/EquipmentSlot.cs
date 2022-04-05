using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] private Image imgIcon;
    [SerializeField] Sprite defaultIcon;

    [SerializeField] private GameObject itemNum;
    [SerializeField] private TextMeshProUGUI itemNumText;
    [SerializeField] private Animator animator;
    [System.NonSerialized] public ItemClass thisItemClass;
    [SerializeField] private GameObject tutorialBox;

    private bool nowItem;
    public void SetData(ItemClass itemClass, bool nowItem)
    {
        if (itemClass == null)
        {
            thisItemClass = null;
            if (defaultIcon != null)
            {
                imgIcon.color = new Color(0.3f, 0.3f, 0.3f, 1);
                imgIcon.sprite = defaultIcon;
            }
            else
            {
                imgIcon.color = new Color(0, 0, 0, 0);
            }

        }
        else
        {
            thisItemClass = itemClass;

            ItemManager itemManager = ItemManager.Instance;
            ItemData itemData = null;
            if (ItemManager.IsItemType(itemClass.itemDataName) == ItemType.equitment)
            {
                itemData = ItemManager.Instance.equipment_Items[itemClass.itemDataName];
                if (itemNum != null)
                {
                    itemNum.SetActive(false);
                }
            }
            else if (ItemManager.IsItemType(itemClass.itemDataName) == ItemType.material)
            {
                itemData = ItemManager.Instance.material_Items[itemClass.itemDataName];
                if (itemNum != null)
                {
                    itemNum.SetActive(true);
                    itemNumText.text = thisItemClass.itemNum.ToString();
                }
            }

            if (itemData == null)
                return;

            imgIcon.sprite = itemData.itemSprite;
            imgIcon.color = Color.white;

            this.nowItem = nowItem;
        }
    }

    public void ShowAni(string ani)
    {
        if(animator)
        {
            animator.SetTrigger(ani);
        }
    }

    public void ShowItem()
    {
        if (thisItemClass == null)
            return;
        EquipmentUI.ShowItem(thisItemClass, nowItem);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 튜토리얼 클릭표시 활성/비활성
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnOffTutorialBox(bool state)
    {
        tutorialBox.SetActive(state);
    }
}
