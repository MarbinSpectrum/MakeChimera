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
                //NULL값인 경우 데이터를 매니저를 가져온다.
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
    /// : 윈도우를 갱신한다.
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
        //최신 데이터에 해당하는 상태에 맞게 데이터를 읽어온다.
        DataManager.LoadPlayData();

        ///현재 활성화 되어있는 슬롯 지우기
        if(Instance.nowUpgradeClass != upgrade_Class)
        {
            Instance.nowUpgradeClass = upgrade_Class;
            Instance.slotList.ForEach(x => { x.gameObject.SetActive(false); });
            foreach (KeyValuePair<Upgrade, SpecialSkillSlot> pair in Instance.specialSkillSlotList)
                pair.Value.gameObject.SetActive(false);
        }

        foreach (KeyValuePair<Upgrade_Class, Image> pair in Instance.selectList)
        {
            //현재 선택중인 메뉴만 이펙트를 킨다. 
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
        //강화 UI를 생성한다.
        int idx = 0;
        foreach (Upgrade up in Enum.GetValues(typeof(Upgrade)))
        {
            if (UpgradeManager.IsSpecialSkill(up) != Instance.nowUpgradeClass)
                continue;
            if (UpgradeManager.HasUpgradeData(up))
            {
                //업그레이드 데이터가 존재하면
                if (upgradeSlots.Count <= idx)
                {
                    //남는 슬롯이 없으면 추가해준다.
                    UpgradeSlot obj = Instantiate(Instance.slotObj);
                    obj.transform.parent = Instance.content;
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    upgradeSlots.Add(obj);
                }

                //슬롯에 업그레이드 정보를 넣어준다.
                upgradeSlots[idx].gameObject.SetActive(true);
                upgradeSlots[idx].SetUpgrade(up);
                idx++;
            }
        }
        upgradeSlots[0].CheckRequireItem();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 윈도우를 ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffWindow(bool state)
    {
        Instance.body.SetActive(state);
        if (state == true)
        {
            Instance.nowUpgradeClass = Instance.startUpgradeClass;

            //UI를 키는 상황이면 윈도우를 갱신해준다.
            RefreshWindow();
        }
        else
        {
            Instance.slotList.ForEach(x => { x.gameObject.SetActive(false); });
            foreach (KeyValuePair<Upgrade, SpecialSkillSlot> pair in Instance.specialSkillSlotList)
                pair.Value.gameObject.SetActive(false);

            //UI를 끄는 상황이면 업그레이드 재료 표시를 끈다.
            Instance.upgradeRequireList.OffRequireList();
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 업그레이드, 레벨에 해당하는 재료 정보를 표시한다.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void ShowRequireSlot(Upgrade upgrade, uint level)
    {
        Instance.upgradeRequireList.ShowRequireList(upgrade, level);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 재료 표시를 끈다.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OffRequireList()
    {
        Instance.upgradeRequireList.OffRequireList();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 재료 부족 메시지 활성/비활성
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffNoResource(bool state)
    {
        Instance.noResource.SetActive(state);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 강화슬롯 선택 이펙트 모두 비활성화
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void UnSelectSlots()
    {
        for (int i = 0; i < Instance.slotList.Count; i++)
            Instance.slotList[i].SelectSlot(false);
        foreach (KeyValuePair<Upgrade, SpecialSkillSlot> pair in Instance.specialSkillSlotList)
            pair.Value.SelectSlot(false);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 강화 버튼 누를때 소리
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
