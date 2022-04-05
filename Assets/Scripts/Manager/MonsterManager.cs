using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Manager
{
    private static MonsterManager m_instance;
    public static MonsterManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                //NULL값인 경우 데이터를 매니저를 가져온다.
                GameObject managerObj = Instantiate(Resources.Load("Manager/MonsterManager") as GameObject);
                m_instance = managerObj.GetComponent<MonsterManager>();

                m_instance.name = "MonsterManager";

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

    [System.NonSerialized] public Dictionary<string, List<MonsterData>> fieldMonsterData = new Dictionary<string, List<MonsterData>>();

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Init()
    {
        //몬스터 데이터를 가져온다.
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform temp = transform.GetChild(i);
            string fieldName = temp.name;

            fieldMonsterData[fieldName] = new List<MonsterData>();
            List<MonsterData> monsterList = fieldMonsterData[fieldName];
            FindMonsterData(temp, fieldName);
        }
    }

    private void FindMonsterData(Transform transform, string fieldName)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            MonsterData data = transform.GetChild(i).GetComponent<MonsterData>();

            if (data != null)
            {
                Animator animator = transform.GetChild(i).GetComponent<Animator>();
                if(animator)
                    data.animator = animator;
                fieldMonsterData[fieldName].Add(data);
            }

            FindMonsterData(child, fieldName);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 몬스터 데이터를 반환
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static MonsterData GetMonster(string monsterName)
    {
        foreach(KeyValuePair<string, List<MonsterData>> fieldData in Instance.fieldMonsterData)
        {
            string fieldName = fieldData.Key;
            MonsterData monsterData = GetMonster(fieldName, monsterName);
            if (monsterData != null)
                return monsterData;
        }
        return null;
    }

    public static MonsterData GetMonster(string fieldName, string monsterName)
    {
        List<MonsterData> monsterDataList = GetMonsterList(fieldName);
        foreach (MonsterData monsterData in monsterDataList)
            if (monsterData.monsterName == monsterName)
                return monsterData;
        return null;
    }

    public static List<MonsterData> GetMonsterList(string fieldName)
    {
        return Instance.fieldMonsterData[fieldName];
    }

    public static List<MonsterData> GetMonsterList(string fieldName, MonsterClass monsterClass)
    {
        List<MonsterData> monsterList = new List<MonsterData>();
        List<MonsterData> monsterDataList = Instance.fieldMonsterData[fieldName];

        foreach (MonsterData monsterData in monsterDataList)
            if (monsterData.monsterClass == monsterClass)
                monsterList.Add(monsterData);

        return monsterList;
    }

    public static List<MonsterData> GetMonsterList_Random(string fieldName, MonsterClass monsterClass, int monsterCnt)
    {
        List<MonsterData> monsterDataList = GetMonsterList(fieldName, monsterClass);
        List<MonsterData> monsterList = new List<MonsterData>();

        //총합 스폰 확률
        int totalSpawn = 0;
        foreach (MonsterData monsterData in monsterDataList)
            totalSpawn += monsterData.spawnPer;

        for (int i = 0; i < monsterCnt; i++)
        {
            int r = Random.Range(0, totalSpawn);

            foreach (MonsterData monsterData in monsterDataList)
            {
                if (r >= monsterData.spawnPer)
                {
                    r -= monsterData.spawnPer;
                }
                else
                {
                    monsterList.Add(monsterData);
                    break;
                }
            }
        }

        return monsterList;
    }
}
