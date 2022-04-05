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
                //������ ü���� 
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
    /// : ���Ϳ� �ش��ϴ� UI ���� 
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void BattleSetting()
    {
        MonsterData nowMonster = FieldSystem.NowMonster();

        MonsterClass nowClass = nowMonster.monsterClass;

        //���� ��� ����
        if (nowClass == MonsterClass.Normal)
            Instance.monsterClassText.color = Instance.normalColor;
        else if (nowClass == MonsterClass.Elite)
            Instance.monsterClassText.color = Instance.eliteColor;
        else if (nowClass == MonsterClass.Boss)
            Instance.monsterClassText.color = Instance.bossColor;
        Instance.monsterClassText.text = nowClass.ToString();

        //���� ü�� ����
        Instance.maxHp = nowMonster.monsterHp;
        Instance.nowHp = Instance.maxHp;
        Instance.barHp.fillAmount = 1;
        Instance.UpdateHp();

        //���� �̸� ����
        Instance.monsterNameText.text = nowMonster.monsterName;
        Instance.monsterName = nowMonster.monsterName;

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : UI�� HPǥ��(%)
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void UpdateHp()
    {
        float per = ((float)(Mathf.Max(0, nowHp)) / (float)maxHp * 100);
        barText.text = string.Format("{0:0.00}", per) + "%";

        barHp.fillAmount = (float)nowHp / (float)maxHp;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : UI���� ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffWindow(bool state)
    {
        Instance.body.SetActive(state);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �÷��̾� ���ݽ� ó��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void AttackPlayer()
    {
        PlayerAttackSystem.NormalAttack();
        PlayerAttackSystem.WolfAgilitySkill();
    }

}
