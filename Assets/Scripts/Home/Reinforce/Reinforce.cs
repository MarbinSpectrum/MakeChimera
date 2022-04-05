using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Reinforce : UI_base
{
    private static Reinforce m_instance;
    public static Reinforce Instance
    {
        get
        {
            if (m_instance == null)
            {
                //NULL���� ��� �����͸� �Ŵ����� �����´�.
                GameObject managerObj = Instantiate(Resources.Load("UI/Reinforce") as GameObject);
                m_instance = managerObj.GetComponent<Reinforce>();

                m_instance.name = "Reinforce";

                DontDestroyOnLoad(m_instance.gameObject);

                m_instance.loadComplete = true;
            }
            return m_instance;
        }
    }

    public override void CallUI()
    {
        Debug.Log($"{Instance.gameObject.transform.name} Load");
    }

    [SerializeField] private Upgrade_Class startUpgradeClass = Upgrade_Class.BaseStat;
    private Upgrade_Class nowUpgradeClass;
    [SerializeField] private Dictionary<Upgrade_Class, Image> selectList
    = new Dictionary<Upgrade_Class, Image>();

    [SerializeField] private GameObject body;
    [SerializeField] private UpgradeSlot slotObj;
    private List<UpgradeSlot> slotList = new List<UpgradeSlot>();

    [SerializeField] private Dictionary<Upgrade, SpecialSkillSlot> specialSkillSlotList = 
        new Dictionary<Upgrade, SpecialSkillSlot>(); 

    [SerializeField] private Transform content;
    [SerializeField] private UpgradeRequireList upgradeRequireList;
    [SerializeField] private GameObject noResource;
    [SerializeField] private EventSE successSE;
    [SerializeField] private EventSE failSE;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �����츦 �����Ѵ�.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void RefreshWindow()
    {
        RefreshWindow(Instance.nowUpgradeClass);
    }
    public static void RefreshWindow(string upgradeClass)
    {
        Upgrade_Class upgrade_Class = (Upgrade_Class)Enum.Parse(typeof(Upgrade_Class), upgradeClass);
        RefreshWindow(upgrade_Class);
    }

    public static void RefreshWindow(Upgrade_Class upgrade_Class)
    {
        //�ֽ� �����Ϳ� �ش��ϴ� ���¿� �°� �����͸� �о�´�.
        DataManager.LoadPlayData();

        ///���� Ȱ��ȭ �Ǿ��ִ� ���� �����
        if(Instance.nowUpgradeClass != upgrade_Class)
        {
            Instance.nowUpgradeClass = upgrade_Class;
            Instance.slotList.ForEach(x => { x.gameObject.SetActive(false); });
            foreach (KeyValuePair<Upgrade, SpecialSkillSlot> pair in Instance.specialSkillSlotList)
                pair.Value.gameObject.SetActive(false);
        }

        foreach (KeyValuePair<Upgrade_Class, Image> pair in Instance.selectList)
        {
            //���� �������� �޴��� ����Ʈ�� Ų��. 
            if (pair.Key == upgrade_Class)
                pair.Value.enabled = true;
            else
                pair.Value.enabled = false;
        }

        if (upgrade_Class == Upgrade_Class.BaseStat)
            MakeSlotList(ref Instance.slotList);
        else if (upgrade_Class == Upgrade_Class.SpecialSkill)
        {
            bool flag = false;
            foreach (KeyValuePair<Upgrade, SpecialSkillSlot> pair in Instance.specialSkillSlotList)
            {
                if(!flag)
                {
                    flag = true;
                    pair.Value.CheckRequireItem();
                }
                pair.Value.gameObject.SetActive(true);
                pair.Value.SetUpgrade();
            }
        }
    }

    private static void MakeSlotList(ref List<UpgradeSlot> upgradeSlots)
    {
        //��ȭ UI�� �����Ѵ�.
        int idx = 0;
        foreach (Upgrade up in Enum.GetValues(typeof(Upgrade)))
        {
            if (UpgradeManager.IsSpecialSkill(up) != Instance.nowUpgradeClass)
                continue;
            if (UpgradeManager.HasUpgradeData(up))
            {
                //���׷��̵� �����Ͱ� �����ϸ�
                if (upgradeSlots.Count <= idx)
                {
                    //���� ������ ������ �߰����ش�.
                    UpgradeSlot obj = Instantiate(Instance.slotObj);
                    obj.transform.parent = Instance.content;
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    upgradeSlots.Add(obj);
                }

                //���Կ� ���׷��̵� ������ �־��ش�.
                upgradeSlots[idx].gameObject.SetActive(true);
                upgradeSlots[idx].SetUpgrade(up);
                idx++;
            }
        }
        upgradeSlots[0].CheckRequireItem();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �����츦 ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffWindow(bool state)
    {
        Instance.body.SetActive(state);
        if (state == true)
        {
            Instance.nowUpgradeClass = Instance.startUpgradeClass;

            //UI�� Ű�� ��Ȳ�̸� �����츦 �������ش�.
            RefreshWindow();
        }
        else
        {
            Instance.slotList.ForEach(x => { x.gameObject.SetActive(false); });
            foreach (KeyValuePair<Upgrade, SpecialSkillSlot> pair in Instance.specialSkillSlotList)
                pair.Value.gameObject.SetActive(false);

            //UI�� ���� ��Ȳ�̸� ���׷��̵� ��� ǥ�ø� ����.
            Instance.upgradeRequireList.OffRequireList();
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���׷��̵�, ������ �ش��ϴ� ��� ������ ǥ���Ѵ�.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void ShowRequireSlot(Upgrade upgrade, uint level)
    {
        Instance.upgradeRequireList.ShowRequireList(upgrade, level);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ��� ǥ�ø� ����.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OffRequireList()
    {
        Instance.upgradeRequireList.OffRequireList();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ��� ���� �޽��� Ȱ��/��Ȱ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffNoResource(bool state)
    {
        Instance.noResource.SetActive(state);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ��ȭ���� ���� ����Ʈ ��� ��Ȱ��ȭ
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void UnSelectSlots()
    {
        for (int i = 0; i < Instance.slotList.Count; i++)
            Instance.slotList[i].SelectSlot(false);
        foreach (KeyValuePair<Upgrade, SpecialSkillSlot> pair in Instance.specialSkillSlotList)
            pair.Value.SelectSlot(false);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ��ȭ ��ư ������ �Ҹ�
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void FailSE()
    {
        Instance.failSE.RunEvent();
    }

    public static void SuccessSE()
    {
        Instance.successSE.RunEvent();
    }
}
