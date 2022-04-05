using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : Manager
{
    private static FieldManager m_instance;
    public static FieldManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                //NULL값인 경우 데이터를 매니저를 가져온다.
                GameObject managerObj = Instantiate(Resources.Load("Manager/FieldManager") as GameObject);
                m_instance = managerObj.GetComponent<FieldManager>();

                m_instance.name = "FieldManager";

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

    [System.NonSerialized] public Dictionary<string, FieldData> fieldData = new Dictionary<string, FieldData>();
    [System.NonSerialized] public Dictionary<string, int> fieldOrder = new Dictionary<string, int>();
    public static string nowField = "Home";
    private int fieldCnt = 1;

    private void Init()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            FieldData data = transform.GetChild(i).GetComponent<FieldData>();
            if(data != null)
            {
                fieldData[data.fieldName] = data;
                fieldOrder[data.fieldName] = fieldCnt;
                fieldCnt++;
            }
        }
    }

    public static int GetFieldOrder(string fieldName)
    {
        return Instance.fieldOrder[fieldName];
    }
}
