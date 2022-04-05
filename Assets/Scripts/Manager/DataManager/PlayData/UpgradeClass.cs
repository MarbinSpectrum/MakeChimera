using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeClass : IComparable<UpgradeClass>, IEquatable<UpgradeClass>
{
    public int CompareTo(UpgradeClass other)
    {
        if (other == null)
            return 1;

        else
            return MakeString(this).CompareTo(MakeString(other));
    }

    public bool Equals(UpgradeClass other)
    {
        if (other == null)
            return false;
        UpgradeClass objAsPart = other as UpgradeClass;
        if (objAsPart == null)
            return false;
        else
            return MakeString(this) == MakeString(other);
    }

    public UpgradeClass()
    {
    }
    public UpgradeClass(Upgrade upgrade, int level)
    {
        this.upgrade = upgrade;
        this.level = level;
    }

    public static string MakeString(UpgradeClass itemClass)
    {
        string temp = "";
        temp += itemClass.upgrade;
        temp += ",";
        temp += itemClass.level;
        temp += ",";
        return temp;
    }

    public static UpgradeClass MakeItemClass(string itemClass)
    {
        UpgradeClass temp = new UpgradeClass();
        List<string> list = MyUtility.Split(itemClass, ',');

        temp.upgrade = (Upgrade)Enum.Parse(typeof(Upgrade), list[0]);
        temp.level = int.Parse(list[1]);

        return temp;
    }

    public Upgrade upgrade;
    public int level;
}
