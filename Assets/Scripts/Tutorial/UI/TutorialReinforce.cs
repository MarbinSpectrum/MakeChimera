using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialReinforce : MonoBehaviour
{
    [SerializeField] private ScrollRect ScrollSlots;
    [SerializeField] private GameObject Tutorial_SelectSlot;
    [SerializeField] private GameObject Tutorial_SelectSlotFlag;
    [SerializeField] private GameObject Tutorial_SelectSlotCheckBox;

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
            Tutorial_SelectSlot.SetActive(true);
            yield return new WaitWhile(() => { return !Tutorial_SelectSlotFlag.activeSelf; });

            //UI 앞으로 표시
            Tutorial_SelectSlotCheckBox.SetActive(true);
            ScrollSlots.enabled = false;
            yield return new WaitWhile(() => { return !nextTutorai; });

            //UI 뒤로 표시
            Tutorial_SelectSlotCheckBox.SetActive(false);
            Tutorial_SelectSlot.SetActive(false);
            ScrollSlots.enabled = true;

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //나가기
            nextTutorai = false;
            closeWindow = true;

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
        else if (closeWindow)
        {
            closeWindow = false;

            Tutorial_CloseBtn.SetActive(false);
            Tutorial_CloseBtnCheckBox.SetActive(false);
            CloseBtn.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            TutorialMain.ReinforceTutorialEnd();
        }
    }
}
