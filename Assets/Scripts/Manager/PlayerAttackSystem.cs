using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSystem : MonoBehaviour
{
    private static PlayerAttackSystem instance;

    private void Awake() => instance = this;

    public static void NormalAttack()
    {
        KeyValuePair<int, bool> damage = PlayerManager.PlayerDamage();
        int damageValue = damage.Key;
        bool isCritical = damage.Value;

        if (isCritical)
        {
            //치명타시 카메라 진동
            CameraVibration.Vibration();

            //용인의 피 처리
            damageValue = (int)(damageValue * DragonBloodSkill());
        }

        MonsterData nowMonster = FieldSystem.NowMonster();
        nowMonster.PlayAni("Damage");

        Player.AttackPlayer();

        DamageUI.ShowDamage(damageValue, isCritical);

        BattleUI.Instance.NowHp -= damageValue;
    }



    private static WolfAgilityData wolfAgilityData;
    public static void WolfAgilitySkill()
    {
        uint level = UpgradeManager.GetUpgradeLevel(Upgrade.WolfAgility);
        if(wolfAgilityData == null || wolfAgilityData.level != level)
            wolfAgilityData = (WolfAgilityData)UpgradeManager.GetUpgradeData(Upgrade.WolfAgility, level);

        float actPer = wolfAgilityData.actPer;
        uint attackNum = wolfAgilityData.attackNum;

        if (MyUtility.RandomResult(actPer, 100f))
        {
            bool critical = false;
            for (int i = 0; i < attackNum; i++)
            {
                KeyValuePair<int, bool> damage = PlayerManager.PlayerDamage();
                int damageValue = damage.Key;
                bool isCritical = damage.Value;

                critical |= isCritical;

                if (isCritical)
                {
                    //용인의 피 처리
                    damageValue = (int)(damageValue * DragonBloodSkill());
                }

                DamageUI.ShowDamage(damageValue, isCritical);

                BattleUI.Instance.NowHp -= damageValue;
            }

            MonsterData nowMonster = FieldSystem.NowMonster();
            nowMonster.PlayAni("Damage");

            if (critical)
            {
                //치명타시 카메라 진동
                CameraVibration.Vibration();
            }
        }
    }


    private static DragonBloodData dragonBloodData;
    private static float DragonBloodSkill()
    {
        uint level = UpgradeManager.GetUpgradeLevel(Upgrade.DragonBlood);
        if (dragonBloodData == null || dragonBloodData.level != level)
            dragonBloodData = (DragonBloodData)UpgradeManager.GetUpgradeData(Upgrade.DragonBlood, level);

        float actPer = dragonBloodData.actPer;
        float damageDur = dragonBloodData.damageDur;

        if (MyUtility.RandomResult(actPer, 100f))
        {
            Debug.Log("용인의 피 발동");
            return (damageDur + 100f)/100f;
        }
        return 1;
    }
}
