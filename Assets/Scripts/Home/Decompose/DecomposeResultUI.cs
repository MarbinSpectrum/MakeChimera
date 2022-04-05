using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecomposeResultUI : MonoBehaviour
{
    [SerializeField] private GameObject body;
    [SerializeField] private Transform content;
    [SerializeField] private DecomposeResultSlot slotObj;
    private List<DecomposeResultSlot> slotList = new List<DecomposeResultSlot>();

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 분해 결과 정보 표시
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetData(List<ItemClass> itemList)
    {
        //필요한 추가 슬롯 갯수를 상정
        int createNum = itemList.Count - slotList.Count;

        for (int i = 0; i < createNum; i++)
        {
            //슬롯을 추가
            DecomposeResultSlot obj = Instantiate(slotObj);
            obj.transform.parent = content;
            obj.transform.localScale = new Vector3(1, 1, 1);
            slotList.Add(obj);
        }
        for (int i = 0; i < slotList.Count; i++)
        {
            //슬롯에 아이템 정보를 표시
            if (i >= itemList.Count)
                slotList[i].gameObject.SetActive(false);
            else

            {
                ItemClass itemData = itemList[i];
                string itemDataName = itemData.itemDataName;
                int itemNum = itemData.itemNum;
                slotList[i].SetData(itemDataName, itemNum);
                slotList[i].gameObject.SetActive(true);
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 윈도우를 ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnOffWindow(bool state)
    {
        body.SetActive(state);
    }
}
