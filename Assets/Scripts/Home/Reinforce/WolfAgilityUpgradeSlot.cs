using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class WolfAgilityUpgradeSlot : SpecialSkillSlot
{
    [SerializeField] private TextMeshProUGUI actPerText;
    [SerializeField] private TextMeshProUGUI attackNumText;

    public override void SetUpgrade()
    {
        base.SetUpgrade();

        WolfAgilityData wolfAgilityData = (WolfAgilityData)upgradeData;

        upgradeExplain.text = wolfAgilityData.GetExplain();
        actPerText.text = $"{wolfAgilityData.actPer}%";
        attackNumText.text = $"{wolfAgilityData.attackNum}회";
    }
}
