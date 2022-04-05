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
    /// : �ش� ���Կ� �������� ������ش�.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetItemData(string itemName,int requireNum)
    {
        //�κ��丮�� �ִ� ������ ���� ��������
        ItemData itemData = ItemManager.Instance.material_Items[itemName];
        int hasMaterial = Inventory.GetMaterialNum(itemName);

        //�ʿ䰹���� ǥ��
        string requireText = $"{hasMaterial} / {requireNum}";
        textNum.text = requireText;
        if (hasMaterial < requireNum)
            textNum.color = Color.red;
        else
            textNum.color = Color.white;

        //������ ������ �߰�
        Sprite sprite = itemData.itemSprite;
        imgIcon.sprite = sprite;

        itemClass = new ItemClass(itemName, ItemType.material, 1);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ������ Ŭ�������� ������ �̺�Ʈ
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ClickSlot()
    {
        equipmentShow.ShowItem(itemClass);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : Ʃ�丮�� ���Ȯ�� Ȱ��/��Ȱ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnOffTutorialRequireBox(bool state)
    {
        if(tutorialBox)
        {
            tutorialBox.SetActive(state);
        }
    }
}
