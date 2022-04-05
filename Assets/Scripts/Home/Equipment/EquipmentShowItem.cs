using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentShowItem : MonoBehaviour
{
    [SerializeField] private GameObject putOnBtn;
    [SerializeField] private GameObject takeOffBtn;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject showItem;
    [SerializeField] private GameObject showEmpty;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemExplainText;
    [SerializeField] private TextMeshProUGUI itemStatText;
    [SerializeField] private bool compareNowItem = false;

    private ItemClass nowItemClass;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 윈도우를 ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnOffWindow(bool state)
    {
        body.SetActive(state);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 아무것도 없을때 표시
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ShowEmpty()
    {
        showItem.SetActive(false);
        showEmpty.SetActive(true);
        OnOffWindow(true);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 아이템 표시
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ShowItem(ItemClass itemClass, bool isNowItem = false)
    {
        PlayData playData = DataManager.Instance.playData;

        void AddText(ref string a, string b, float change, float now)
        {
            if (change != 0 && now != 0)
            {
                string temp = "";
                float abs = Mathf.Abs(change - now);
                if(compareNowItem)
                {
                    if (change < now)
                        temp = $"{b} : {change} <color=#0089e9>(-{abs})</color> \n";
                    else if (change > now)
                        temp = $"{b} : {change} <color=red>(+{abs})</color> \n";
                    else
                        temp = $"{b} : {change} \n";
                }
                else
                    temp = $"{b} : {change} \n";
                a += temp;
            }
        }

        showItem.SetActive(true);
        showEmpty.SetActive(false);
        nowItemClass = itemClass;
        ItemManager itemManager = ItemManager.Instance;
        if (ItemManager.IsItemType(itemClass.itemDataName) == ItemType.equitment)
        {
            EquipmentData itemData = (EquipmentData)ItemManager.Instance.equipment_Items[itemClass.itemDataName];
            EquipmentType equipmentType = itemData.equipmentType;
            Sprite itemSprite = itemData.itemSprite;
            
            itemIcon.sprite = itemSprite;
            itemNameText.text = itemData.itemName;
            itemExplainText.text = itemData.itemExplain;

            //현재 플레이어 장비 가져오기
            ItemClass nowItem = playData.GetNowEquip(equipmentType);
            if(nowItem == null)
            {
                nowItem = new ItemClass();
                nowItem.attackMin = itemClass.attackMin;
                nowItem.attackMax = itemClass.attackMax;
                nowItem.criticalDamage = itemClass.criticalDamage;
                nowItem.criticalPer = itemClass.criticalPer;
            }

            //아이템 능력치 표시
            string itemStat = "";
            AddText(ref itemStat, "최소 공격력", itemClass.attackMin, nowItem.attackMin);

            Vector2Int attackMax = itemData.attackMax;
            AddText(ref itemStat, "최대 공격력", itemClass.attackMax, nowItem.attackMax);

            Vector2Int criticalDamage = itemData.criticalDamage;
            AddText(ref itemStat, "치명타 데미지", itemClass.criticalDamage, nowItem.criticalDamage);

            Vector2Int criticalPer = itemData.criticalPer;
            AddText(ref itemStat, "치명타 확률", itemClass.criticalPer, nowItem.criticalPer);
            itemStatText.text = itemStat;

            if (putOnBtn)
                putOnBtn.SetActive(!isNowItem);
            if (takeOffBtn)
                takeOffBtn.SetActive(isNowItem);
        }
        else if (ItemManager.IsItemType(itemClass.itemDataName) == ItemType.material)
        {
            ItemData itemData = ItemManager.Instance.material_Items[itemClass.itemDataName];
            Sprite sprite = itemData.itemSprite;
            float fontSize = itemStatText.fontSize;

            itemIcon.sprite = sprite;
            itemNameText.text = itemData.itemName;
            itemExplainText.text = itemData.itemExplain;

            //아이템 드랍위치표시
            List<string> spawnFields = ItemManager.GetSpawnField(itemData);

            string itemDropPos = "획득경로\n";
            itemDropPos += $"<size={fontSize * 0.7f}><color=#000000>";
            foreach (string fieldName in spawnFields)
            {
                itemDropPos += $"<size={fontSize * 0.8f}>· </size>";
                itemDropPos += fieldName;
                itemDropPos += "\n";
            }
            itemDropPos += "</color></size>";
            itemStatText.text = itemDropPos;

            if (putOnBtn)
                putOnBtn.SetActive(false);
            if (takeOffBtn)
                takeOffBtn.SetActive(false);
        }
        OnOffWindow(true);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 아이템 장착
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void PutOnItem()
    {
        PlayData playData = DataManager.Instance.playData;
        if (playData.PutOnItem(nowItemClass))
        {
            OnOffWindow(false);
            EquipmentUI.RefreshMenu();
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 아이템 해제
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void TakeOffItem()
    {
        PlayData playData = DataManager.Instance.playData;
        if (playData.TakeOffItem(nowItemClass))
        {
            OnOffWindow(false);
            EquipmentUI.RefreshMenu();
        }
    }
}
