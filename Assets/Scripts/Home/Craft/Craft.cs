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
                //NULL값인 경우 데이터를 매니저를 가져온다.
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
    /// : 메뉴를 갱신(선택한 아이템 타입을 기준으로)
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
            //현재 선택중인 메뉴만 이펙트를 킨다. 
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

            //제작 가능 여부에 따라 아이템 분류
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
            //표시할 아이템이 없으면 UI 비활성화
            Instance.showEmpty.SetActive(true);
            Instance.craftBtn.SetActive(false);
            ChangeNowItem(null);
        }
        else
        {
            //표시할 아이템이 있으므로 UI 활성화
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
    /// : 아이템 슬롯을 생성
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void MakeItemSlot(List<EquipmentData> equipmentList)
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
        {
            refreshFlag = true;
        }

        if(refreshFlag == false)
        {
            return;
        }

        //필요한 추가 슬롯 갯수를 상정
        int createNum = equipmentList.Count - slotList.Count;

        for (int i = 0; i < createNum; i++)
        {
            //필요 갯수만큼 슬롯을 추가
            CraftSlot obj = Instantiate(slotObj);
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
                ItemData itemData = equipmentList[i];
                slotList[i].SetData(itemData);
                slotList[i].gameObject.SetActive(true);
            }
        }

        Instance.prev_equipmentList.Clear();
        foreach(EquipmentData equipment in equipmentList)
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
    /// : 아이템 정보 표시
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void ShowItemExplain(ItemData itemData)
    {
        EquipmentData equipmentData = (EquipmentData)itemData;
        Instance.craftItemExplain.SetItem(equipmentData);
        Instance.nowItem = itemData;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 아이템 제작
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void ItemCraft()
    {
        if(Instance.nowItem == null)
            return;
        EquipmentData itemData = (EquipmentData)Instance.nowItem;
        ItemClass itemClass = itemData.MakeItem();
        if (itemClass != null)
        {
            //재료가 충분해서 만들수 있다면
            //제작완료 표시
            SelectMenu();
            Instance.craftCompleteWindow.SetItem(itemData, itemClass);
        }
        else
        {
            //재료가 부족하면 재료부족 메시지 출력
            OnOffNoResource(true);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 현재 선택중인 아이템 변경
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void ChangeNowItem(ItemData itemData)
    {
        Instance.nowItem = itemData;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 강화슬롯 선택 이펙트 모두 비활성화
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void UnSelectSlots()
    {
        for (int i = 0; i < Instance.slotList.Count; i++)
            Instance.slotList[i].SelectSlot(false);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 재료 부족 메시지 활성/비활성
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffNoResource(bool state)
    {
        Instance.noResource.SetActive(state);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 튜토리얼 아이템 선택 Box 활성/비활성
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffTutorialSlotBox(bool state)
    {
        if(Instance.slotList.Count >= 1)
        {
            Instance.slotList[0].OnOffTutorialBox(state);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 튜토리얼 재료확인 Box 활성/비활성
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffTutorialRequireBox(bool state)
    {
        if (Instance.craftItemExplain)
        {
            Instance.craftItemExplain.OnOffTutorialRequireBox(state);
        }
    }
}
