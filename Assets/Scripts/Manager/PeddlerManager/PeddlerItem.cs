using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PeddlerItem : SerializedMonoBehaviour
{
    [LabelText("아이템 이름")]
    public string itemDataName;

    [LabelText("등장 수치")]
    public int itemSpawnPoint;

    [Title("필요재료")]
    [LabelText("필요재료/갯수")]
    [SerializeField] public List<RequireItemClass> requireItem = new List<RequireItemClass>();

    public bool BuyItem()
    {
        if (ItemData.CanMake(requireItem) == false)
            return false;
        foreach (RequireItemClass require in requireItem)
        {
            string item = require.item;
            int num = require.itemNum;
            if (Inventory.RemoveItem(item, num) == false)
                return false;
        }

        ItemManager itemManager = ItemManager.Instance;
        ItemType itemType = ItemManager.IsItemType(itemDataName);
        ItemClass itemClass = new ItemClass();

        itemClass.itemDataName = itemDataName;
        itemClass.itemType = itemType;
        itemClass.itemNum = 1;

        if (itemType == ItemType.equitment)
        {
            EquipmentData equipmentData = (EquipmentData)itemManager.equipment_Items[itemDataName];
            itemClass.criticalPer = Random.Range(equipmentData.criticalPer.x, equipmentData.criticalPer.y + 1);
            itemClass.criticalDamage = Random.Range(equipmentData.criticalDamage.x, equipmentData.criticalDamage.y + 1);
            itemClass.attackMax = Random.Range(equipmentData.attackMax.x, equipmentData.attackMax.y + 1);
            itemClass.attackMin = Random.Range(equipmentData.attackMin.x, equipmentData.attackMin.y + 1);
            itemClass.attackMin = Mathf.Min(itemClass.attackMin, itemClass.attackMin);
        }

        Debug.Log(itemClass.itemDataName);

        Inventory.AddItem(itemClass);

        return true;
    }


}
