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
    /// : 업그레이드 슬롯에 업그레이드 정보를 표시
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public virtual void SetUpgrade()
    {
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

            upgradeIcon.sprite = upGradeIcon;
            levelUpBtn.SetActive(true);

            upgradeData = UpgradeManager.GetUpgradeData(upgrade, nextLv);
        }
        else
        {
            //최대 레벨인 상태
            upgradeName.text = UpgradeManager.GetUpgradeName(upgrade, nowLv);
            upgradeLevel.text = "Lv.MAX";
            upgradeIcon.sprite = UpgradeManager.GetUpgradeIcon(upgrade, nowLv);
            levelUpBtn.SetActive(false);

            upgradeData = UpgradeManager.GetUpgradeData(upgrade, nowLv);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 업그레이드 슬롯 선택시 필요한 재료 표시
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void CheckRequireItem()
    {
        //업그레이드 정보 로드
        uint nowLv = UpgradeManager.GetUpgradeLevel(upgrade);
        uint nextLv = nowLv + 1;
        bool nowMaxLv = UpgradeManager.NowMaxLevel(upgrade);

        if (UpgradeManager.NowMaxLevel(upgrade))
        {
            //최대 레벨인 상태
            Reinforce.OffRequireList();
        }
        else
        {
            //최대 레벨이 아닌 상태
            Reinforce.ShowRequireSlot(upgrade, nextLv);
        }
        Reinforce.UnSelectSlots();
        SelectSlot(true);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 업그레이드 버튼
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void UpgradeBtn()
    {
        if (UpgradeManager.AddUpgrade(upgrade))
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
