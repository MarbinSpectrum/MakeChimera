using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class UpgradeData : SerializedMonoBehaviour
{
    [Title("���׷��̵� ����")]
    [LabelText("���׷��̵� �̸�")]
    public string upgradeName;

    [LabelText("���׷��̵� ������")]
    public Sprite itemImg;

    [LabelText("���׷��̵� ����")]
    public Upgrade upgradeType;

    [LabelText("���׷��̵� ����")]
    public uint level;

    [LabelText("���׷��̵� �Ŀ�")]
    [SerializeField]
    private float power;

    [Title("�ʿ����")]
    [LabelText("�ʿ����/����")]
    public List<RequireItemClass> requireItem = new List<RequireItemClass>();

    public float GetPower()
    {
        return power;
    }
}
