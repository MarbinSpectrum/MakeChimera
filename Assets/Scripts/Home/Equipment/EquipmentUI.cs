using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class EquipmentUI : UI_base
{
    private static EquipmentUI m_instance;
    public static EquipmentUI Instance
    {
        get
        {
            if (m_instance == null)
            {
                //NULL값인 경우 데이터를 매니저를 가져온다.
                GameObject managerObj = Instantiate(Resources.Load("UI/EquipmentUI") as GameObject);
                m_instance = managerObj.GetComponent<EquipmentUI>();

                m_instance.name = "EquipmentUI";

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

    private void Init()
    {
        nowitemType = startitemType;
        nowitemSort = startitemSort;
        nowSortType = startSortType;
        prev_equipmentList.Clear();
        equipmentItemSortUI.SortBtn(nowitemSort.ToString());
    }

    private string nowitemType = "head";
    private SortBy nowitemSort = SortBy.AttackMax;
    private SortType nowSortType = SortType.ASC;
    [SerializeField] private SortBy startitemSort = SortBy.AttackMax;
    [SerializeField] private SortType startSortType = SortType.ASC;
    [SerializeField] private string startitemType = "head";

    [Title("------------------------------------------")]
    [SerializeField] private EquipmentShowItem equipmentShowItem;
    [SerializeField] private EquipmentItemSortUI equipmentItemSortUI;
    [SerializeField] private SortSelectBtn sortSelectBtn;
    [SerializeField] private EquipmentSlot slotObj;
    [SerializeField] private GameObject sortGroup;
    [SerializeField] private Dictionary<EquipmentType, Image> selectList
        = new Dictionary<EquipmentType, Image>();
    [SerializeField] private Dictionary<EquipmentType, EquipmentSlot> nowItemEquipmentSlot 
        = new Dictionary<EquipmentType, EquipmentSlot>();
    [SerializeField] private Dictionary<EquipmentType, GameObject> showEmptyItem
        = new Dictionary<EquipmentType, GameObject>();
    [SerializeField] private GameObject body;
    [SerializeField] private Transform content;
    [SerializeField] private TextMeshProUGUI playerDamage;
    [SerializeField] private TextMeshProUGUI playerCriticalPer;
    [SerializeField] private TextMeshProUGUI playerCriticalDamage;

    private List<EquipmentSlot> slotList = new List<EquipmentSlot>();
    private List<ItemClass> prev_equipmentList = new List<ItemClass>();
    private ItemData nowItem;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 윈도우를 ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffWindow(bool state)
    {
        Instance.body.SetActive(state);
        if (state == true)
        {
            Instance.Init();
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 장비창 갱신
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void RefreshMenu()
    {
        SelectMenu();
        Player.UpdateWear();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 메뉴를 갱신(선택한 아이템 타입을 기준으로)
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private static void SelectMenu()
    {
        SelectMenu(Instance.nowitemType);
    }
    public static void SelectMenu(string itemType)
    {
        Instance.nowitemType = itemType;

        EquipmentType equipmentType = (EquipmentType)Enum.Parse(typeof(EquipmentType), itemType);
        foreach (KeyValuePair<EquipmentType, Image> pair in Instance.selectList)
        {
            //현재 선택중인 메뉴만 이펙트를 킨다. 
            if(pair.Key == equipmentType)
                pair.Value.enabled = true;
            else
                pair.Value.enabled = false;
        }

        List<ItemClass> hasItems = null;
        if (equipmentType == EquipmentType.material)
        {
            DataManager.LoadPlayData();
            PlayData playData = DataManager.Instance.playData;
            ItemManager itemManager = ItemManager.Instance;

            hasItems = playData.materials;

            //아이템 슬롯 생성
            Instance.MakeItemSlot(hasItems);
            Instance.sortGroup.SetActive(false);
        }
        else
        {
            DataManager.LoadPlayData();
            PlayData playData = DataManager.Instance.playData;
            ItemManager itemManager = ItemManager.Instance;

            hasItems = new List<ItemClass>();
            List<ItemClass> itemDatas = playData.equipments;
            foreach (ItemClass temp in itemDatas)
            {
                EquipmentData equipmentData = (EquipmentData)itemManager.equipment_Items[temp.itemDataName];
                if (equipmentData.equipmentType == equipmentType)
                {
                    //장비 타입에 맞는 아이템을 분류(Ex 머리,몸통,팔 등)
                    hasItems.Add(temp);
                }
            }

            //아이템 정렬
            SortItemClass(ref hasItems);

            //아이템 슬롯 생성
            Instance.MakeItemSlot(hasItems);
            Instance.sortGroup.SetActive(true);
        }

        foreach (KeyValuePair<EquipmentType, GameObject> pair in Instance.showEmptyItem)
            pair.Value.SetActive(false);
        if (hasItems.Count == 0)
            Instance.showEmptyItem[equipmentType].SetActive(true);
        NowEquipMent();
        ShowStat();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 플레이어 스텟을 표시
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private static void ShowStat()
    {
        PlayData playData = DataManager.Instance.playData;

        Vector2Int damage = PlayerManager.TotalDamage();
        float criticalDamage = PlayerManager.TotalCriticalDamage();
        float criticalPer = PlayerManager.TotalCriticalPer();
        Instance.playerDamage.text = $"{damage.x} ~ {damage.y}";
        Instance.playerCriticalDamage.text = $"{criticalDamage}%";
        Instance.playerCriticalPer.text = $"{criticalPer}%";
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 아이템 리스트를 정렬
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private static void SortItemClass(ref List<ItemClass> list)
    {
        //최대 공격력 기준으로 정렬
        if (Instance.nowitemSort == SortBy.AttackMax && Instance.nowSortType == SortType.ASC)
            list.Sort((a, b) => a.attackMax.CompareTo(b.attackMax));
        else if (Instance.nowitemSort == SortBy.AttackMax && Instance.nowSortType == SortType.DES)
        {
            list.Sort((a, b) => a.attackMax.CompareTo(b.attackMax));
            list.Reverse();
        }

        //최소 공격력 기준으로 정렬
        else if (Instance.nowitemSort == SortBy.AttackMin && Instance.nowSortType == SortType.ASC)
            list.Sort((a, b) => a.attackMin.CompareTo(b.attackMin));
        else if (Instance.nowitemSort == SortBy.AttackMin && Instance.nowSortType == SortType.DES)
        {
            list.Sort((a, b) => a.attackMin.CompareTo(b.attackMin));
            list.Reverse();
        }

        //치명타 데미지 기준으로 정렬
        else if (Instance.nowitemSort == SortBy.CriticalDamage && Instance.nowSortType == SortType.ASC)
            list.Sort((a, b) => a.criticalDamage.CompareTo(b.criticalDamage));
        else if (Instance.nowitemSort == SortBy.CriticalDamage && Instance.nowSortType == SortType.DES)
        {
            list.Sort((a, b) => a.criticalDamage.CompareTo(b.criticalDamage));
            list.Reverse();
        }

        //치명타 확률 기준으로 정렬
        else if (Instance.nowitemSort == SortBy.CriticalPer && Instance.nowSortType == SortType.ASC)
            list.Sort((a, b) => a.criticalPer.CompareTo(b.criticalPer));
        else if (Instance.nowitemSort == SortBy.CriticalPer && Instance.nowSortType == SortType.DES)
        {
            list.Sort((a, b) => a.criticalPer.CompareTo(b.criticalPer));
            list.Reverse();
        }

        //이름 기준으로 정렬
        else if (Instance.nowitemSort == SortBy.Name && Instance.nowSortType == SortType.ASC)
            list.Sort((a, b) => a.itemDataName.CompareTo(b.itemDataName));
        else if (Instance.nowitemSort == SortBy.Name && Instance.nowSortType == SortType.DES)
        {
            list.Sort((a, b) => a.itemDataName.CompareTo(b.itemDataName));
            list.Reverse();
        }
        else
            list.Sort();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 아이템 슬롯을 생성
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void MakeItemSlot(List<ItemClass> equipmentList)
    {
        //재갱신을 해야되는지 검사
        bool refreshFlag = false;
        if (prev_equipmentList.Count == equipmentList.Count)
        {
            for (int i = 0; i < equipmentList.Count && !refreshFlag; i++)
                if (prev_equipmentList[i].Equals(equipmentList[i]) == false)
                    refreshFlag = true;
        }
        else
            refreshFlag = true;

        if (refreshFlag == false)
        {
            return;
        }

        //필요한 추가 슬롯 갯수를 상정
        int createNum = equipmentList.Count - slotList.Count;

        for (int i = 0; i < createNum; i++)
        {
            //필요 갯수만큼 슬롯을 추가
            EquipmentSlot obj = Instantiate(slotObj);
            obj.transform.parent = content;
            obj.transform.localScale = new Vector3(1, 1, 1);
            slotList.Add(obj);
        }

        for (int i = 0; i < slotList.Count; i++)
        {
            //애니메이션을 재시작 시키기위해서 비활성화
            slotList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < slotList.Count; i++)
        {
            //슬롯에 아이템 정보를 표시
            if (i >= equipmentList.Count)
                slotList[i].gameObject.SetActive(false);
            else
            {
                ItemClass itemData = equipmentList[i];
                slotList[i].SetData(itemData, false);
                slotList[i].gameObject.SetActive(true);
            }
        }

        Instance.prev_equipmentList.Clear();
        foreach (ItemClass equipment in equipmentList)
            Instance.prev_equipmentList.Add(equipment);

        //슬롯이 서서히 나타나는 애니메이션을 코루틴으로 표현
        StartCoroutine(RunSlotAni(equipmentList.Count, 0.02f));

        //슬롯 애니메이션 설정
        IEnumerator RunSlotAni(int cnt, float term)
        {
            for (int i = 0; i < cnt; i++)
            {
                slotList[i].ShowAni("Show");
                yield return new WaitForSeconds(term);
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 현재 장비중인 장비 표시
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private static void NowEquipMent()
    {
        DataManager.LoadPlayData();
        PlayData playData = DataManager.Instance.playData;

        foreach(KeyValuePair<EquipmentType, EquipmentSlot> pair in Instance.nowItemEquipmentSlot)
        {
            EquipmentType equipmentType = pair.Key;
            int idx = playData.GetNowEquipIdx(equipmentType);
            if(idx != -1)
            {
                ItemClass itemData = playData.nowEquipments[idx];
                pair.Value.SetData(itemData, true);
            }
            else
            {
                pair.Value.SetData(null, true);
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 아이템 클릭시 정보 표시
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void ShowItem(ItemClass itemClass, bool nowItem)
    {
        Instance.equipmentShowItem.ShowItem(itemClass, nowItem);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 아이템을 정렬 기준으로 정렬
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void SortItemList(SortBy sortBy)
    {
        Instance.nowitemSort = sortBy;
        RefreshMenu();
        Instance.sortSelectBtn.SetText(sortBy);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 오름차순, 내림차순 정렬설정
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void ChangeSortType()
    {
        SortType sortType = Instance.nowSortType;
        if (sortType == SortType.ASC)
            ChangeSortType(SortType.DES);
        else if (sortType == SortType.DES)
            ChangeSortType(SortType.ASC);
    }
    public static void ChangeSortType(SortType sortType)
    {
        Instance.nowSortType = sortType;
        RefreshMenu();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 튜토리얼 아이템 선택 Box 활성/비활성
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffTutorialSlotBox(bool state)
    {
        if (Instance.slotList.Count >= 1)
        {
            Instance.slotList[0].OnOffTutorialBox(state);
        }
    }
}
