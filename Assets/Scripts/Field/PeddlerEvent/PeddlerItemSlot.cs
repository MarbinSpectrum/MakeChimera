using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// : ����� ������ �Ǹ� ������ ����
public class PeddlerItemSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private Image itemImg;
    [SerializeField] private RequireSlot slotObj;
    [SerializeField] private Transform content;

    //Ȱ��ȭ/��Ȱ��ȭ�� ������Ʈ
    [SerializeField] private GameObject offObj;
    [SerializeField] private GameObject onObj;

    private List<RequireSlot> slotList = new List<RequireSlot>();
    private string thisItem;
    private ItemClass itemClass = new ItemClass();

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ����� ������ ������ ���
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetItem(string itemDataName)
    {
        PeddlerManager peddlerManager = PeddlerManager.Instance;
        ItemManager itemManager = ItemManager.Instance;

        //����� ���������� ��ϵǾ��ִ��� �˻�
        if (peddlerManager.Items.ContainsKey(itemDataName) == false)
        {
            Debug.LogError($"{itemDataName}�� ����� ���������� ����� �Ǿ����� �ʽ��ϴ�.");
            return;
        }
        thisItem = itemDataName;

        //�ش� ������ �����͸� �����´�.
        ItemData itemData = null;
        if (ItemManager.IsItemType(itemDataName) == ItemType.equitment)
            itemData = itemManager.equipment_Items[itemDataName];
        else if (ItemManager.IsItemType(itemDataName) == ItemType.material)
            itemData = itemManager.material_Items[itemDataName];

        //�������� �̹���
        Sprite itemSprite = itemData.itemSprite;

        //�������� ǥ�û� �̸�
        string showItemName = itemData.itemName;

        //�̹��� ���
        itemNameText.text = showItemName;
        itemImg.sprite = itemSprite;

        //���Կ� �ʿ��� ������ ǥ��
        PeddlerItem peddlerItem = peddlerManager.Items[itemDataName];
        List<RequireItemClass> requireItems = peddlerItem.requireItem;
        SetCostList(requireItems);

        //������ ���� ���
        itemClass.itemDataName = showItemName;
        itemClass.itemType = ItemManager.IsItemType(itemDataName);
        itemClass.itemNum = 1;
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ����� ������ �������� ����Ʈ ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void SetCostList(List<RequireItemClass> requireItems)
    {
        //�߰��� �����ؾ��� ���� ���� ����
        int createNum = requireItems.Count - slotList.Count;
        for (int i = 0; i < createNum; i++)
        {
            //������ �߰�
            RequireSlot obj = Instantiate(slotObj);
            obj.transform.parent = content;
            obj.transform.localScale = new Vector3(1, 1, 1);
            slotList.Add(obj);
        }

        for (int i = 0; i < slotList.Count; i++)
        {
            //���� ������ �ִ� ������ �̿��ؼ� ����ǥ��
            if (i >= requireItems.Count)
                slotList[i].gameObject.SetActive(false);
            else
            {
                string itemDataName = requireItems[i].item;
                int itemNum = requireItems[i].itemNum;

                slotList[i].SetItemData(itemDataName, itemNum);
                slotList[i].gameObject.SetActive(true);
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���� Ȱ��/��Ȱ��ȭ
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnOffSlot(bool state)
    {
        offObj.SetActive(!state);
        onObj.SetActive(state);
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���� ������ ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void BuyItem()
    {
        if (thisItem == string.Empty)
            return;
        PeddlerManager peddlerManager = PeddlerManager.Instance;
        PeddlerItem item = peddlerManager.Items[thisItem];
        if(item.BuyItem())
        {
            thisItem = "";
            OnOffSlot(false);
        }
        else
        {
            PeddlerUI.OnOffNoResource(true);
        }
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���� ������ Ŭ���� ������ ���� ǥ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ShowItemData()
    {
        PeddlerUI.ShowItem(itemClass);
    }
}
