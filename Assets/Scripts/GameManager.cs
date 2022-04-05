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
    [LabelText("몬스터 등장 주기")] 
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
    /// : (전투 -> 대기 -> 전투)의 사이클이 진행된다.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private IEnumerator GameCycle()
    {
        yield return new WaitWhile(() => { return runCycle == false; });
        yield return new WaitWhile(() => { return eventRun; });         // 전투가 끝나기를 기다린다.

        yield return new WaitWhile(() => { return runCycle == false; });
        yield return new WaitForSeconds(term);

        yield return new WaitWhile(() => { return runCycle == false; });

        eventRun = true;

        //전투가 끝나면 행상인이 등장할지 몬스터가 나올지 검사한다.
        if (CheckSpawnPeddler())
        {
            Instance.spawnPeddler = false;

            //행상인이 나온다면
            FieldSystem.SpawnPeddler();
        }
        else
        {
            if(monsterEmpty)
            {
                Instance.CreateMonsterSpawnList();
            }

            //몬스터가 나온다면
            MonsterData nowMonster = spawnMonsterList.Dequeue();
            FieldSystem.SpawnMonster(nowMonster);
        }

        StartCoroutine("GameCycle");

        yield return null;
    }

    private void CreateMonsterSpawnList()
    {
        spawnMonsterList.Clear();

        //현재필드 이름를 가져온다.
        string fieldName = FieldManager.nowField;

        //어떠한 보스가 나올지 결정
        List<MonsterData> bossList = MonsterManager.GetMonsterList_Random(fieldName, MonsterClass.Boss, 1);
        MonsterData bossMonster = null;
        if (bossList.Count > 0)
            bossMonster = bossList[0];
        if(bossMonster == null)
        {
            //보스가 없는 맵으로 판단
            //임의의 몬스터로 몬스터 리스트를 생성 
            
            for(int i = 0; i < 20; i++)
            {
                //대략 20마리정도 생성하도록 만듬
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

            //해당 목록을 UI로 출력
            SpawnUI.SetList(monsterList);

            //몬스터 목록 섞기
            Custom.Fun.Shuffle(ref monsterList);

            monsterList.Add(bossMonster);

            //만든 목록을 스폰리스트에 등록
            foreach (MonsterData monsterData in monsterList)
                spawnMonsterList.Enqueue(monsterData);

        }

        monsterEmpty = false;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 전투 시작 처리
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void BattleStart()
    {
        MonsterData nowMonster = FieldSystem.NowMonster();

        if (nowMonster == null)
        {
            Debug.LogError("현재 몬스터가 존재하지 않습니다.");
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
    /// : 전투 종료 처리
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
    /// : 행상인과 대화 시작
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void TalkToPeddlerStart()
    {
        BackGround.StopAni();
        Player.Animation("Idle");
        PeddlerUI.Init();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 행상인과 대화 종료
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void TalkToPeddlerEnd()
    {
        BackGround.RunAni();
        Player.Animation("Run");

        Instance.eventRun = false;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 킬 카운트
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
    /// : 몬스터 리스트를 갱신
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void CheckMonsterList()
    {
        if(Instance.spawnMonsterList.Count == 0)
        {
            Instance.monsterEmpty = true;
            Instance.spawnPeddler = true;

            //스테이지 종료 현재 스테이지 진행상황 갱신
            PlayData playData = DataManager.Instance.playData;

            int nowStage = playData.nowStage;
            nowStage = Mathf.Max(nowStage, FieldManager.GetFieldOrder(FieldManager.nowField));
            playData.nowStage = nowStage;

            DataManager.SaveData();
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 행상인이 등장하는 조건이 충족됬는지 확인
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private bool CheckSpawnPeddler()
    {
        return spawnPeddler;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 이벤트중인지 확인
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool GetEventRun()
    {
        return Instance.eventRun;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 사이클이 실행중인지 확인
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool GetRunCycle()
    {
        return Instance.runCycle;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 사이클 ON/OFF 설정
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void SetRunCycle(bool state)
    {
        Instance.runCycle = state;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 전투를 실행인지 확인
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool GetNowBattle()
    {
        return Instance.nowBattle;
    }
}
