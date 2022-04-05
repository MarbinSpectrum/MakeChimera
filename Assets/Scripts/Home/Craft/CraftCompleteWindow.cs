using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class CraftCompleteWindow : SerializedMonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Dictionary<Upgrade, CraftCompleteGauge> itemGauge = new Dictionary<Upgrade, CraftCompleteGauge>();
    [SerializeField] private GameObject body;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private EventSE_IsKey endSE;

    private EquipmentData nowItemData;
    private ItemClass nowItemClass;
    private Dictionary<Upgrade,IEnumerator> nowCor = new Dictionary<Upgrade, IEnumerator>();
    private bool createComplete = false;
    private int upgradeCnt = 0;
    public void OnOffWindow(bool state)
    {
        if (state == false)
        {
            foreach (Upgrade up in Enum.GetValues(typeof(Upgrade)))
                if (nowCor.ContainsKey(up) && nowCor[up] != null)
                    StopCoroutine(nowCor[up]);
            createComplete = true;
        }
        body.SetActive(state);
    }
    public void SetItem(EquipmentData itemData, ItemClass itemClass)
    {
        upgradeCnt = 0;

        createComplete = false;

        OnOffWindow(true);

        nowItemData = itemData;
        nowItemClass = itemClass;

        itemIcon.sprite = nowItemData.itemSprite;
        itemNameText.text = nowItemData.itemName;

        ActItemGauge(nowItemData.attackMin, Upgrade.MinDamage);
        ActItemGauge(nowItemData.attackMax, Upgrade.MaxDamage);
        ActItemGauge(nowItemData.criticalDamage, Upgrade.CriticalDamage);
        ActItemGauge(nowItemData.criticalPer, Upgrade.CriticalPer);

        SetItemGauge(itemClass.attackMin, nowItemData.attackMin.y, Upgrade.MinDamage);
        SetItemGauge(itemClass.attackMax, nowItemData.attackMax.y, Upgrade.MaxDamage);
        SetItemGauge(itemClass.criticalDamage, nowItemData.criticalDamage.y, Upgrade.CriticalDamage);
        SetItemGauge(itemClass.criticalPer, nowItemData.criticalPer.y, Upgrade.CriticalPer);
    }

    private void ActItemGauge(Vector2 data, Upgrade upgrade)
    {
        bool act = (data.x == data.y && data.x == 0);
        itemGauge[upgrade].gameObject.SetActive(!act);
    }

    private void SetItemGauge(float now, float max, Upgrade upgrade)
    {
        float per = now / max;
        nowCor[upgrade] = Cor_SetItemGauge(per, max, upgrade);
        StartCoroutine(nowCor[upgrade]);

        string MakeString(float v, Upgrade upgrade)
        {
            if (upgrade == Upgrade.MaxDamage || upgrade == Upgrade.MinDamage)
                v = (int)v;
            string str = ((int)v).ToString();
            if (upgrade == Upgrade.CriticalPer || upgrade == Upgrade.CriticalDamage)
                str += "%";
            return str;
        }

        IEnumerator Cor_SetItemGauge(float per, float value, Upgrade upgrade)
        {
            float nowPer = 0;
            while (nowPer < per)
            {
                itemGauge[upgrade].gauge = nowPer;
                string nowStr = MakeString(nowPer * value, upgrade);
                itemGauge[upgrade].SetText(nowStr);
                nowPer += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            string str = MakeString(per * value, upgrade);
            itemGauge[upgrade].SetText(str);
            itemGauge[upgrade].gauge = per;
            if(value != 0)
                endSE.RunEvent();
            upgradeCnt++;
            if (upgradeCnt >= 4)
                createComplete = true;
        }
    }

    public bool IsGetComplete()
    {
        return createComplete;
    }
}
