using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PeddlerManager : Manager
{
    private static PeddlerManager m_instance;
    public static PeddlerManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                //NULL값인 경우 데이터를 매니저를 가져온다.
                GameObject managerObj = Instantiate(Resources.Load("Manager/PeddlerManager") as GameObject);
                m_instance = managerObj.GetComponent<PeddlerManager>();

                m_instance.name = "PeddlerManager";

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

    [System.NonSerialized] public Dictionary<string, PeddlerItem> Items = new Dictionary<string, PeddlerItem>();

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Init()
    {
        FindItemData(transform, ref Items);
    }

    private void FindItemData(Transform transform, ref Dictionary<string, PeddlerItem> dataList)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            PeddlerItem data = child.GetComponent<PeddlerItem>();
            if (data != null)
            {
                string itemName = data.itemDataName;
                dataList.Add(itemName, data);
            }

            FindItemData(child, ref dataList);
        }
    }

    public static List<string> GetItemList(int len)
    {
        //가져오기로한 아이템을 가지고 있는 HashSet
        HashSet<string> getCheck = new HashSet<string>();
        for (int i = 0; i < len; i++)
        {
            int sum = 0;
            foreach (KeyValuePair<string, PeddlerItem> data in Instance.Items)
                if (getCheck.Contains(data.Key) == false)
                    sum += data.Value.itemSpawnPoint;

            //어떠한 아이템을 선택할지 고른다.
            int r = Random.Range(0, sum);
            foreach (KeyValuePair<string, PeddlerItem> data in Instance.Items)
            {
                if (getCheck.Contains(data.Key) == false)
                {
                    if (r < data.Value.itemSpawnPoint)
                    {
                        getCheck.Add(data.Key);
                        break;
                    }
                    r -= data.Value.itemSpawnPoint;
                }
            }
        }

        //HashSet을 List로 만든다.
        List<string> items = new List<string>();
        foreach (string itemName in getCheck)
            items.Add(itemName);
        return items;
    }
}
