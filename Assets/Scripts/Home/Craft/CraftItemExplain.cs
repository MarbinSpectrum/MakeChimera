using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftItemExplain : MonoBehaviour
{
    private EquipmentData nowItemData;
    [SerializeField] private GameObject body;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemExplain1;
    [SerializeField] private TextMeshProUGUI itemExplain2;
    [SerializeField] private Image imgIcon;
    [SerializeField] private RequireSlot slotObj;
    [SerializeField] private Transform slotTrans;

    private List<RequireSlot> slotList = new List<RequireSlot>();

    public void Init()
    {
        body.SetActive(false);
    }

    public void AddText(ref string a, string b,ref Vector2Int vector2Int)
    {
        if (vector2Int.x > vector2Int.y)
            vector2Int.x = vector2Int.y;
        if (vector2Int.x == vector2Int.y && vector2Int.x == 0)
            return;
        if(vector2Int.x == vector2Int.y)
            a += $"{b} : {vector2Int.x} \n";
        else
            a += $"{b} : {vector2Int.x} ~ {vector2Int.y} \n";
    }

    public void SetItem(EquipmentData itemData)
    {
        nowItemData = itemData;

        //������ �̸� ǥ��
        itemName.text = itemData.itemName;

        //������ �ɷ�ġ ǥ��
        string expalainText1 = "";
        Vector2Int attackMin = itemData.attackMin;
        AddText(ref expalainText1, "�ּ� ���ݷ�", ref attackMin);

        Vector2Int attackMax = itemData.attackMax;
        AddText(ref expalainText1, "�ִ� ���ݷ�", ref attackMax);

        Vector2Int criticalDamage = itemData.criticalDamage;
        AddText(ref expalainText1, "ġ��Ÿ ������", ref criticalDamage);

        Vector2Int criticalPer = itemData.criticalPer;
        AddText(ref expalainText1, "ġ��Ÿ Ȯ��", ref criticalPer);
        itemExplain1.text = expalainText1;

        //������ �̹��� ǥ��
        Sprite sprite = itemData.itemSprite;
        imgIcon.sprite = sprite;

        //������ ���� ǥ��
        itemExplain2.text = itemData.itemExplain;

        body.SetActive(true);

        //�ʿ��� ������ ǥ��
        ShowRequireSlot(itemData);
    }

    private void ShowRequireSlot(EquipmentData itemData)
    {
        int createNum = itemData.requireItem.Count - slotList.Count;

        for (int i = 0; i < createNum; i++)
        {
            RequireSlot obj = Instantiate(slotObj);
            obj.transform.parent = slotTrans;
            obj.transform.localScale = new Vector3(1, 1, 1);
            slotList.Add(obj);
        }
        for (int i = 0; i < slotList.Count; i++)
        {
            if (i >= itemData.requireItem.Count)
                slotList[i].gameObject.SetActive(false);
            else
            {
                slotList[i].SetItemData(itemData.requireItem[i].item, itemData.requireItem[i].itemNum);
                slotList[i].gameObject.SetActive(true);
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : Ʃ�丮�� ������ ���� Box Ȱ��/��Ȱ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnOffTutorialRequireBox(bool state)
    {
        if (slotList.Count >= 1)
        {
            slotList[0].OnOffTutorialRequireBox(state);
        }
    }
}
