using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DragonBloodData : UpgradeData
{
    [Title("용인의 피 스킬 정보")]
    [LabelText("능력 설명")]
    [InfoBox("{a}는 발동확률, {b}는 데미지 배율")]
    [TextArea]
    public string explain;
    public string GetExplain()
    {
        string temp = "";
        for (int i = 0; i < explain.Length; i++)
        {
            if (i + 2 < explain.Length)
                if (explain[i] == '{' && explain[i + 2] == '}')
                    if (explain[i + 1] == 'a')
                    {
                        temp += $"{actPer}%";
                        i += 2;
                        continue;
                    }
                    else if (explain[i + 1] == 'b')
                    {
                        temp += $"{damageDur}%";
                        i += 2;
                        continue;
                    }
            temp += explain[i];
        }
        return temp;
    }

    [LabelText("발동 확률(%)")]
    [Range(0, 100)]
    public float actPer;

    [LabelText("데미지 배율(%)")]
    public float damageDur;
}
