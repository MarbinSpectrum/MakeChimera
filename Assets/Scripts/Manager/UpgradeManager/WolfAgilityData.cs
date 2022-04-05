using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class WolfAgilityData : UpgradeData
{
    [Title("������ ��ø�� ��ų ����")]
    [LabelText("�ɷ� ����")]
    [InfoBox("{a}�� �ߵ�Ȯ��, {b}�� Ÿ��Ƚ��")]
    [TextArea]
    [SerializeField]
    private string explain;
    public string GetExplain()
    {
        string temp = "";
        for(int i = 0; i < explain.Length; i++)
        {
            if(i + 2 < explain.Length)
                if (explain[i] == '{' && explain[i + 2] == '}')
                    if (explain[i + 1] == 'a')
                    {
                        temp += $"{actPer}%";
                        i += 2;
                        continue;
                    }
                    else if (explain[i + 1] == 'b')
                    {
                        temp += $"{attackNum}ȸ";
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

    [LabelText("Ÿ�� Ƚ��(max 100)")]
    [Range(0, 100)]
    public uint attackNum;
}
