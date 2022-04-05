using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Craft : UI_base
{
    private static Craft m_instance;
    public static Craft Instance
    {
        get
        {
            if (m_instance == null)
            {
                //NULL���� ��� �����͸� �Ŵ����� �����´�.
                GameObject soundManagerObj = Instantiate(Resources.Load("UI/Craft") as GameObject);
                m_instance = soundManagerObj.GetComponent<Craft>();

                m_instance.name = "Craft";

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
        nowOption = startOption;
        craftDropDown.Init(nowOption);
        prev_equipmentList.Clear();
        OnOffNoResource(false);
        SelectMenu();
    }

    private string nowitemType = "head";
    [SerializeField] private string startitemType = "head";
    [SerializeField] private CraftDropDownMenu startOption = CraftDropDownMenu.CanCraft;
    [System.NonSerialized] public CraftDropDownMenu nowOption = CraftDropDownMenu.CanCraft;

    [Title("------------------------------------------")]
    [SerializeField] private CraftDropDown craftDropDown;
    [SerializeField] private CraftCompleteWindow craftCompleteWindow;
    [SerializeField] private CraftItemExplain craftItemExplain;
    [SerializeField] private CraftSlot slotObj;
    [SerializeField] private GameObject noResource;
    [SerializeField] private GameObject craftBtn;
    [SerializeField] private Dictionary<EquipmentType, Image> selectList = new Dictionary<EquipmentType, Image>();
    [SerializeField] private GameObject showEmpty;
    [SerializeField] private GameObject body;
    [SerializeField] private Transform content;
     
    private List<CraftSlot> slotList = new List<CraftSlot>();
    private List<EquipmentData> prev_equipmentList = new List<EquipmentData>();
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
    /// : �޴��� ����(������ ������ Ÿ���� ��������)
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void SelectMenu()
    {
        SelectMenu(Instance.nowitemType);
    }

    public static void SelectMenu(string itemType)
    {
        Instance.craftItemExplain.Init();

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

        List<EquipmentData> equipmentList = new List<EquipmentData>();

        ItemManager item = ItemManager.Instance;
        Dictionary<string, ItemData> itemDatas = item.equipment_Items;

        foreach(KeyValuePair<string,ItemData> pair in itemDatas)
        {
            EquipmentData equipmentData = (EquipmentData)pair.Value;

            //���� ���� ���ο� ���� ������ �з�
            if (equipmentData.equipmentType != equipmentType && equipmentType != EquipmentType.anything)
                continue;
            if(equipmentData.CanMake() && Instance.nowOption == CraftDropDownMenu.CanCraft)
            {
                equipmentList.Add(equipmentData);
                continue;
            }
            if (!equipmentData.CanMake() && Instance.nowOption == CraftDropDownMenu.CantCraft)
            {
                equipmentList.Add(equipmentData);
                continue;
            }
        }

        equipmentList.Sort();
        Instance.MakeItemSlot(equipmentList);

        if (equipmentList.Count == 0)
        {
            //ǥ���� �������� ������ UI ��Ȱ��ȭ
            Instance.showEmpty.SetActive(true);
            Instance.craftBtn.SetActive(false);
            ChangeNowItem(null);
        }
        else
        {
            //ǥ���� �������� �����Ƿ� UI Ȱ��ȭ
            Instance.showEmpty.SetActive(false);
            Instance.craftBtn.SetActive(true);
            if(Instance.nowItem == null)
                Instance.slotList[0].ClickSlot();
            else
            {
                bool flag = true;
                for(int i = 0; i < equipmentList.Count; i++)
                {
                    if(Instance.slotList[i].itemData == Instance.nowItem)
                    {
                        Instance.slotList[i].ClickSlot();
                        flag = false;
                        break;
                    }
                }
                if(flag)
                {
                    Instance.slotList[0].ClickSlot();
                }
            }
        }

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ������ ������ ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void MakeItemSlot(List<EquipmentData> equipmentList)
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
        {
            refreshFlag = true;
        }

        if(refreshFlag == false)
        {
            return;
        }

        //�ʿ��� �߰� ���� ������ ����
        int createNum = equipmentList.Count - slotList.Count;

        for (int i = 0; i < createNum; i++)
        {
            //�ʿ� ������ŭ ������ �߰�
            CraftSlot obj = Instantiate(slotObj);
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
                ItemData itemData = equipmentList[i];
                slotList[i].SetData(itemData);
                slotList[i].gameObject.SetActive(true);
            }
        }

        Instance.prev_equipmentList.Clear();
        foreach(EquipmentData equipment in equipmentList)
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
    /// : ������ ���� ǥ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void ShowItemExplain(ItemData itemData)
    {
        EquipmentData equipmentData = (EquipmentData)itemData;
        Instance.craftItemExplain.SetItem(equipmentData);
        Instance.nowItem = itemData;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ������ ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void ItemCraft()
    {
        if(Instance.nowItem == null)
            return;
        EquipmentData itemData = (EquipmentData)Instance.nowItem;
        ItemClass itemClass = itemData.MakeItem();
        if (itemClass != null)
        {
            //��ᰡ ����ؼ� ����� �ִٸ�
            //���ۿϷ� ǥ��
            SelectMenu();
            Instance.craftCompleteWindow.SetItem(itemData, itemClass);
        }
        else
        {
            //��ᰡ �����ϸ� ������ �޽��� ���
            OnOffNoResource(true);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���� �������� ������ ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void ChangeNowItem(ItemData itemData)
    {
        Instance.nowItem = itemData;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ��ȭ���� ���� ����Ʈ ��� ��Ȱ��ȭ
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void UnSelectSlots()
    {
        for (int i = 0; i < Instance.slotList.Count; i++)
            Instance.slotList[i].SelectSlot(false);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ��� ���� �޽��� Ȱ��/��Ȱ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffNoResource(bool state)
    {
        Instance.noResource.SetActive(state);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : Ʃ�丮�� ������ ���� Box Ȱ��/��Ȱ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffTutorialSlotBox(bool state)
    {
        if(Instance.slotList.Count >= 1)
        {
            Instance.slotList[0].OnOffTutorialBox(state);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : Ʃ�丮�� ���Ȯ�� Box Ȱ��/��Ȱ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffTutorialRequireBox(bool state)
    {
        if (Instance.craftItemExplain)
        {
            Instance.craftItemExplain.OnOffTutorialRequireBox(state);
        }
    }
}
