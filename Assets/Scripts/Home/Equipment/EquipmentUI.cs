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
                //NULL���� ��� �����͸� �Ŵ����� �����´�.
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
    /// : ���â ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void RefreshMenu()
    {
        SelectMenu();
        Player.UpdateWear();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �޴��� ����(������ ������ Ÿ���� ��������)
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
            //���� �������� �޴��� ����Ʈ�� Ų��. 
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

            //������ ���� ����
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
                    //��� Ÿ�Կ� �´� �������� �з�(Ex �Ӹ�,����,�� ��)
                    hasItems.Add(temp);
                }
            }

            //������ ����
            SortItemClass(ref hasItems);

            //������ ���� ����
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
    /// : �÷��̾� ������ ǥ��
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
            EquipmentSlot obj = Instantiate(slotObj);
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
                slotList[i].SetData(itemData, false);
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
    /// : ���� ������� ��� ǥ��
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
    /// : ������ Ŭ���� ���� ǥ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void ShowItem(ItemClass itemClass, bool nowItem)
    {
        Instance.equipmentShowItem.ShowItem(itemClass, nowItem);
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
