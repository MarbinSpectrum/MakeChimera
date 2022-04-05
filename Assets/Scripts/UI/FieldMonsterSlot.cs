using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldMonsterSlot : MonoBehaviour
{
    public Image icon;

    public void SetIcon(string fieldName,string monsterName)
    {
        List<MonsterData> monsterList = MonsterManager.Instance.fieldMonsterData[fieldName];
        foreach(MonsterData monsterData in monsterList)
        {
            if(monsterData.monsterName == monsterName)
            {
                SetIcon(monsterData.monsterIcon);
                return;
            }
        }
    }

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }
}
