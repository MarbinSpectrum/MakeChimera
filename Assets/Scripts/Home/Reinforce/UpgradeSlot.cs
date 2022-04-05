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
    /// : 업그레이드 슬롯에 업그레이드 정보를 표시
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public virtual void SetUpgrade(Upgrade upgrade)
    {
        thisUpgrade = upgrade;

        //업그레이드 정보 로드
        uint nowLv = UpgradeManager.GetUpgradeLevel(upgrade);
        uint nextLv = nowLv + 1;
        float power = UpgradeManager.GetUpgradePower(upgrade, nextLv);
        string upGradeName = UpgradeManager.GetUpgradeName(upgrade, nextLv);
        bool nowMaxLv = UpgradeManager.NowMaxLevel(upgrade);
        Sprite upGradeIcon = UpgradeManager.GetUpgradeIcon(upgrade, nextLv);

        //UI에 해당하는 정보 설정
        if (nowMaxLv == false)
        {
            //최대 레벨이 아닌 상태
            upgradeName.text = upGradeName;
            upgradeLevel.text = $"Lv.{nextLv}";

            //증가 능력치 표시
            string powerText = $"+{power.ToString()}";
            if (UpgradeManager.IsPercentSkill(upgrade))
                powerText += "%";
            upgradePower.text = powerText;

            upgradeIcon.sprite = upGradeIcon;
            levelUpBtn.SetActive(true);
        }
        else
        {
            //최대 레벨인 상태
            upgradeName.text = UpgradeManager.GetUpgradeName(upgrade, nowLv);
            upgradeLevel.text = "Lv.MAX";
            upgradePower.text = " ";
            upgradeIcon.sprite = UpgradeManager.GetUpgradeIcon(upgrade, nowLv);
            levelUpBtn.SetActive(false);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 업그레이드 슬롯 선택시 필요한 재료 표시
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void CheckRequireItem()
    {
        //업그레이드 정보 로드
        uint nowLv = UpgradeManager.GetUpgradeLevel(thisUpgrade);
        uint nextLv = nowLv + 1;
        bool nowMaxLv = UpgradeManager.NowMaxLevel(thisUpgrade);

        if (UpgradeManager.NowMaxLevel(thisUpgrade))
        {
            //최대 레벨인 상태
            Reinforce.OffRequireList();
        }
        else
        {
            //최대 레벨이 아닌 상태
            Reinforce.ShowRequireSlot(thisUpgrade, nextLv);
        }
        Reinforce.UnSelectSlots();
        SelectSlot(true);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 업그레이드 버튼
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void UpgradeBtn()
    {
        if(UpgradeManager.AddUpgrade(thisUpgrade))
        {
            //업그레이드가 완료되면 윈도우를 갱신
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
    /// : 선택여부 이펙트 활성화/비활성화
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SelectSlot(bool state)
    {
        selectImg.enabled = state;
    }
}
