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
    /// : ���� ��� ���� ǥ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetData(List<ItemClass> itemList)
    {
        //�ʿ��� �߰� ���� ������ ����
        int createNum = itemList.Count - slotList.Count;

        for (int i = 0; i < createNum; i++)
        {
            //������ �߰�
            DecomposeResultSlot obj = Instantiate(slotObj);
            obj.transform.parent = content;
            obj.transform.localScale = new Vector3(1, 1, 1);
            slotList.Add(obj);
        }
        for (int i = 0; i < slotList.Count; i++)
        {
            //���Կ� ������ ������ ǥ��
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
    /// : �����츦 ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnOffWindow(bool state)
    {
        body.SetActive(state);
    }
}
