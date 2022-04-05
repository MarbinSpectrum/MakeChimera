using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// : 행상인 상점의 판매 아이템 슬롯
public class PeddlerItemSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private Image itemImg;
    [SerializeField] private RequireSlot slotObj;
    [SerializeField] private Transform content;

    //활성화/비활성화시 오브젝트
    [SerializeField] private GameObject offObj;
    [SerializeField] private GameObject onObj;

    private List<RequireSlot> slotList = new List<RequireSlot>();
    private string thisItem;
    private ItemClass itemClass = new ItemClass();

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 행상인 상점의 아이템 목록
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetItem(string itemDataName)
    {
        PeddlerManager peddlerManager = PeddlerManager.Instance;
        ItemManager itemManager = ItemManager.Instance;

        //행상인 아이템으로 등록되어있는지 검사
        if (peddlerManager.Items.ContainsKey(itemDataName) == false)
        {
            Debug.LogError($"{itemDataName}은 행상인 아이템으로 등록이 되어있지 않습니다.");
            return;
        }
        thisItem = itemDataName;

        //해당 아이템 데이터를 가져온다.
        ItemData itemData = null;
        if (ItemManager.IsItemType(itemDataName) == ItemType.equitment)
            itemData = itemManager.equipment_Items[itemDataName];
        else if (ItemManager.IsItemType(itemDataName) == ItemType.material)
            itemData = itemManager.material_Items[itemDataName];

        //아이템의 이미지
        Sprite itemSprite = itemData.itemSprite;

        //아이템의 표시상 이름
        string showItemName = itemData.itemName;

        //이미지 등록
        itemNameText.text = showItemName;
        itemImg.sprite = itemSprite;

        //구입에 필요한 아이템 표시
        PeddlerItem peddlerItem = peddlerManager.Items[itemDataName];
        List<RequireItemClass> requireItems = peddlerItem.requireItem;
        SetCostList(requireItems);

        //아이템 정보 등록
        itemClass.itemDataName = showItemName;
        itemClass.itemType = ItemManager.IsItemType(itemDataName);
        itemClass.itemNum = 1;
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 행상인 아이템 가격정보 리스트 생성
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void SetCostList(List<RequireItemClass> requireItems)
    {
        //추가로 생성해야할 슬롯 갯수 상정
        int createNum = requireItems.Count - slotList.Count;
        for (int i = 0; i < createNum; i++)
        {
            //슬롯을 추가
            RequireSlot obj = Instantiate(slotObj);
            obj.transform.parent = content;
            obj.transform.localScale = new Vector3(1, 1, 1);
            slotList.Add(obj);
        }

        for (int i = 0; i < slotList.Count; i++)
        {
            //현재 가지고 있는 슬롯을 이용해서 가격표시
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
    /// : 슬롯 활성/비활성화
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnOffSlot(bool state)
    {
        offObj.SetActive(!state);
        onObj.SetActive(state);
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 슬롯 아이템 구입
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
    /// : 슬롯 아이콘 클릭시 아이템 정보 표시
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ShowItemData()
    {
        PeddlerUI.ShowItem(itemClass);
    }
}
