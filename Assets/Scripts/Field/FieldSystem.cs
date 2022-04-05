using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FieldSystem : MonoBehaviour
{
    public static FieldSystem Instance;

    [SerializeField] private PlayableDirector spawnMonsterDirector;
    [SerializeField] private Transform createPoint;
    [System.NonSerialized] public MonsterData nowMonster;
    [SerializeField] private Animator peddlerAni;
    [SerializeField] private PlayableDirector spawnPeddlerDirector;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        spawnMonsterDirector.stopped += BattleStart;
    }

    public static void SpawnMonster(MonsterData monsterData)
    {
        if (Instance == null)
            return;
        Instance.nowMonster = Instantiate(monsterData);
        Instance.nowMonster.transform.parent = Instance.createPoint;
        Instance.nowMonster.transform.localPosition = Vector3.zero;
        Instance.nowMonster.gameObject.SetActive(true);

        Instance.spawnMonsterDirector.Play();
    }

    public static MonsterData NowMonster()
    {
        return Instance.nowMonster;
    }

    public static void SpawnPeddler()
    {
        if (Instance == null)
            return;
        Instance.spawnPeddlerDirector.Play();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���� ������ GameManager���� �˷���
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private static void BattleStart(PlayableDirector aDirector)
    {
        GameManager.BattleStart();
        Instance.nowMonster.PlayAni("Idle");
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���� ���� ó��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void BattleEnd()
    {
        Destroy(Instance.nowMonster.gameObject);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ����ΰ� ��ȭ ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void TalktoPeddlerStart()
    {
        GameManager.TalkToPeddlerStart();
        Instance.peddlerAni.SetTrigger("Idle");
        Instance.spawnPeddlerDirector.Pause();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ����ΰ� ��ȭ ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void TalktoPeddlerEnd()
    {
        GameManager.TalkToPeddlerEnd();
        Instance.peddlerAni.SetTrigger("Run");
        Instance.spawnPeddlerDirector.Resume();
    }
}
