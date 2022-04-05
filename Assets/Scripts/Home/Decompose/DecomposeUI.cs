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
                //NULL���� ��� �����͸� �Ŵ����� �����´�.
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

    //������ ������ ����Ʈ
    private List<ItemClass> selectItemList = new List<ItemClass>();

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �����츦 ON OFF
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
    /// : �����츦 ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void RefreshMenu()
    {
        SelectMenu();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �޴��� ����(������ ������ Ÿ���� ��������)
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
            //���� �������� �޴��� ����Ʈ�� Ų��. 
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
    /// : ������ ����Ʈ�� ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private static void SortItemClass(ref List<ItemClass> list)
    {
        //�ִ� ���ݷ� �������� ����
        if (Instance.nowitemSort == SortBy.AttackMax && Instance.nowSortType == SortType.ASC)
            list.Sort((a, b) => a.attackMax.CompareTo(b.attackMax));
        else if (Instance.nowitemSort == SortBy.AttackMax && Instance.nowSortType == SortType.DES)
        {
            list.Sort((a, b) => a.attackMax.CompareTo(b.attackMax));
            list.Reverse();
        }

        //�ּ� ���ݷ� �������� ����
        else if (Instance.nowitemSort == SortBy.AttackMin && Instance.nowSortType == SortType.ASC)
            list.Sort((a, b) => a.attackMin.CompareTo(b.attackMin));
        else if (Instance.nowitemSort == SortBy.AttackMin && Instance.nowSortType == SortType.DES)
        {
            list.Sort((a, b) => a.attackMin.CompareTo(b.attackMin));
            list.Reverse();
        }

        //ġ��Ÿ ������ �������� ����
        else if (Instance.nowitemSort == SortBy.CriticalDamage && Instance.nowSortType == SortType.ASC)
            list.Sort((a, b) => a.criticalDamage.CompareTo(b.criticalDamage));
        else if (Instance.nowitemSort == SortBy.CriticalDamage && Instance.nowSortType == SortType.DES)
        {
            list.Sort((a, b) => a.criticalDamage.CompareTo(b.criticalDamage));
            list.Reverse();
        }

        //ġ��Ÿ Ȯ�� �������� ����
        else if (Instance.nowitemSort == SortBy.CriticalPer && Instance.nowSortType == SortType.ASC)
            list.Sort((a, b) => a.criticalPer.CompareTo(b.criticalPer));
        else if (Instance.nowitemSort == SortBy.CriticalPer && Instance.nowSortType == SortType.DES)
        {
            list.Sort((a, b) => a.criticalPer.CompareTo(b.criticalPer));
            list.Reverse();
        }

        //�̸� �������� ����
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
    /// : ������ ������ ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void MakeItemSlot(List<ItemClass> equipmentList)
    {
        //�簻���� �ؾߵǴ��� �˻�
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

        //�ʿ��� �߰� ���� ������ ����
        int createNum = equipmentList.Count - slotList.Count;

        for (int i = 0; i < createNum; i++)
        {
            //�ʿ� ������ŭ ������ �߰�
            DecomposeSlot obj = Instantiate(slotObj);
            obj.transform.parent = content;
            obj.transform.localScale = new Vector3(1, 1, 1);
            slotList.Add(obj);
        }

        for (int i = 0; i < slotList.Count; i++)
        {
            //�ִϸ��̼��� ����� ��Ű�����ؼ� ��Ȱ��ȭ
            slotList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < slotList.Count; i++)
        {
            //���Կ� ������ ������ ǥ��
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

        //������ ������ ��Ÿ���� �ִϸ��̼��� �ڷ�ƾ���� ǥ��
        StartCoroutine(RunSlotAni(equipmentList.Count, 0.02f));

        //���� �ִϸ��̼� ����
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
    /// : ������ Ŭ���� ���� ǥ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void ShowItem(ItemClass itemClass)
    {
        Instance.equipmentShowItem.ShowItem(itemClass);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �������� ���� �������� ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void SortItemList(SortBy sortBy)
    {
        Instance.nowitemSort = sortBy;
        RefreshMenu();
        Instance.sortSelectBtn.SetText(sortBy);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���� ����Ʈ�� ������ ���
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void AddSelectList(ItemClass itemClass)
    {
        Instance.selectItemList.Add(new ItemClass(itemClass));
        if (Instance.selectItemList.Count > 0)
            Instance.decomposeBtn.SetActive(true);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���� ����Ʈ�� ������ ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void RemoveSelectList(ItemClass itemClass)
    {
        Instance.selectItemList.Remove(itemClass);
        if (Instance.selectItemList.Count == 0)
            Instance.decomposeBtn.SetActive(false);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���� ���� UI Ȱ��ȭ/��Ȱ��ȭ 
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffDecomposeCheckUI(bool state)
    {
        if ((state == true && Instance.selectItemList.Count > 0) || state == false)
            Instance.decomposeCheckUI.SetActive(state);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ��������, �������� ���ļ���
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
    /// : ������ ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void DecomposeItem()
    {
        ItemManager itemManager = ItemManager.Instance;
        PlayData playData = DataManager.Instance.playData;

        List<ItemClass> resultList = new List<ItemClass>();
        List<ItemClass> list = playData.equipments;
        foreach (ItemClass itemClass in Instance.selectItemList)
        {
            //������ �����ۿ� �ش��ϴ� ���� �������� �����ش�.
            EquipmentData equipmentData = (EquipmentData)itemManager.equipment_Items[itemClass.itemDataName];
            foreach (DecomposeItemClass decomposeItemClass in equipmentData.decomposeItem)
            {
                string itemDataName = decomposeItemClass.item;
                int itemNum = decomposeItemClass.itemNum;

                //���� �������� �κ��丮�� �߰�
                Inventory.AddItem(itemDataName, itemNum);
                AddDecomposeResult(new ItemClass(itemDataName, ItemType.material, itemNum));
            }

            //�÷��̾� ��񿡼� �ش� �������� �����Ѵ�.
            list.Remove(itemClass);
        }
        DataManager.SaveData();
        RefreshMenu();

        //���� ����� �߰��ϴ� �����Լ�
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

        //���ذ���� ���
        Instance.decomposeResultUI.SetData(resultList);
        Instance.decomposeResultUI.OnOffWindow(true);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : Ʃ�丮�� ������ ���� Box Ȱ��/��Ȱ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffTutorialSlotBox(bool state)
    {
        if (Instance.slotList.Count >= 1)
        {
            Instance.slotList[0].OnOffTutorialBox(state);
        }
    }
}
