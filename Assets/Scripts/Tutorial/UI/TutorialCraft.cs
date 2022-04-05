using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCraft : MonoBehaviour
{
    [SerializeField] private GameObject TutorialSort;
    [SerializeField] private GameObject NormalSort;

    [Header("----------")]
    [SerializeField] private ScrollRect ScrollSlots;
    [SerializeField] private GameObject Tutorial_SelectSlot;
    [SerializeField] private GameObject Tutorial_SelectSlotFlag;

    [Header("----------")]
    [SerializeField] private Transform ItemData;
    [SerializeField] private Transform RequireItems;
    [SerializeField] private GameObject Tutorial_RequireItem;
    [SerializeField] private GameObject Tutorial_RequireItemFlag;

    [Header("----------")]
    [SerializeField] private Transform Craft_Run;
    [SerializeField] private GameObject Tutorial_CraftRun;
    [SerializeField] private GameObject Tutorial_CraftRunFlag;
    [SerializeField] private GameObject Tutorial_CraftBtnCheckBox;

    [Header("----------")]
    [SerializeField] private CraftCompleteWindow CraftCompleteWindow;
    [SerializeField] private GameObject CraftCompleteDonClick;
    private bool craftWindow = false;

    [Header("----------")]
    [SerializeField] private Transform CloseBtn;
    [SerializeField] private GameObject Tutorial_CloseBtn;
    [SerializeField] private GameObject Tutorial_CloseBtnCheckBox;
    private bool closeWindow = false;

    private bool nextTutorai;

    private void Start()
    {
        PlayData playData = DataManager.Instance.playData;

        if (playData.firstPlay)
        {
            //ó�� �����ϸ� Ʃ�丮�� ����
            StartCoroutine(StartTutorial());
        }
        else
        {
            //�ƴϸ� �������
            gameObject.SetActive(false);
        }

        IEnumerator StartTutorial()
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //���� Ŭ��
            nextTutorai = false;

            //UI�ڷ� ǥ��
            TutorialSort.SetActive(true);
            NormalSort.SetActive(false);
            Tutorial_SelectSlot.SetActive(true);
            ScrollSlots.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            yield return new WaitWhile(() => { return !Tutorial_SelectSlotFlag.activeSelf; });

            //UI ������ ǥ��
            Craft.OnOffTutorialSlotBox(true);
            ScrollSlots.enabled = false;
            ScrollSlots.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            yield return new WaitWhile(() => { return !nextTutorai; });

            //UI �ڷ� ǥ��
            TutorialSort.SetActive(false);
            NormalSort.SetActive(true);
            Craft.OnOffTutorialSlotBox(false);
            Tutorial_SelectSlot.SetActive(false);
            ScrollSlots.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            ScrollSlots.enabled = true;

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //�ʿ��� ��� Ȯ��
            nextTutorai = false;
       
            //UI�ڷ� ǥ��
            Tutorial_RequireItem.SetActive(true);
            ItemData.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            RequireItems.SetSiblingIndex(Tutorial_RequireItem.transform.GetSiblingIndex() - 1);
            yield return new WaitWhile(() => { return !Tutorial_RequireItemFlag.activeSelf; });

            //UI������ ǥ��
            Craft.OnOffTutorialRequireBox(true);
            RequireItems.SetSiblingIndex(Tutorial_RequireItem.transform.GetSiblingIndex() + 1);
            yield return new WaitWhile(() => { return !nextTutorai; });

            //UI�ڷ� ǥ��
            Craft.OnOffTutorialRequireBox(false);
            Tutorial_RequireItem.SetActive(false);
            ItemData.SetSiblingIndex(transform.GetSiblingIndex() - 1);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //�����ϱ� ��ư
            nextTutorai = false;

            //UI�ڷ� ǥ��
            Tutorial_CraftRun.SetActive(true);
            Craft_Run.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            yield return new WaitWhile(() => { return !Tutorial_CraftRunFlag.activeSelf; });

            //UI ������ ǥ��
            Tutorial_CraftBtnCheckBox.SetActive(true);
            Craft_Run.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            yield return new WaitWhile(() => { return !nextTutorai; });

            //UI�ڷ� ǥ��
            Tutorial_CraftBtnCheckBox.SetActive(false);
            Tutorial_CraftRun.SetActive(false);
            Craft_Run.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //���ۿ���
            nextTutorai = false;
            craftWindow = true;

            //Ŭ������(��ŵ�� ��������)
            CraftCompleteDonClick.SetActive(true);
            CraftCompleteWindow.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);

            //������ �Ϸ�ɶ����� ���
            yield return new WaitWhile(() => { return !CraftCompleteWindow.IsGetComplete(); });

            //Ŭ������ ����
            CraftCompleteDonClick.SetActive(false);

            yield return new WaitWhile(() => { return !nextTutorai; });
            CraftCompleteWindow.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //������
            nextTutorai = false;
            closeWindow = true;

            Tutorial_CloseBtn.SetActive(true);
            Tutorial_CloseBtnCheckBox.SetActive(true);
            CloseBtn.SetSiblingIndex(transform.GetSiblingIndex() + 1);
        }
    }

    public void NextTutorial()
    {
        if(Tutorial_SelectSlot.activeSelf)
        {
            nextTutorai = true;
        }
        else if (Tutorial_RequireItem.activeSelf)
        {
            nextTutorai = true;
        }
        else if (Tutorial_CraftRun.activeSelf)
        {
            nextTutorai = true;
        }
        else if(craftWindow)
        {
            craftWindow = false;
            nextTutorai = true;
        }
        else if(closeWindow)
        {
            CloseBtn.SetSiblingIndex(CraftCompleteWindow.transform.GetSiblingIndex() - 1);
            closeWindow = false;
            Tutorial_CloseBtn.SetActive(false);
            Tutorial_CloseBtnCheckBox.SetActive(false);
            TutorialMain.CraftTutorialEnd();
        }
    }


}
