using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class UpgradeData : SerializedMonoBehaviour
{
    [Title("업그레이드 정보")]
    [LabelText("업그레이드 이름")]
    public string upgradeName;

    [LabelText("업그레이드 아이콘")]
    public Sprite itemImg;

    [LabelText("업그레이드 종류")]
    public Upgrade upgradeType;

    [LabelText("업그레이드 레벨")]
    public uint level;

    [LabelText("업그레이드 파워")]
    [SerializeField]
    private float power;

    [Title("필요재료")]
    [LabelText("필요재료/갯수")]
    public List<RequireItemClass> requireItem = new List<RequireItemClass>();

    public float GetPower()
    {
        return power;
    }
}
