using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public abstract class SpecialSkillSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeName;
    [SerializeField] private TextMeshProUGUI upgradeLevel;
    [SerializeField] protected TextMeshProUGUI upgradeExplain;
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private Image selectImg;
    [SerializeField] private GameObject levelUpBtn;
    [SerializeField] protected Upgrade upgrade;
    protected UpgradeData upgradeData;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���׷��̵� ���Կ� ���׷��̵� ������ ǥ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public virtual void SetUpgrade()
    {
        //���׷��̵� ���� �ε�
        uint nowLv = UpgradeManager.GetUpgradeLevel(upgrade);
        uint nextLv = nowLv + 1;
        float power = UpgradeManager.GetUpgradePower(upgrade, nextLv);
        string upGradeName = UpgradeManager.GetUpgradeName(upgrade, nextLv);
        bool nowMaxLv = UpgradeManager.NowMaxLevel(upgrade);
        Sprite upGradeIcon = UpgradeManager.GetUpgradeIcon(upgrade, nextLv);

        //UI�� �ش��ϴ� ���� ����
        if (nowMaxLv == false)
        {
            //�ִ� ������ �ƴ� ����
            upgradeName.text = upGradeName;
            upgradeLevel.text = $"Lv.{nextLv}";

            //���� �ɷ�ġ ǥ��
            string powerText = $"+{power.ToString()}";
            if (UpgradeManager.IsPercentSkill(upgrade))
                powerText += "%";

            upgradeIcon.sprite = upGradeIcon;
            levelUpBtn.SetActive(true);

            upgradeData = UpgradeManager.GetUpgradeData(upgrade, nextLv);
        }
        else
        {
            //�ִ� ������ ����
            upgradeName.text = UpgradeManager.GetUpgradeName(upgrade, nowLv);
            upgradeLevel.text = "Lv.MAX";
            upgradeIcon.sprite = UpgradeManager.GetUpgradeIcon(upgrade, nowLv);
            levelUpBtn.SetActive(false);

            upgradeData = UpgradeManager.GetUpgradeData(upgrade, nowLv);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���׷��̵� ���� ���ý� �ʿ��� ��� ǥ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void CheckRequireItem()
    {
        //���׷��̵� ���� �ε�
        uint nowLv = UpgradeManager.GetUpgradeLevel(upgrade);
        uint nextLv = nowLv + 1;
        bool nowMaxLv = UpgradeManager.NowMaxLevel(upgrade);

        if (UpgradeManager.NowMaxLevel(upgrade))
        {
            //�ִ� ������ ����
            Reinforce.OffRequireList();
        }
        else
        {
            //�ִ� ������ �ƴ� ����
            Reinforce.ShowRequireSlot(upgrade, nextLv);
        }
        Reinforce.UnSelectSlots();
        SelectSlot(true);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���׷��̵� ��ư
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void UpgradeBtn()
    {
        if (UpgradeManager.AddUpgrade(upgrade))
        {
            //���׷��̵尡 �Ϸ�Ǹ� �����츦 ����
            Reinforce.SuccessSE();
            Reinforce.RefreshWindow();
        }
        else
        {
            Reinforce.FailSE();
            Reinforce.OnOffNoResource(true);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���ÿ��� ����Ʈ Ȱ��ȭ/��Ȱ��ȭ
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SelectSlot(bool state)
    {
        selectImg.enabled = state;
    }
}
