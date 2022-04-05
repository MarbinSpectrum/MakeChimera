using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialField : MonoBehaviour
{
    [SerializeField] private ScrollRect ScrollSlots;
    [SerializeField] private GameObject Tutorial_SelectField;
    [SerializeField] private GameObject Tutorial_SelectFieldFlag;
    [SerializeField] private GameObject Tutorial_SelectFieldCheckBox;

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

            //튜토리얼 표시
            Tutorial_SelectField.SetActive(true);
            yield return new WaitWhile(() => { return !Tutorial_SelectFieldFlag.activeSelf; });

            //선택 표시
            Tutorial_SelectFieldCheckBox.SetActive(true);
            ScrollSlots.enabled = false;
            yield return new WaitWhile(() => { return !nextTutorai; });

            //숨기기
            Tutorial_SelectFieldCheckBox.SetActive(false);
            Tutorial_SelectField.SetActive(false);
            ScrollSlots.enabled = true;
        }

    }

    public void NextTutorial()
    {
        if (Tutorial_SelectField.activeSelf)
        {
            nextTutorai = true;
        }
    }
}
