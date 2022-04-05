using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class DragonBloodUpgradeSlot : SpecialSkillSlot
{
    [SerializeField] private TextMeshProUGUI actPerText;
    [SerializeField] private TextMeshProUGUI damageDurText;

    public override void SetUpgrade()
    {
        base.SetUpgrade();

        DragonBloodData dragonBloodData = (DragonBloodData)upgradeData;

        upgradeExplain.text = dragonBloodData.GetExplain();
        actPerText.text = $"{dragonBloodData.actPer}%";
        damageDurText.text = $"{dragonBloodData.damageDur}%";
    }
}

