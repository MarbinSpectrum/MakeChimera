using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class DecomposeUI : UI_base
{
    private static DecomposeUI m_instance;
    public static DecomposeUI Instance
    {
        get
        {
            if (m_instance == null)
            {
                //NULL값인 경우 데이터를 매니저를 가져온다.
                GameObject managerObj = Instantiate(Resources.Load("UI/DecomposeUI") as GameObject);
                m_instance = managerObj.GetComponent<DecomposeUI>();

                m_instance.name = "DecomposeUI";

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
        decomposeItemSortUI.SortBtn(nowitemSort.ToString());
    }

    private string nowitemType = "head";
    private SortBy nowitemSort = SortBy.AttackMax;
    private SortType nowSortType = SortType.ASC;
    [SerializeField] private SortBy startitemSort = SortBy.AttackMax;
    [SerializeField] private SortType startSortType = SortType.ASC;
    [SerializeField] private string startitemType = "head";

    [Title("------------------------------------------")]
    [SerializeField] private EquipmentShowItem equipmentShowItem;
    [SerializeField] private GameObject decomposeCheckUI;
    [SerializeField] private GameObject decomposeBtn;
    [SerializeField] private DecomposeResultUI decomposeResultUI;
    [SerializeField] private DecomposeItemSortUI decomposeItemSortUI;
    [SerializeField] private SortSelectBtn sortSelectBtn;
    [SerializeField] private DecomposeSlot slotObj;
    [SerializeField] private Dictionary<EquipmentType, Image> selectList = new Dictionary<EquipmentType, Image>();
    [SerializeField] private GameObject body;
    [SerializeField] private Transform content;

    private List<DecomposeSlot> slotList = new List<DecomposeSlot>();
    private List<ItemClass> prev_equipmentList = new List<ItemClass>();
    private ItemData nowItem;

    //선택한 아이템 리스트
    private List<ItemClass> selectItemList = new List<ItemClass>();

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
    /// : 윈도우를 갱신
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void RefreshMenu()
    {
        SelectMenu();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 메뉴를 갱신(선택한 아이템 타입을 기준으로)
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private static void SelectMenu()
    {
        Instance.equipmentShowItem.OnOffWindow(false);
        SelectMenu(Instance.nowitemType);
    }

    public static void SelectMenu(string itemType)
    {
        Instance.selectItemList.Clear();
        Instance.decomposeBtn.SetActive(false);

        Instance.nowitemType = itemType;

        EquipmentType equipmentType = (EquipmentType)Enum.Parse(typeof(EquipmentType), itemType);
        foreach (KeyValuePair<EquipmentType, Image> pair in Instance.selectList)
        {
            //현재 선택중인 메뉴만 이펙트를 킨다. 
            if (pair.Key == equipmentType)
                pair.Value.enabled = true;
            else
                pair.Value.enabled = false;
        }

        DataManager.LoadPlayData();
        PlayData playData = DataManager.Instance.playData;
        ItemManager itemManager = ItemManager.Instance;

        List<ItemClass> hasItems = new List<ItemClass>();
        List<ItemClass> itemDatas = playData.equipments;
        foreach (ItemClass temp in itemDatas)
        {
            EquipmentData equipmentData = (EquipmentData)itemManager.equipment_Items[temp.itemDataName];
            if (equipmentData.equipmentType == equipmentType || equipmentType == EquipmentType.anything)
                hasItems.Add(temp);
        }


        if (hasItems.Count == 0)
            Instance.equipmentShowItem.ShowEmpty();
        else
        {
            SortItemClass(ref hasItems);
            Instance.equipmentShowItem.ShowItem(hasItems[0]);
        }
        Instance.MakeItemSlot(hasItems);
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
            DecomposeSlot obj = Instantiate(slotObj);
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
                slotList[i].SetData(itemData);
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
    /// : 아이템 클릭시 정보 표시
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void ShowItem(ItemClass itemClass)
    {
        Instance.equipmentShowItem.ShowItem(itemClass);
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
    /// : 선택 리스트에 아이템 등록
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void AddSelectList(ItemClass itemClass)
    {
        Instance.selectItemList.Add(new ItemClass(itemClass));
        if (Instance.selectItemList.Count > 0)
            Instance.decomposeBtn.SetActive(true);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 선택 리스트에 아이템 삭제
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void RemoveSelectList(ItemClass itemClass)
    {
        Instance.selectItemList.Remove(itemClass);
        if (Instance.selectItemList.Count == 0)
            Instance.decomposeBtn.SetActive(false);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 분해 여부 UI 활성화/비활성화 
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffDecomposeCheckUI(bool state)
    {
        if ((state == true && Instance.selectItemList.Count > 0) || state == false)
            Instance.decomposeCheckUI.SetActive(state);
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
    /// : 아이템 분해
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void DecomposeItem()
    {
        ItemManager itemManager = ItemManager.Instance;
        PlayData playData = DataManager.Instance.playData;

        List<ItemClass> resultList = new List<ItemClass>();
        List<ItemClass> list = playData.equipments;
        foreach (ItemClass itemClass in Instance.selectItemList)
        {
            //선택한 아이템에 해당하는 분해 아이템을 구해준다.
            EquipmentData equipmentData = (EquipmentData)itemManager.equipment_Items[itemClass.itemDataName];
            foreach (DecomposeItemClass decomposeItemClass in equipmentData.decomposeItem)
            {
                string itemDataName = decomposeItemClass.item;
                int itemNum = decomposeItemClass.itemNum;

                //분해 아이템을 인벤토리에 추가
                Inventory.AddItem(itemDataName, itemNum);
                AddDecomposeResult(new ItemClass(itemDataName, ItemType.material, itemNum));
            }

            //플레이어 장비에서 해당 아이템을 제거한다.
            list.Remove(itemClass);
        }
        DataManager.SaveData();
        RefreshMenu();

        //분해 결과를 추가하는 로컬함수
        void AddDecomposeResult(ItemClass itemClass)
        {
            for (int i = 0; i < resultList.Count; i++)
            {
                if (resultList[i].itemDataName == itemClass.itemDataName)
                {
                    resultList[i].itemNum += itemClass.itemNum;
                    return;
                }
            }
            resultList.Add(itemClass);
        }

        //분해결과를 출력
        Instance.decomposeResultUI.SetData(resultList);
        Instance.decomposeResultUI.OnOffWindow(true);
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
