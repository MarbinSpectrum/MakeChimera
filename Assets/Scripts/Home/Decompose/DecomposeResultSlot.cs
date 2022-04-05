using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DecomposeResultSlot : MonoBehaviour
{
    [SerializeField] private Image imgIcon;
    [SerializeField] private TextMeshProUGUI itemNumText;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 분해 아이템 정보표시
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetData(string itemDataName, int num)
    {
        ItemManager itemManager = ItemManager.Instance;

        //아이템 데이터를 가져온다.
        ItemData itemData = null;
        if (ItemManager.IsItemType(itemDataName) == ItemType.equitment)
            itemData = itemManager.equipment_Items[itemDataName];
        else if (ItemManager.IsItemType(itemDataName) == ItemType.material)
            itemData = itemManager.material_Items[itemDataName];

        //해당 아이템의 이미지를 가져온다.
        Sprite itemSprite = itemData.itemSprite;

        //이미지 등록
        itemNumText.text = num.ToString();
        imgIcon.sprite = itemSprite;
    }
}
