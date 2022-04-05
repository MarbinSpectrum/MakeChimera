using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerManager : Manager
{
    private static PlayerManager m_instance;
    public static PlayerManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                //NULL값인 경우 데이터를 매니저를 가져온다.
                GameObject managerObj = Instantiate(Resources.Load("Manager/PlayerManager") as GameObject);
                m_instance = managerObj.GetComponent<PlayerManager>();

                m_instance.name = "PlayerManager";

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

    [LabelText("치명타 확률(%)")]
    [Range(0,100)]
    public float criticalPer;

    [LabelText("치명타 데미지(%)")]
    public float criticalDamage;

    [LabelText("최대 데미지")]
    public int attackMax;

    [LabelText("최소 데미지")]
    public int attackMin;

    [LabelText("최대 치명타 확률(%)")]
    [Range(0, 100)]
    public float maxCriticalPer = 80;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 플레이어 데미지
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static Vector2Int TotalDamage()
    {
        PlayData playData = DataManager.Instance.playData;
        int minDamage = Instance.attackMin +
            (int)UpgradeManager.GetUpgradePower(Upgrade.MinDamage);
        int maxDamage = Instance.attackMax + 
            (int)UpgradeManager.GetUpgradePower(Upgrade.MaxDamage);
        foreach (ItemClass item in playData.nowEquipments)
        {
            minDamage += item.attackMin;
            maxDamage += item.attackMax;
        }
        minDamage = Mathf.Min(maxDamage, minDamage);

        return new Vector2Int(minDamage, maxDamage);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 플레이어 치명타 확률
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static float TotalCriticalPer()
    {
        PlayData playData = DataManager.Instance.playData;
        float perCritical = Instance.criticalPer + 
            UpgradeManager.GetUpgradePower(Upgrade.CriticalPer);
        foreach (ItemClass item in playData.nowEquipments)
            perCritical += item.criticalPer;
        perCritical = Mathf.Min(perCritical, Instance.maxCriticalPer);

        return perCritical;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 플레이어 치명타 데미지
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static float TotalCriticalDamage()
    {
        PlayData playData = DataManager.Instance.playData;
        float damageCritical = Instance.criticalDamage +
            UpgradeManager.GetUpgradePower(Upgrade.CriticalDamage);
        foreach (ItemClass item in playData.nowEquipments)
            damageCritical += item.criticalDamage;
        return damageCritical;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 플레이어 실제 데미지(데미지,크리티컬 여부)
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static KeyValuePair<int,bool> PlayerDamage()
    {
        Vector2Int totalDamage = TotalDamage();
        float totalCirticalPer = TotalCriticalPer();
        float totalCirticalDamage = TotalCriticalDamage();
        int damage = Random.Range(totalDamage.x, totalDamage.y + 1);

        float r = Random.Range(0, 100f);
        if (r < totalCirticalPer)
        {
            int total = (int)(damage * ((100 + totalCirticalDamage) / 100f));
            return new KeyValuePair<int, bool>(total, true);
        }
        else
            return new KeyValuePair<int, bool>(damage, false);
    }
}
