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

        //아이템 이름 표시
        itemName.text = itemData.itemName;

        //아이템 능력치 표시
        string expalainText1 = "";
        Vector2Int attackMin = itemData.attackMin;
        AddText(ref expalainText1, "최소 공격력", ref attackMin);

        Vector2Int attackMax = itemData.attackMax;
        AddText(ref expalainText1, "최대 공격력", ref attackMax);

        Vector2Int criticalDamage = itemData.criticalDamage;
        AddText(ref expalainText1, "치명타 데미지", ref criticalDamage);

        Vector2Int criticalPer = itemData.criticalPer;
        AddText(ref expalainText1, "치명타 확률", ref criticalPer);
        itemExplain1.text = expalainText1;

        //아이템 이미지 표시
        Sprite sprite = itemData.itemSprite;
        imgIcon.sprite = sprite;

        //아이템 설명 표시
        itemExplain2.text = itemData.itemExplain;

        body.SetActive(true);

        //필요한 아이템 표시
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
    /// : 튜토리얼 아이템 선택 Box 활성/비활성
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnOffTutorialRequireBox(bool state)
    {
        if (slotList.Count >= 1)
        {
            slotList[0].OnOffTutorialRequireBox(state);
        }
    }
}
