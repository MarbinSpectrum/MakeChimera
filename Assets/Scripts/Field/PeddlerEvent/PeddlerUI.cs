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

        //������� �Ǹ��� ������ ����Ʈ�� �������ش�.
        List<string> itemList = PeddlerManager.GetItemList(slotNum);
        for(int i = 0; i < slotNum; i++)
        {
            //������ ���Կ� �������� �������ش�.
            Instance.PeddlerItemSlot[i].OnOffSlot(i < itemList.Count);
            if (i < itemList.Count)
            {
                Instance.PeddlerItemSlot[i].SetItem(itemList[i]);
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ����ΰ� ��ȭ ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void TalktoStart()
    {
        Instance.talkBtn.SetActive(false);
        Instance.windowObj.SetActive(true);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ����ΰ� ��ȭ ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void TalktoEnd()
    {
        OnOffWindow(false);
        FieldSystem.TalktoPeddlerEnd();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �����츦 ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffWindow(bool state)
    {
        Instance.body.SetActive(state);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : EquipmentShowItem�� ���� ������ ������ ǥ�����ش�.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void ShowItem(ItemClass itemClass)
    {
        Instance.equipmentShow.ShowItem(itemClass);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ��� ���� �޽��� Ȱ��/��Ȱ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffNoResource(bool state)
    {
        Instance.noResource.SetActive(state);
    }
}
