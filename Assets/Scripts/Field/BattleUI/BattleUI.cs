using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUI : MonoBehaviour
{
    public static BattleUI Instance;

    [SerializeField] private Image barHp;
    [SerializeField] private GameObject body;
    [SerializeField] private TextMeshProUGUI barText;
    [SerializeField] private TextMeshProUGUI monsterClassText;
    [SerializeField] private TextMeshProUGUI monsterNameText;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color eliteColor;
    [SerializeField] private Color bossColor;

    private string monsterName;
    private int maxHp;
    private int nowHp;
    public int NowHp
    {
        set
        {
            nowHp = value;
            UpdateHp();

            if (nowHp <= 0)
            {
                //몬스터의 체력이 
                GameManager.BattleEnd();
                OnOffWindow(false);
            }
        }
        get
        {
            return nowHp;
        }
    }

    private void Awake() => Instance = this;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 몬스터에 해당하는 UI 설정 
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void BattleSetting()
    {
        MonsterData nowMonster = FieldSystem.NowMonster();

        MonsterClass nowClass = nowMonster.monsterClass;

        //몬스터 등급 설정
        if (nowClass == MonsterClass.Normal)
            Instance.monsterClassText.color = Instance.normalColor;
        else if (nowClass == MonsterClass.Elite)
            Instance.monsterClassText.color = Instance.eliteColor;
        else if (nowClass == MonsterClass.Boss)
            Instance.monsterClassText.color = Instance.bossColor;
        Instance.monsterClassText.text = nowClass.ToString();

        //몬스터 체력 설정
        Instance.maxHp = nowMonster.monsterHp;
        Instance.nowHp = Instance.maxHp;
        Instance.barHp.fillAmount = 1;
        Instance.UpdateHp();

        //몬스터 이름 설정
        Instance.monsterNameText.text = nowMonster.monsterName;
        Instance.monsterName = nowMonster.monsterName;

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : UI에 HP표시(%)
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void UpdateHp()
    {
        float per = ((float)(Mathf.Max(0, nowHp)) / (float)maxHp * 100);
        barText.text = string.Format("{0:0.00}", per) + "%";

        barHp.fillAmount = (float)nowHp / (float)maxHp;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : UI상태 ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffWindow(bool state)
    {
        Instance.body.SetActive(state);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 플레이어 공격시 처리
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void AttackPlayer()
    {
        PlayerAttackSystem.NormalAttack();
        PlayerAttackSystem.WolfAgilitySkill();
    }

}
