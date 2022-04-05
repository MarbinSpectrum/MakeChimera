using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UpgradeSlot : SerializedMonoBehaviour
{
    [SerializeField] private TextMeshProUGUI upgradeName;
    [SerializeField] private TextMeshProUGUI upgradeLevel;
    [SerializeField] private TextMeshProUGUI upgradePower;
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private GameObject levelUpBtn;
    [SerializeField] private Image selectImg;

    protected Upgrade thisUpgrade;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���׷��̵� ���Կ� ���׷��̵� ������ ǥ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public virtual void SetUpgrade(Upgrade upgrade)
    {
        thisUpgrade = upgrade;

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
            upgradePower.text = powerText;

            upgradeIcon.sprite = upGradeIcon;
            levelUpBtn.SetActive(true);
        }
        else
        {
            //�ִ� ������ ����
            upgradeName.text = UpgradeManager.GetUpgradeName(upgrade, nowLv);
            upgradeLevel.text = "Lv.MAX";
            upgradePower.text = " ";
            upgradeIcon.sprite = UpgradeManager.GetUpgradeIcon(upgrade, nowLv);
            levelUpBtn.SetActive(false);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���׷��̵� ���� ���ý� �ʿ��� ��� ǥ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void CheckRequireItem()
    {
        //���׷��̵� ���� �ε�
        uint nowLv = UpgradeManager.GetUpgradeLevel(thisUpgrade);
        uint nextLv = nowLv + 1;
        bool nowMaxLv = UpgradeManager.NowMaxLevel(thisUpgrade);

        if (UpgradeManager.NowMaxLevel(thisUpgrade))
        {
            //�ִ� ������ ����
            Reinforce.OffRequireList();
        }
        else
        {
            //�ִ� ������ �ƴ� ����
            Reinforce.ShowRequireSlot(thisUpgrade, nextLv);
        }
        Reinforce.UnSelectSlots();
        SelectSlot(true);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���׷��̵� ��ư
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void UpgradeBtn()
    {
        if(UpgradeManager.AddUpgrade(thisUpgrade))
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
