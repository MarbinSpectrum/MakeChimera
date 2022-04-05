using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeRequireList : MonoBehaviour
{
    [SerializeField] private GameObject body;
    [SerializeField] private RequireSlot slotObj;
    [SerializeField] private Transform slotTrans;

    private List<RequireSlot> slotList = new List<RequireSlot>();

    public void OffRequireList()
    {
        body.SetActive(false);
    }

    public void ShowRequireList(Upgrade upgrade, uint level)
    {
        body.SetActive(true);

        List<RequireItemClass> requireItems = UpgradeManager.GetUpgradeRequireList(upgrade, level);

        int createNum = requireItems.Count - slotList.Count;

        for (int i = 0; i < createNum; i++)
        {
            RequireSlot obj = Instantiate(slotObj);
            obj.transform.parent = slotTrans;
            obj.transform.localScale = new Vector3(1, 1, 1);
            slotList.Add(obj);
        }
        for (int i = 0; i < slotList.Count; i++)
        {
            if (i >= requireItems.Count)
                slotList[i].gameObject.SetActive(false);
            else
            {
                slotList[i].SetItemData(requireItems[i].item, requireItems[i].itemNum);
                slotList[i].gameObject.SetActive(true);
            }
        }
    }
}
