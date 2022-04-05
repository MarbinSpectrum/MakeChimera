using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Custom;

public class GameManager : SerializedMonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    [LabelText("���� ���� �ֱ�")] 
    private float term = 3;

    private bool nowBattle = false;
    private bool eventRun = false;
    private bool runCycle = true;
    private bool monsterEmpty = true;
    private bool spawnPeddler = false;

    private Dictionary<MonsterClass, int> killCount = new Dictionary<MonsterClass, int>();
    private Queue<MonsterData> spawnMonsterList = new Queue<MonsterData>();

    private void Awake()
    {
        Instance = this;
    }

    public static void GameStart()
    {
        ClearKill_Count();
        Instance.StartCoroutine("GameCycle");
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : (���� -> ��� -> ����)�� ����Ŭ�� ����ȴ�.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private IEnumerator GameCycle()
    {
        yield return new WaitWhile(() => { return runCycle == false; });
        yield return new WaitWhile(() => { return eventRun; });         // ������ �����⸦ ��ٸ���.

        yield return new WaitWhile(() => { return runCycle == false; });
        yield return new WaitForSeconds(term);

        yield return new WaitWhile(() => { return runCycle == false; });

        eventRun = true;

        //������ ������ ������� �������� ���Ͱ� ������ �˻��Ѵ�.
        if (CheckSpawnPeddler())
        {
            Instance.spawnPeddler = false;

            //������� ���´ٸ�
            FieldSystem.SpawnPeddler();
        }
        else
        {
            if(monsterEmpty)
            {
                Instance.CreateMonsterSpawnList();
            }

            //���Ͱ� ���´ٸ�
            MonsterData nowMonster = spawnMonsterList.Dequeue();
            FieldSystem.SpawnMonster(nowMonster);
        }

        StartCoroutine("GameCycle");

        yield return null;
    }

    private void CreateMonsterSpawnList()
    {
        spawnMonsterList.Clear();

        //�����ʵ� �̸��� �����´�.
        string fieldName = FieldManager.nowField;

        //��� ������ ������ ����
        List<MonsterData> bossList = MonsterManager.GetMonsterList_Random(fieldName, MonsterClass.Boss, 1);
        MonsterData bossMonster = null;
        if (bossList.Count > 0)
            bossMonster = bossList[0];
        if(bossMonster == null)
        {
            //������ ���� ������ �Ǵ�
            //������ ���ͷ� ���� ����Ʈ�� ���� 
            
            for(int i = 0; i < 20; i++)
            {
                //�뷫 20�������� �����ϵ��� ����
                foreach (MonsterClass monsterClass in Enum.GetValues(typeof(MonsterClass)))
                {
                    List<MonsterData> tempList = MonsterManager.GetMonsterList_Random(fieldName, monsterClass, 1);
                    if (tempList.Count > 0)
                    {
                        spawnMonsterList.Enqueue(tempList[0]);
                        break;
                    }
                }
            }
        }
        else
        {
            List<MonsterData> monsterList = new List<MonsterData>();
            foreach(KeyValuePair<MonsterClass, int> requireMonster in bossMonster.spawnConditions)
            {
                MonsterClass monsterClass = requireMonster.Key;
                int monsterNum = requireMonster.Value;

                List<MonsterData> tempList = MonsterManager.GetMonsterList_Random(fieldName, monsterClass, monsterNum);
                foreach (MonsterData monsterData in tempList)
                    monsterList.Add(monsterData);
            }

            //�ش� ����� UI�� ���
            SpawnUI.SetList(monsterList);

            //���� ��� ����
            Custom.Fun.Shuffle(ref monsterList);

            monsterList.Add(bossMonster);

            //���� ����� ��������Ʈ�� ���
            foreach (MonsterData monsterData in monsterList)
                spawnMonsterList.Enqueue(monsterData);

        }

        monsterEmpty = false;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���� ���� ó��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void BattleStart()
    {
        MonsterData nowMonster = FieldSystem.NowMonster();

        if (nowMonster == null)
        {
            Debug.LogError("���� ���Ͱ� �������� �ʽ��ϴ�.");
            BattleEnd();
            return;
        }

        BackGround.StopAni();
        Player.Animation("Idle");
        BattleUI.OnOffWindow(true);
        BattleUI.BattleSetting();
        Instance.nowBattle = true;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���� ���� ó��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void BattleEnd()
    {
        MonsterData nowMonster = FieldSystem.NowMonster();

        if(nowMonster != null)
        {
            BackGround.RunAni();
            Player.Animation("Run");
            BattleUI.OnOffWindow(false);
            nowMonster.DeathMonster();
            FieldSystem.BattleEnd();
        }

        Instance.nowBattle = false;
        Instance.eventRun = false;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ����ΰ� ��ȭ ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void TalkToPeddlerStart()
    {
        BackGround.StopAni();
        Player.Animation("Idle");
        PeddlerUI.Init();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ����ΰ� ��ȭ ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void TalkToPeddlerEnd()
    {
        BackGround.RunAni();
        Player.Animation("Run");

        Instance.eventRun = false;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ų ī��Ʈ
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void KillAdd(MonsterClass monsterClass)
    {
        KillAdd(monsterClass, 1);
    }
    public static void KillAdd(MonsterClass monsterClass, int add)
    {
        Instance.killCount[monsterClass] += add;
    }

    public static int GetKill_Count(MonsterClass monsterClass)
    {
        return Instance.killCount[monsterClass];
    }

    public static void ClearKill_Count()
    {
        Instance.killCount.Clear();
        foreach (MonsterClass mc in Enum.GetValues(typeof(MonsterClass)))
            Instance.killCount.Add(mc, 0);
        Instance.monsterEmpty = true;
        Instance.spawnMonsterList.Clear();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���� ����Ʈ�� ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void CheckMonsterList()
    {
        if(Instance.spawnMonsterList.Count == 0)
        {
            Instance.monsterEmpty = true;
            Instance.spawnPeddler = true;

            //�������� ���� ���� �������� �����Ȳ ����
            PlayData playData = DataManager.Instance.playData;

            int nowStage = playData.nowStage;
            nowStage = Mathf.Max(nowStage, FieldManager.GetFieldOrder(FieldManager.nowField));
            playData.nowStage = nowStage;

            DataManager.SaveData();
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ������� �����ϴ� ������ ��������� Ȯ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private bool CheckSpawnPeddler()
    {
        return spawnPeddler;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �̺�Ʈ������ Ȯ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool GetEventRun()
    {
        return Instance.eventRun;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ����Ŭ�� ���������� Ȯ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool GetRunCycle()
    {
        return Instance.runCycle;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ����Ŭ ON/OFF ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void SetRunCycle(bool state)
    {
        Instance.runCycle = state;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ������ �������� Ȯ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool GetNowBattle()
    {
        return Instance.nowBattle;
    }
}
