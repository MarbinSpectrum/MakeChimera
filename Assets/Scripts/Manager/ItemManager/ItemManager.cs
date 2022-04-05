using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Manager
{
    private static ItemManager m_instance;
    public static ItemManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                //NULL값인 경우 데이터를 매니저를 가져온다.
                GameObject managerObj = Instantiate(Resources.Load("Manager/ItemManager") as GameObject);
                m_instance = managerObj.GetComponent<ItemManager>();

                m_instance.name = "ItemManager";

                m_instance.Init();

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

    [System.NonSerialized] public Dictionary<string, ItemData> material_Items = new Dictionary<string, ItemData>();
    [System.NonSerialized] public Dictionary<string, ItemData> equipment_Items = new Dictionary<string, ItemData>();
    [System.NonSerialized] public Dictionary<string, int> item_Order = new Dictionary<string, int>();

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    [SerializeField] private Transform materials;
    [SerializeField] private Transform equipments;

    private void Init()
    {
        FindItemData(materials, ref material_Items);
        FindItemData(equipments, ref equipment_Items);
    }

    private void FindItemData(Transform transform, ref Dictionary<string, ItemData> dataList)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!child.gameObject.activeSelf)
                continue;
            ItemData data = child.GetComponent<ItemData>();
            if (data != null)
            {
                data.itemDataName = data.transform.name;
                dataList.Add(data.itemDataName, data);          
            }

            FindItemData(child, ref dataList);
        }
    }
    public static ItemType IsItemType(string itemName)
    {
        if (Instance.material_Items.ContainsKey(itemName))
            return ItemType.material;
        else
            return ItemType.equitment;
    }
    public static int GetItemOrder(string itemName)
    {
        if(Instance.item_Order.ContainsKey(itemName) == false)
        {
            EquipmentData equipmentData = (EquipmentData)Instance.equipment_Items[itemName];
            List<string> spawnArea = GetRequireItemField(equipmentData);

            int itemOrder = int.MaxValue;
            foreach (string fieldName in spawnArea)
            {
                //재료템 드랍 필드중 순서가 가장 먼저인 필드를 검출
                int fieldOrder = FieldManager.GetFieldOrder(fieldName);
                itemOrder = Mathf.Min(itemOrder, fieldOrder);
            }

            Instance.item_Order[itemName] = itemOrder;
        }
        return Instance.item_Order[itemName];
    }
    public static List<string> GetSpawnField(ItemData itemData)
    {
        return GetSpawnField(itemData.itemName);
    }
    public static List<string> GetSpawnField(string itemName)
    {
        List<string> fieldList = new List<string>();

        MonsterManager monsterManager = MonsterManager.Instance;
        foreach (KeyValuePair<string, List<MonsterData>> fieldData in monsterManager.fieldMonsterData)
        {
            string fieldName = fieldData.Key;
            List<MonsterData> fieldmonsterList = fieldData.Value;

            //필드가 튜토리얼이면 포함안함
            if (fieldName == "튜토리얼")
                continue;

            bool findField = false;
            for (int i = 0; (i < fieldmonsterList.Count && !findField); i++)
            {
                //필드에 나오는 몬스터에 해당 아이템이 드랍되는지 검사
                foreach (DropItemData dropItemData in fieldmonsterList[i].dropitemData)
                {
                    //몬스터의 드랍 데이터를 하나씩 검사
                    if (dropItemData.itemName == itemName)
                    {
                        fieldList.Add(fieldData.Key);
                        findField = true;
                        break;
                    }
                }
            }
        }

        return fieldList;
    }
    public static List<string> GetRequireItemField(EquipmentData equipmentData)
    {
        HashSet<string> fieldSet = new HashSet<string>();
        List<RequireItemClass> requireItems = equipmentData.requireItem;

        foreach (RequireItemClass item in requireItems)
        {
            //포함 재료템을 통해서 필요한 아이템이 나오는 리스트를 작성
            List<string> tempFieldList = GetSpawnField(item.item);
            foreach(string fieldName in tempFieldList)
                fieldSet.Add(fieldName);
        }

        //리스트로 변경
        List<string> fieldList = new List<string>();
        foreach (string fieldName in fieldSet)
            fieldList.Add(fieldName);
        return fieldList;
    }
}
