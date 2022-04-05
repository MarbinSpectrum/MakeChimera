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
                //NULL���� ��� �����͸� �Ŵ����� �����´�.
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

    [LabelText("ġ��Ÿ Ȯ��(%)")]
    [Range(0,100)]
    public float criticalPer;

    [LabelText("ġ��Ÿ ������(%)")]
    public float criticalDamage;

    [LabelText("�ִ� ������")]
    public int attackMax;

    [LabelText("�ּ� ������")]
    public int attackMin;

    [LabelText("�ִ� ġ��Ÿ Ȯ��(%)")]
    [Range(0, 100)]
    public float maxCriticalPer = 80;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �÷��̾� ������
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
    /// : �÷��̾� ġ��Ÿ Ȯ��
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
    /// : �÷��̾� ġ��Ÿ ������
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
    /// : �÷��̾� ���� ������(������,ũ��Ƽ�� ����)
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
