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
    [LabelText("������ �̸�")]
    public string itemName;

    [LabelText("������ ��� ����")]
    public Vector2Int dropNum;

    [LabelText("��� Ȯ��(%)")]
    [Range(0, 100)]
    public float dropPer;
}

public class MonsterData : SerializedMonoBehaviour
{
    [LabelText("���� ���")]
    public MonsterClass monsterClass;

    [LabelText("���� �̸�")]
    [VerticalGroup("monster")]
    public string monsterName;

    [LabelText("���� ������")]
    public Sprite monsterIcon;

    [LabelText("���� ����")]
    [VerticalGroup("monster")]
    public string monsterType;

    [LabelText("���� ü��")]
    public int monsterHp;

    //[HideIf("monsterClass", MonsterClass.Boss)]
    [LabelText("���� Ȯ��")]
    public int spawnPer;

    [Space(50)]

    [LabelText("���� ������ ��� �� Ȯ��")]
    [InfoBox("�ƹ��͵� ������ ĭ�� �ƹ��͵� ����ȵǴ� Ȯ����")]
    public List<DropItemData> dropitemData = new List<DropItemData>();

    [Space(50)]

    [ShowIf("monsterClass", MonsterClass.Boss)]
    [LabelText("���� ��ȯ ����")]
    [InfoBox("<���� ���, ��ƾ��ϴ� ��>")]
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
        //�ش� ���Ϳ��� ���� �� �ִ� ������ ���
        List<ItemClass> getItem = GetItem();

        //�κ��丮�� �������� �߰�
        foreach(ItemClass dropItemData in getItem)
            Inventory.AddItem(dropItemData.itemDataName, dropItemData.itemNum);

        //������ ������ȿ�� ���
        List<string> items = new List<string>();
        foreach (ItemClass dropItemData in getItem)
            items.Add(dropItemData.itemDataName);

        //���� ��ġ�� �������� ������ ����Ʈ ���
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
