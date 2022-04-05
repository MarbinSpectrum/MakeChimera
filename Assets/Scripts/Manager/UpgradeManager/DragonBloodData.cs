using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DragonBloodData : UpgradeData
{
    [Title("������ �� ��ų ����")]
    [LabelText("�ɷ� ����")]
    [InfoBox("{a}�� �ߵ�Ȯ��, {b}�� ������ ����")]
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

    [LabelText("�ߵ� Ȯ��(%)")]
    [Range(0, 100)]
    public float actPer;

    [LabelText("������ ����(%)")]
    public float damageDur;
}
