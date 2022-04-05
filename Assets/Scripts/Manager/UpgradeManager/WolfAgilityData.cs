using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class WolfAgilityData : UpgradeData
{
    [Title("´Á´ëÀÇ ¹ÎÃ¸¼º ½ºÅ³ Á¤º¸")]
    [LabelText("´É·Â ¼³¸í")]
    [InfoBox("{a}´Â ¹ßµ¿È®·ü, {b}´Â Å¸°ÝÈ½¼ö")]
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
                        temp += $"{attackNum}È¸";
                        i += 2;
                        continue;
                    }
            temp += explain[i];
        }
        return temp;
    }

    [LabelText("¹ßµ¿ È®·ü(%)")]
    [Range(0, 100)]
    public float actPer;

    [LabelText("Å¸°Ý È½¼ö(max 100)")]
    [Range(0, 100)]
    public uint attackNum;
}
