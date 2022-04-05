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
    /// : ���� ������ ����ǥ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetData(string itemDataName, int num)
    {
        ItemManager itemManager = ItemManager.Instance;

        //������ �����͸� �����´�.
        ItemData itemData = null;
        if (ItemManager.IsItemType(itemDataName) == ItemType.equitment)
            itemData = itemManager.equipment_Items[itemDataName];
        else if (ItemManager.IsItemType(itemDataName) == ItemType.material)
            itemData = itemManager.material_Items[itemDataName];

        //�ش� �������� �̹����� �����´�.
        Sprite itemSprite = itemData.itemSprite;

        //�̹��� ���
        itemNumText.text = num.ToString();
        imgIcon.sprite = itemSprite;
    }
}
