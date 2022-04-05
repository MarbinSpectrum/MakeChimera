using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum MonsterClass
{
    Normal,
    Elite,
    Boss
}

[System.Serializable]
public class DropItemData
{
    [LabelText("아이템 이름")]
    public string itemName;

    [LabelText("아이템 드롭 갯수")]
    public Vector2Int dropNum;

    [LabelText("드랍 확률(%)")]
    [Range(0, 100)]
    public float dropPer;
}

public class MonsterData : SerializedMonoBehaviour
{
    [LabelText("몬스터 등급")]
    public MonsterClass monsterClass;

    [LabelText("몬스터 이름")]
    [VerticalGroup("monster")]
    public string monsterName;

    [LabelText("몬스터 아이콘")]
    public Sprite monsterIcon;

    [LabelText("몬스터 종류")]
    [VerticalGroup("monster")]
    public string monsterType;

    [LabelText("몬스터 체력")]
    public int monsterHp;

    //[HideIf("monsterClass", MonsterClass.Boss)]
    [LabelText("스폰 확률")]
    public int spawnPer;

    [Space(50)]

    [LabelText("몬스터 아이템 드랍 및 확률")]
    [InfoBox("아무것도 안적은 칸은 아무것도 드랍안되는 확률임")]
    public List<DropItemData> dropitemData = new List<DropItemData>();

    [Space(50)]

    [ShowIf("monsterClass", MonsterClass.Boss)]
    [LabelText("몬스터 소환 조건")]
    [InfoBox("<몬스터 등급, 잡아야하는 수>")]
    public Dictionary<MonsterClass, int> spawnConditions = new Dictionary<MonsterClass, int>();

    
    [HideInInspector] public Animator animator;
    
    public List<ItemClass> GetItem()
    {
        List<ItemClass> itemList = new List<ItemClass>();
        foreach(DropItemData temp in dropitemData)
        {
            float r = Random.RandomRange(0, 100);
            if(r < temp.dropPer)
            {
                itemList.Add(new ItemClass(temp.itemName, ItemType.material, Random.Range(temp.dropNum.x, temp.dropNum.y)));
            }
        }

        return itemList;
    }

    public void DeathMonster()
    {
        DropItem();
        GameManager.KillAdd(monsterClass);
        GameManager.CheckMonsterList();
        SpawnUI.SubMonsterCnt(monsterName);
    }

    private void DropItem()
    {
        //해당 몬스터에게 나올 수 있는 아이템 경우
        List<ItemClass> getItem = GetItem();

        //인벤토리에 아이템을 추가
        foreach(ItemClass dropItemData in getItem)
            Inventory.AddItem(dropItemData.itemDataName, dropItemData.itemNum);

        //아이템 터지는효과 출력
        List<string> items = new List<string>();
        foreach (ItemClass dropItemData in getItem)
            items.Add(dropItemData.itemDataName);

        //몬스터 위치에 아이템이 나오는 이펙트 출력
        Vector3 monsterPos = FieldSystem.Instance.nowMonster.transform.position;
        BoomEffect.PlayEffect(items, monsterPos);
    }

    public void PlayAni(string ani)
    {
        if (animator == null)
            return;
        animator.SetTrigger(ani);
    }

    public bool BossSpawnCheck()
    {
        foreach (KeyValuePair<MonsterClass, int> temp in spawnConditions)
        {
            int monsterCnt = GameManager.GetKill_Count(temp.Key);
            int checkCnt = temp.Value;
            if (monsterCnt < checkCnt)
                return false;
        }
        return true;
    }
}
