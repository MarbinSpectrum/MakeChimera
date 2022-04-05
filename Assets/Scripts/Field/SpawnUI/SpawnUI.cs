using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUI : MonoBehaviour
{
    public static SpawnUI Instance;

    private Dictionary<string, SpawnSlot> slotList = new Dictionary<string, SpawnSlot>();
    [SerializeField] private SpawnSlot slotObj;
    [SerializeField] private Transform slotTrans;


    private void Awake() => Instance = this;

    public static void SetList(List<MonsterData> monsterDatas)
    {
        if (Instance == null)
            return;
        Dictionary<string, int> monsterList = new Dictionary<string, int>();

        //몬스터 리스트를 정리
        monsterList.Clear();
        foreach (MonsterData monsterData in monsterDatas)
        {
            string monsterName = monsterData.monsterName;
            if (monsterList.ContainsKey(monsterName))
                monsterList[monsterName]++;
            else
                monsterList[monsterName] = 1;
        }

        //슬롯리스트를 표시
        foreach (KeyValuePair<string, SpawnSlot> monsterPair in Instance.slotList)
        {
            string monsterName = monsterPair.Key;
            if (!monsterList.ContainsKey(monsterName))
            {
                GameObject slotObj = monsterPair.Value.gameObject;
                slotObj.SetActive(false);
            }
        }

        foreach (KeyValuePair<string, int> monsterPair in monsterList)
        {
            string monsterName = monsterPair.Key;
            int monsterNum = monsterPair.Value;

            if(!Instance.slotList.ContainsKey(monsterName))
            {
                //필요 갯수만큼 슬롯을 추가
                SpawnSlot obj = Instantiate(Instance.slotObj);
                obj.transform.parent = Instance.slotTrans;
                obj.transform.localPosition = Instance.slotObj.transform.localPosition;
                obj.transform.localScale = new Vector3(1, 1, 1);
                Instance.slotList[monsterName] = obj;
            }

            Instance.slotList[monsterName].SetData(monsterName, monsterNum);
            GameObject slotObj = Instance.slotList[monsterName].gameObject;
            slotObj.SetActive(true);
        }
    }

    public static void SubMonsterCnt(string monsterName)
    {
        if (Instance == null)
            return;
        if(Instance.slotList.ContainsKey(monsterName))
            Instance.slotList[monsterName].monsterNum--;
    }
}
