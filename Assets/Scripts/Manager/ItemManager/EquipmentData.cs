using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum EquipmentType
{
    head,
    body,
    left_arm,
    right_arm,
    left_leg,
    right_leg,
    ear_ring,
    neck_ring,
    weapon,
    material,
    anything //��� ������
}

[SerializeField]
public class RequireItemClass : System.IEquatable<RequireItemClass>
{
    public string item;
    public int itemNum;

    public bool Equals(RequireItemClass other)
    {
        if (other == null)
            return false;

        RequireItemClass objAsPart = other as RequireItemClass;
        if (objAsPart == null)
            return false;
        else
            return item == other.item &&
                itemNum == other.itemNum;
    }
}

[SerializeField]
public class DecomposeItemClass : System.IEquatable<DecomposeItemClass>
{
    public string item;
    public int itemNum;

    public bool Equals(DecomposeItemClass other)
    {
        if (other == null)
            return false;

        DecomposeItemClass objAsPart = other as DecomposeItemClass;
        if (objAsPart == null)
            return false;
        else
            return item == other.item &&
                itemNum == other.itemNum;
    }
}

public class EquipmentData : ItemData, System.IComparable<EquipmentData>, System.IEquatable<EquipmentData>
{
    [Title("���� ����")]
    [LabelText("�������")]
    public EquipmentType equipmentType;

    [Title("�ɷ�ġ(�ּ�~�ִ�)")]
    [LabelText("ġ��Ÿ Ȯ��")]
    public Vector2Int criticalPer;

    [LabelText("ġ��Ÿ ������")]
    public Vector2Int criticalDamage;

    [LabelText("�ִ� ������")]
    public Vector2Int attackMax;

    [LabelText("�ּ� ������")]
    public Vector2Int attackMin;

    [Title("�ʿ����")]
    [LabelText("�ʿ����/����")]
    public List<RequireItemClass> requireItem = new List<RequireItemClass>();

    [Title("�������")]
    [LabelText("���/����")]
    public List<DecomposeItemClass> decomposeItem = new List<DecomposeItemClass>();

    public bool CanMake()
    {
        return CanMake(requireItem);
    }

    public ItemClass MakeItem(bool noResource = false)
    {
        if (noResource == false)
        {
            if (CanMake() == false)
                return null;
            foreach (RequireItemClass require in requireItem)
            {
                string item = require.item;
                int num = require.itemNum;
                if (Inventory.RemoveItem(item, num) == false)
                    return null;
            }
        }

        ItemClass itemClass = new ItemClass();
        itemClass.itemDataName = itemDataName;
        itemClass.itemType = ItemType.equitment;
        itemClass.itemNum = 1;
        itemClass.criticalPer = Random.Range(criticalPer.x, criticalPer.y + 1);
        itemClass.criticalDamage = Random.Range(criticalDamage.x, criticalDamage.y + 1);
        itemClass.attackMax = Random.Range(attackMax.x, attackMax.y + 1);
        itemClass.attackMin = Random.Range(attackMin.x, attackMin.y + 1);
        itemClass.attackMin = Mathf.Min(itemClass.attackMin, itemClass.attackMin);

        Inventory.AddItem(itemClass);

        return itemClass;
    }

    public bool Equals(EquipmentData other)
    {
        if (other == null)
            return false;
        EquipmentData objAsPart = other as EquipmentData;
        if (objAsPart == null)
            return false;
        else
        {
            if (itemDataName == other.itemDataName &&
                equipmentType == other.equipmentType &&
                criticalPer == other.criticalPer &&
                criticalDamage == other.criticalDamage &&
                attackMax == other.attackMax &&
                attackMin == other.attackMin)
            {
                if (requireItem.Count != other.requireItem.Count)
                {
                    for (int i = 0; i < requireItem.Count; i++)
                    {
                        if (requireItem[i].Equals(other.requireItem[i]) == false)
                        {
                            return false;
                        }
                    }
                }
                if (decomposeItem.Count != other.decomposeItem.Count)
                {
                    for (int i = 0; i < decomposeItem.Count; i++)
                    {
                        if (decomposeItem[i].Equals(other.decomposeItem[i]) == false)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }

    public int CompareTo(EquipmentData other)
    {
        if (other == null)
            return 1;
        else
        {
            int a = ItemManager.GetItemOrder(itemDataName);
            int b = ItemManager.GetItemOrder(other.itemDataName);
            return a.CompareTo(b);
        }
    }
}
