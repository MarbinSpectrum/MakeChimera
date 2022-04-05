using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeddlerUI : MonoBehaviour
{
    public static PeddlerUI Instance;
    [SerializeField] private List<PeddlerItemSlot> PeddlerItemSlot;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject talkBtn;
    [SerializeField] private GameObject windowObj;
    [SerializeField] private EquipmentShowItem equipmentShow;
    [SerializeField] private GameObject noResource;
    private void Awake()
    {
        Instance = this;
    }

    public static void Init()
    {
        Instance.body.SetActive(true);
        Instance.talkBtn.SetActive(true);
        Instance.windowObj.SetActive(false);

        int slotNum = Instance.PeddlerItemSlot.Count;

        //행상인이 판매할 아이템 리스트를 생성해준다.
        List<string> itemList = PeddlerManager.GetItemList(slotNum);
        for(int i = 0; i < slotNum; i++)
        {
            //아이템 슬롯에 아이템을 설정해준다.
            Instance.PeddlerItemSlot[i].OnOffSlot(i < itemList.Count);
            if (i < itemList.Count)
            {
                Instance.PeddlerItemSlot[i].SetItem(itemList[i]);
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 행상인과 대화 시작
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void TalktoStart()
    {
        Instance.talkBtn.SetActive(false);
        Instance.windowObj.SetActive(true);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 행상인과 대화 시작
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void TalktoEnd()
    {
        OnOffWindow(false);
        FieldSystem.TalktoPeddlerEnd();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 윈도우를 ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffWindow(bool state)
    {
        Instance.body.SetActive(state);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : EquipmentShowItem를 통해 아이템 정보를 표시해준다.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void ShowItem(ItemClass itemClass)
    {
        Instance.equipmentShow.ShowItem(itemClass);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 재료 부족 메시지 활성/비활성
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffNoResource(bool state)
    {
        Instance.noResource.SetActive(state);
    }
}
