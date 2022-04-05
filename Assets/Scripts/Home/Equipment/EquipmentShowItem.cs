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
    /// : �����츦 ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnOffWindow(bool state)
    {
        body.SetActive(state);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �ƹ��͵� ������ ǥ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ShowEmpty()
    {
        showItem.SetActive(false);
        showEmpty.SetActive(true);
        OnOffWindow(true);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ������ ǥ��
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

            //���� �÷��̾� ��� ��������
            ItemClass nowItem = playData.GetNowEquip(equipmentType);
            if(nowItem == null)
            {
                nowItem = new ItemClass();
                nowItem.attackMin = itemClass.attackMin;
                nowItem.attackMax = itemClass.attackMax;
                nowItem.criticalDamage = itemClass.criticalDamage;
                nowItem.criticalPer = itemClass.criticalPer;
            }

            //������ �ɷ�ġ ǥ��
            string itemStat = "";
            AddText(ref itemStat, "�ּ� ���ݷ�", itemClass.attackMin, nowItem.attackMin);

            Vector2Int attackMax = itemData.attackMax;
            AddText(ref itemStat, "�ִ� ���ݷ�", itemClass.attackMax, nowItem.attackMax);

            Vector2Int criticalDamage = itemData.criticalDamage;
            AddText(ref itemStat, "ġ��Ÿ ������", itemClass.criticalDamage, nowItem.criticalDamage);

            Vector2Int criticalPer = itemData.criticalPer;
            AddText(ref itemStat, "ġ��Ÿ Ȯ��", itemClass.criticalPer, nowItem.criticalPer);
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

            //������ �����ġǥ��
            List<string> spawnFields = ItemManager.GetSpawnField(itemData);

            string itemDropPos = "ȹ����\n";
            itemDropPos += $"<size={fontSize * 0.7f}><color=#000000>";
            foreach (string fieldName in spawnFields)
            {
                itemDropPos += $"<size={fontSize * 0.8f}>�� </size>";
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
    /// : ������ ����
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
    /// : ������ ����
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
