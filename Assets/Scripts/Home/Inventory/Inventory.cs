using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class Inventory : UI_base
{
    private static Inventory m_instance;
    public static Inventory Instance
    {
        get
        {
            if (m_instance == null)
            {
                //NULL값인 경우 데이터를 매니저를 가져온다.
                GameObject soundManagerObj = Instantiate(Resources.Load("UI/Inventory") as GameObject);
                m_instance = soundManagerObj.GetComponent<Inventory>();

                m_instance.name = "Inventory";

                m_instance.Init();

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

    [SerializeField] private InventorySlot slotObj;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject body;

    private List<InventorySlot> slotList = new List<InventorySlot>();
    private void Init()
    {
        DataManager.LoadPlayData();
        PlayData playData = DataManager.Instance.playData;

        int createNum = playData.materials.Count - slotList.Count;

        for (int i = 0; i < createNum; i++)
        {
            InventorySlot obj = Instantiate(slotObj);
            obj.transform.parent = content;
            obj.transform.localScale = new Vector3(1, 1, 1);
            slotList.Add(obj);
        }
        for(int i = 0; i < slotList.Count; i++)
        {
            if(i >= playData.materials.Count)
                slotList[i].gameObject.SetActive(false);
            else
            {
                slotList[i].SetData(playData.materials[i]);
                slotList[i].gameObject.SetActive(true);
            }
        }
    }

    public static int GetMaterialNum(string itemDataName)
    {
        PlayData playData = DataManager.Instance.playData;
        return playData.GetMaterialNum(itemDataName);
    }

    public static bool AddItem(string itemDataName, int itemNum)
    {
        if (itemDataName.Length == 0)
            return false;
        PlayData playData = DataManager.Instance.playData;
        return playData.AddMaterial(itemDataName, itemNum);
    }

    public static bool RemoveItem(string itemDataName, int itemNum)
    {
        if (HasItem(itemDataName, itemNum) == false)
            return false;
        PlayData playData = DataManager.Instance.playData;
        return playData.RemoveItem(itemDataName, itemNum);
    }

    public static void AddItem(ItemClass itemClass)
    {
        PlayData playData = DataManager.Instance.playData;
        if (itemClass.itemType == ItemType.equitment)
            playData.equipments.Add(itemClass);
        if (itemClass.itemType == ItemType.material)
            playData.AddMaterial(itemClass.itemDataName, itemClass.itemNum);

        DataManager.SaveData();
    }

    public static bool HasItem(string itemDataName, int itemNum)
    {
        PlayData playData = DataManager.Instance.playData;
        return playData.HasItem(itemDataName, itemNum);
    }

    public static void OnOffWindow(bool state)
    {
        Instance.body.SetActive(state);
        if(state == true)
        {
            Instance.Init();
        }
    }
}
