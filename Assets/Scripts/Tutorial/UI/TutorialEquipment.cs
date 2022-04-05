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
            //처음 실행하면 튜토리얼 시작
            StartCoroutine(StartTutorial());
        }
        else
        {
            //아니면 실행안함
            gameObject.SetActive(false);
        }

        IEnumerator StartTutorial()
        {

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //슬롯 클릭
            nextTutorai = false;

            //UI뒤로 표시
            TutorialSort.SetActive(true);
            NormalSort.SetActive(false);
            Tutorial_SelectSlot.SetActive(true);
            ScrollSlots.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            yield return new WaitWhile(() => { return !Tutorial_SelectSlotFlag.activeSelf; });

            //UI 앞으로 표시
            EquipmentUI.OnOffTutorialSlotBox(true);
            ScrollSlots.enabled = false;
            ScrollSlots.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            yield return new WaitWhile(() => { return !nextTutorai; });

            //UI뒤로 표시
            TutorialSort.SetActive(false);
            NormalSort.SetActive(true);
            EquipmentUI.OnOffTutorialSlotBox(false);
            Tutorial_SelectSlot.SetActive(false);
            ScrollSlots.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            ScrollSlots.enabled = true;

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //아이템 장착
            nextTutorai = false;

            //UI뒤로 표시
            Tutorial_PutOn.SetActive(true);
            Equipment_ShowItem.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            yield return new WaitWhile(() => { return !Tutorial_PutOnFlag.activeSelf; });

            //UI 앞으로 표시
            Tutorial_PutOnCheckBox.SetActive(true);
            Equipment_ShowItem.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            yield return new WaitWhile(() => { return !nextTutorai; });

            //UI뒤로 표시
            Tutorial_PutOnCheckBox.SetActive(false);
            Tutorial_PutOn.SetActive(false);
            Equipment_ShowItem.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //플레이어 정보 확인
            nextTutorai = false;

            //UI 활성
            Equipment_PlayerData.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            Tutorial_PlayerData.SetActive(true);
            yield return new WaitWhile(() => { return !Tutorial_PlayerDataFlag.activeSelf; });

            //이펙트표시
            Tutorial_PlayerDataCheckBox.SetActive(true);
            Tutorial_PlayerDataImgCheckBox.SetActive(true);
            yield return new WaitWhile(() => { return !nextTutorai; });

            //UI 비활성
            Equipment_PlayerData.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            Equipment_ShowItem.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            Tutorial_PlayerData.SetActive(false);
            Tutorial_PlayerDataCheckBox.SetActive(false);
            Tutorial_PlayerDataImgCheckBox.SetActive(false);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //나가기
            nextTutorai = false;
            closeWindow = true;

            //UI 앞으로 표시
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
