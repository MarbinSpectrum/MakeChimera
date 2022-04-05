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
                //NULL���� ��� �����͸� �Ŵ����� �����´�.
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
        //����������� �������� ������ �ִ� HashSet
        HashSet<string> getCheck = new HashSet<string>();
        for (int i = 0; i < len; i++)
        {
            int sum = 0;
            foreach (KeyValuePair<string, PeddlerItem> data in Instance.Items)
                if (getCheck.Contains(data.Key) == false)
                    sum += data.Value.itemSpawnPoint;

            //��� �������� �������� ����.
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

        //HashSet�� List�� �����.
        List<string> items = new List<string>();
        foreach (string itemName in getCheck)
            items.Add(itemName);
        return items;
    }
}
