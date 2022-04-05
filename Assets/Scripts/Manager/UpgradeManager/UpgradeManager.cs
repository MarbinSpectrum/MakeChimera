using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum Upgrade
{
    MinDamage,
    MaxDamage,
    CriticalPer,
    CriticalDamage,
    WolfAgility,
    DragonBlood,
}
public enum Upgrade_Class
{
    BaseStat,
    SpecialSkill
}

public class UpgradeManager : Manager
{
    private static UpgradeManager m_instance;
    public static UpgradeManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                //NULL���� ��� �����͸� �Ŵ����� �����´�.
                GameObject managerObj = Instantiate(Resources.Load("Manager/UpgradeManager") as GameObject);
                m_instance = managerObj.GetComponent<UpgradeManager>();

                m_instance.name = "UpgradeManager";

                m_instance.Init();

                DontDestroyOnLoad(m_instance.gameObject);

                m_instance.loadComplete = true;
            }
            return m_instance;
        }
    }

    public override void CallManager()
    {
        Debug.Log($"{Instance.gameObject.transform.name} Load");
    }

    private Dictionary<Upgrade, Dictionary<uint, UpgradeData>> upgradeDatas = 
        new Dictionary<Upgrade, Dictionary<uint, UpgradeData>>();

    private void Init()
    {
        foreach (Upgrade up in Enum.GetValues(typeof(Upgrade)))
            upgradeDatas[up] = new Dictionary<uint, UpgradeData>();
        FindUpgradeData(transform);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���׷��̵� ������ �ε�
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void FindUpgradeData(Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            UpgradeData data = transform.GetChild(i).GetComponent<UpgradeData>();

            if(data != null)
            {
                Upgrade upgradeType = data.upgradeType;
                uint level = data.level;
                upgradeDatas[upgradeType].Add(level, data);
            }

            FindUpgradeData(child);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �ش� ���׷��̵尡 Ư�� �ɷ����� Ȯ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static Upgrade_Class IsSpecialSkill(Upgrade upgrade)
    {
        switch(upgrade)
        {
            case Upgrade.WolfAgility:
            case Upgrade.DragonBlood:
                return Upgrade_Class.SpecialSkill;
        }
        return Upgrade_Class.BaseStat;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �ش� ���׷��̵� ����� �ۼ�Ʈ���� �ƴ��� �˻�
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool IsPercentSkill(Upgrade upgrade)
    {
        switch (upgrade)
        {
            case Upgrade.WolfAgility:
            case Upgrade.DragonBlood:
            case Upgrade.CriticalDamage:
            case Upgrade.CriticalPer:
                return true;
        }
        return false;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���׷��̵� �����͸� �޾ƿ�
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static UpgradeData GetUpgradeData(Upgrade upgrade,uint level)
    {
        return Instance.upgradeDatas[upgrade][level];
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���׷��̵� ������ �ִ��� �˻�
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool HasUpgradeData(Upgrade upgrade)
    {
        return Instance.upgradeDatas[upgrade].Count > 0;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���� ���׷��̵� ������ �޾ƿ´�
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static uint GetUpgradeLevel(Upgrade upgrade)
    {
        PlayData playData = DataManager.Instance.playData;
        return playData.GetUpgradeLevel(upgrade);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���׷��̵� �Ŀ��� �޾ƿ´�
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static float GetUpgradePower(Upgrade upgrade)
    {
        uint level = GetUpgradeLevel(upgrade);
        return GetUpgradePower(upgrade, level);
    }
    public static float GetUpgradePower(Upgrade upgrade, uint level)
    {
        if (!Instance.upgradeDatas.ContainsKey(upgrade))
            return 0;
        if (Instance.upgradeDatas[upgrade].ContainsKey(level))
            return Instance.upgradeDatas[upgrade][level].GetPower();
        return 0;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���׷��̵� �̸��� �޾ƿ´�.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static string GetUpgradeName(Upgrade upgrade, uint level)
    {
        if(Instance.upgradeDatas.ContainsKey(upgrade) == false)
        {
            Debug.LogError("���׷��̵� ������ ����.");
            return "";
        }
        if (Instance.upgradeDatas[upgrade].ContainsKey(level))
        {
            return Instance.upgradeDatas[upgrade][level].upgradeName;
        }
        return "";
    }
    public static string GetUpgradeName(Upgrade upgrade)
    {
        uint level = GetUpgradeLevel(upgrade);
        return GetUpgradeName(upgrade, level);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���� �ִ� �������� �˻�
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool IsMaxLevel(Upgrade upgrade, uint level)
    {
        if (Instance.upgradeDatas[upgrade].ContainsKey(level + 1))
            return false;
        return true;
    }
    public static bool NowMaxLevel(Upgrade upgrade)
    {
        uint level = GetUpgradeLevel(upgrade);
        return IsMaxLevel(upgrade, level);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ��ȭ �������� �˻�
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool CanUpgrade(Upgrade upgrade, uint level)
    {
        if (HasUpgradeData(upgrade) == false)
            return false;

        //��� ������ �޾ƿ´�.
        List<RequireItemClass> requireItems = GetUpgradeRequireList(upgrade,level);
        foreach(RequireItemClass item in requireItems)
        {
            //���ᰡ �ִ��� Ȯ���Ѵ�.
            if (Inventory.HasItem(item.item, item.itemNum) == false)
            {
                return false;
            }
        }
        return true;
    }
    public static bool CanUpgrade(Upgrade upgrade)
    {
        uint level = GetUpgradeLevel(upgrade);
        return CanUpgrade(upgrade, level);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���׷��̵带 �Ѵ�.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool AddUpgrade(Upgrade upgrade)
    {
        uint nextLv = GetUpgradeLevel(upgrade) + 1;

        if (CanUpgrade(upgrade, nextLv) == false)
        {
            Debug.LogError("���ۺҰ�");
            return false;
        }
        if (IsMaxLevel(upgrade, nextLv - 1))
        {
            Debug.LogError("�ִ뷹��");
            return false;
        }

        //��� ����Ʈ�� �����´�.
        List<RequireItemClass> requireItems = GetUpgradeRequireList(upgrade, nextLv);
        foreach (RequireItemClass item in requireItems)
        {
            //��ῡ �ش��ϴ� �������� �κ��丮���� ����
            Inventory.RemoveItem(item.item, item.itemNum);
        }

        PlayData playData = DataManager.Instance.playData;
        return playData.AddUpgradeLevel(upgrade);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���׷��̵� ��Ḧ �޾ƿ´�.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static List<RequireItemClass> GetUpgradeRequireList(Upgrade upgrade, uint level)
    {
        if(Instance.upgradeDatas.ContainsKey(upgrade) == false)
            return null;
        if (Instance.upgradeDatas[upgrade].ContainsKey(level) == false)
            return null;
        UpgradeData upgradeData = Instance.upgradeDatas[upgrade][level];
        return upgradeData.requireItem;
    }
    public static List<RequireItemClass> GetUpgradeRequireList(Upgrade upgrade)
    {
        uint level = GetUpgradeLevel(upgrade);
        return GetUpgradeRequireList(upgrade, level);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���׷��̵� �������� �޾ƿ´�.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static Sprite GetUpgradeIcon(Upgrade upgrade, uint level)
    {
        if (Instance.upgradeDatas.ContainsKey(upgrade) == false)
            return null;
        if (Instance.upgradeDatas[upgrade].ContainsKey(level) == false)
            return null;
        UpgradeData upgradeData = Instance.upgradeDatas[upgrade][level];
        return upgradeData.itemImg;
    }
    public static Sprite GetUpgradeIcon(Upgrade upgrade)
    {
        uint level = GetUpgradeLevel(upgrade);
        return GetUpgradeIcon(upgrade, level);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���׷��̵� �������� �޾ƿ´�.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static string GetSkillExplain(Upgrade upgrade, uint level)
    {
        if (Instance.upgradeDatas.ContainsKey(upgrade) == false)
            return null;
        if (Instance.upgradeDatas[upgrade].ContainsKey(level) == false)
            return null;
        UpgradeData upgradeData = Instance.upgradeDatas[upgrade][level];
        if (upgrade == Upgrade.WolfAgility)
        {
            WolfAgilityData wolfAgilityData = (WolfAgilityData)upgradeData;
            return wolfAgilityData.GetExplain();
        }
        else if (upgrade == Upgrade.DragonBlood)
        {
            DragonBloodData dragonBloodData = (DragonBloodData)upgradeData;
            return dragonBloodData.GetExplain();
        }

        return "";
    }
    public static string GetSkillExplain(Upgrade upgrade)
    {
        uint level = GetUpgradeLevel(upgrade);
        return GetSkillExplain(upgrade, level);
    }
}
