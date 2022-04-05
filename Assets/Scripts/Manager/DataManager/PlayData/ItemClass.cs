using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    material,
    equitment
}

[SerializeField]
public class ItemClass : IComparable<ItemClass>, IEquatable<ItemClass>
{
    public int CompareTo(ItemClass other)
    {
        if (other == null)
            return 1;

        else
            return MakeString(this).CompareTo(MakeString(other));
    }
    public bool Equals(ItemClass other)
    {
        if (other == null)
            return false;
        ItemClass objAsPart = other as ItemClass;
        if (objAsPart == null)
            return false;
        else
            return MakeString(this) == MakeString(other);
    }

    public ItemClass()
    {
    }
    public ItemClass(ItemClass itemClass)
    {
        this.itemDataName = itemClass.itemDataName;
        this.itemType = itemClass.itemType;
        this.itemNum = itemClass.itemNum;
        this.criticalPer = itemClass.criticalPer;
        this.criticalDamage = itemClass.criticalDamage;
        this.attackMax = itemClass.attackMax;
        this.attackMin = itemClass.attackMin;
    }
    public ItemClass(string itemName, ItemType itemType, int itemNum)
    {
        this.itemDataName = itemName;
        this.itemType = itemType;
        this.itemNum = itemNum;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 데이터를 string으로 변경
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static string MakeString(ItemClass itemClass)
    {
        string[] stringArr = new string[]
        {
            itemClass.itemDataName,
            itemClass.itemType.ToString(),
            itemClass.itemNum.ToString(),
            itemClass.criticalPer.ToString(),
            itemClass.criticalDamage.ToString(),
            itemClass.attackMax.ToString(),
            itemClass.attackMin.ToString()
        };

        string temp = "";
        foreach(string s in stringArr)
            temp += s + ",";

        return temp;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : string을 데이터로 변경
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static ItemClass MakeItemClass(string itemClass)
    {
        ItemClass temp = new ItemClass();
        List<string> list = MyUtility.Split(itemClass, ',');

        temp.itemDataName = list[0];
        temp.itemType = (ItemType)Enum.Parse(typeof(ItemType), list[1]);
        temp.itemNum = int.Parse(list[2]);

        temp.criticalPer = float.Parse(list[3]);
        temp.criticalDamage = float.Parse(list[4]);
        temp.attackMax = int.Parse(list[5]);
        temp.attackMin = int.Parse(list[6]);

        return temp;
    }

    public string itemDataName;
    public ItemType itemType;
    public int itemNum;

    public float criticalPer;
    public float criticalDamage;
    public int attackMax;
    public int attackMin;
}
