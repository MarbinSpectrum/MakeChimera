using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialEquipment : MonoBehaviour
{
    [SerializeField] private GameObject TutorialSort;
    [SerializeField] private GameObject NormalSort;

    [Header("----------")]
    [SerializeField] private ScrollRect ScrollSlots;
    [SerializeField] private GameObject Tutorial_SelectSlot;
    [SerializeField] private GameObject Tutorial_SelectSlotFlag;

    [Header("----------")]
    [SerializeField] private Transform Equipment_ShowItem;
    [SerializeField] private GameObject Tutorial_PutOn;
    [SerializeField] private GameObject Tutorial_PutOnFlag;
    [SerializeField] private GameObject Tutorial_PutOnCheckBox;

    [Header("----------")]
    [SerializeField] private Transform Equipment_PlayerData;
    [SerializeField] private GameObject Tutorial_PlayerData;
    [SerializeField] private GameObject Tutorial_PlayerDataFlag;
    [SerializeField] private GameObject Tutorial_PlayerDataCheckBox;
    [SerializeField] private GameObject Tutorial_PlayerDataImgCheckBox;

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
            EquipmentUI.OnOffTutorialSlotBox(true);
            ScrollSlots.enabled = false;
            ScrollSlots.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            yield return new WaitWhile(() => { return !nextTutorai; });

            //UI�ڷ� ǥ��
            TutorialSort.SetActive(false);
            NormalSort.SetActive(true);
            EquipmentUI.OnOffTutorialSlotBox(false);
            Tutorial_SelectSlot.SetActive(false);
            ScrollSlots.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            ScrollSlots.enabled = true;

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //������ ����
            nextTutorai = false;

            //UI�ڷ� ǥ��
            Tutorial_PutOn.SetActive(true);
            Equipment_ShowItem.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            yield return new WaitWhile(() => { return !Tutorial_PutOnFlag.activeSelf; });

            //UI ������ ǥ��
            Tutorial_PutOnCheckBox.SetActive(true);
            Equipment_ShowItem.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            yield return new WaitWhile(() => { return !nextTutorai; });

            //UI�ڷ� ǥ��
            Tutorial_PutOnCheckBox.SetActive(false);
            Tutorial_PutOn.SetActive(false);
            Equipment_ShowItem.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //�÷��̾� ���� Ȯ��
            nextTutorai = false;

            //UI Ȱ��
            Equipment_PlayerData.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            Tutorial_PlayerData.SetActive(true);
            yield return new WaitWhile(() => { return !Tutorial_PlayerDataFlag.activeSelf; });

            //����Ʈǥ��
            Tutorial_PlayerDataCheckBox.SetActive(true);
            Tutorial_PlayerDataImgCheckBox.SetActive(true);
            yield return new WaitWhile(() => { return !nextTutorai; });

            //UI ��Ȱ��
            Equipment_PlayerData.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            Equipment_ShowItem.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            Tutorial_PlayerData.SetActive(false);
            Tutorial_PlayerDataCheckBox.SetActive(false);
            Tutorial_PlayerDataImgCheckBox.SetActive(false);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //������
            nextTutorai = false;
            closeWindow = true;

            //UI ������ ǥ��
            Tutorial_CloseBtn.SetActive(true);
            Tutorial_CloseBtnCheckBox.SetActive(true);
            CloseBtn.SetSiblingIndex(transform.GetSiblingIndex() + 1);
        }
    }

    public void NextTutorial()
    {
        if (Tutorial_SelectSlot.activeSelf)
        {
            nextTutorai = true;
        }
        else if (Tutorial_PutOn.activeSelf)
        {
            nextTutorai = true;
        }
        else if (Tutorial_PlayerData.activeSelf)
        {
            nextTutorai = true;
        }
        else if (closeWindow)
        {
            CloseBtn.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            Equipment_ShowItem.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            closeWindow = false;
            Tutorial_CloseBtn.SetActive(false);
            Tutorial_CloseBtnCheckBox.SetActive(false);
            TutorialMain.EquipmentTutorialEnd();
        }
    }
}
