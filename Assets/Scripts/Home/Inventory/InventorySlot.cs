using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image imgIcon;
    [SerializeField] private TextMeshProUGUI numText;
    public void SetData(ItemClass itemClass)
    {
        ItemData itemData = ItemManager.Instance.material_Items[itemClass.itemDataName];
        Sprite sprite = itemData.itemSprite;
        imgIcon.sprite = sprite;
        numText.text = itemClass.itemNum.ToString();
    }
}
