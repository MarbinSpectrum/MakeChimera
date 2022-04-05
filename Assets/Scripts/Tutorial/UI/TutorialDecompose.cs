using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDecompose : MonoBehaviour
{
    [SerializeField] private GameObject TutorialSort;
    [SerializeField] private GameObject NormalSort;

    [Header("----------")]
    [SerializeField] private ScrollRect ScrollSlots;
    [SerializeField] private GameObject Tutorial_SelectSlot;
    [SerializeField] private GameObject Tutorial_SelectSlotFlag;

    [Header("----------")]
    [SerializeField] private Transform Decompose_Run;
    [SerializeField] private GameObject Tutorial_DecomposeRun;
    [SerializeField] private GameObject Tutorial_DecomposeRunFlag;
    [SerializeField] private GameObject Tutorial_DecomposeBtnCheckBox;

    [Header("----------")]
    [SerializeField] private Transform Decompose_Check;
    [SerializeField] private GameObject Tutorial_DecomposeCheck;
    [SerializeField] private GameObject Tutorial_DecomposeCheckBtnCheckBox;

    [Header("----------")]
    [SerializeField] private Transform Decompose_Result;
    [SerializeField] private GameObject Tutorial_DecomposeResult;
    private bool decomposeResult = false;

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
            DecomposeUI.OnOffTutorialSlotBox(true);
            ScrollSlots.enabled = false;
            ScrollSlots.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            yield return new WaitWhile(() => { return !nextTutorai; });

            //UI뒤로 표시
            TutorialSort.SetActive(false);
            NormalSort.SetActive(true);
            DecomposeUI.OnOffTutorialSlotBox(false);
            Tutorial_SelectSlot.SetActive(false);
            ScrollSlots.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            ScrollSlots.enabled = true;

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //분해하기 버튼
            nextTutorai = false;

            //UI뒤로 표시
            Tutorial_DecomposeRun.SetActive(true);
            Decompose_Run.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            yield return new WaitWhile(() => { return !Tutorial_DecomposeRunFlag.activeSelf; });

            //UI 앞으로 표시
            Tutorial_DecomposeBtnCheckBox.SetActive(true);
            Decompose_Run.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            yield return new WaitWhile(() => { return !nextTutorai; });

            //UI뒤로 표시
            Tutorial_DecomposeBtnCheckBox.SetActive(false);
            Tutorial_DecomposeRun.SetActive(false);
            Decompose_Run.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //분해확인 버튼
            nextTutorai = false;

            //UI 앞으로 표시
            Tutorial_DecomposeCheckBtnCheckBox.SetActive(true);
            Tutorial_DecomposeCheck.SetActive(true);
            Decompose_Check.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            yield return new WaitWhile(() => { return !nextTutorai; });

            //UI뒤로 표시
            Tutorial_DecomposeCheckBtnCheckBox.SetActive(false);
            Tutorial_DecomposeCheck.SetActive(false);
            Decompose_Check.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //분해 결과
            nextTutorai = false;
            decomposeResult = true;

            //UI 앞으로 표시
            Tutorial_DecomposeResult.SetActive(true);
            Decompose_Result.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            yield return new WaitWhile(() => { return !nextTutorai; });

            //UI뒤로 표시
            Tutorial_DecomposeResult.SetActive(false);
            Decompose_Result.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);

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
        else if (Tutorial_DecomposeRun.activeSelf)
        {
            nextTutorai = true;
        }
        else if (Tutorial_DecomposeCheck.activeSelf)
        {
            nextTutorai = true;
        }
        else if (decomposeResult)
        {
            decomposeResult = false;
            nextTutorai = true;
        }
        else if (closeWindow)
        {
            CloseBtn.SetSiblingIndex(Decompose_Check.GetSiblingIndex() - 1);
            closeWindow = false;
            Tutorial_CloseBtn.SetActive(false);
            Tutorial_CloseBtnCheckBox.SetActive(false);
            TutorialMain.DecomposeTutorialEnd();
        }
    }
}
