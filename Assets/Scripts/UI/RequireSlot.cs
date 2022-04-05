using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RequireSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textNum;
    [SerializeField] private Image imgIcon;
    [SerializeField] private EquipmentShowItem equipmentShow;
    [SerializeField] private GameObject tutorialBox;

    private ItemClass itemClass;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 해당 슬롯에 아이템을 등록해준다.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetItemData(string itemName,int requireNum)
    {
        //인벤토리에 있는 아이템 갯수 가져오기
        ItemData itemData = ItemManager.Instance.material_Items[itemName];
        int hasMaterial = Inventory.GetMaterialNum(itemName);

        //필요갯수를 표시
        string requireText = $"{hasMaterial} / {requireNum}";
        textNum.text = requireText;
        if (hasMaterial < requireNum)
            textNum.color = Color.red;
        else
            textNum.color = Color.white;

        //아이템 아이콘 추가
        Sprite sprite = itemData.itemSprite;
        imgIcon.sprite = sprite;

        itemClass = new ItemClass(itemName, ItemType.material, 1);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 슬롯을 클릭했을때 나오는 이벤트
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ClickSlot()
    {
        equipmentShow.ShowItem(itemClass);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 튜토리얼 재료확인 활성/비활성
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnOffTutorialRequireBox(bool state)
    {
        if(tutorialBox)
        {
            tutorialBox.SetActive(state);
        }
    }
}
