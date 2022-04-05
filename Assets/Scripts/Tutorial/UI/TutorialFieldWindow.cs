using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFieldWindow : MonoBehaviour
{
    [SerializeField] private GameObject Tutorial_FieldWindow;
    [SerializeField] private GameObject Tutorial_FieldWindowFlag;
    [SerializeField] private GameObject Tutorial_CheckBox0;
    [SerializeField] private GameObject Tutorial_CheckBox1;

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
            Tutorial_FieldWindow.SetActive(true);
            yield return new WaitWhile(() => { return !Tutorial_FieldWindowFlag.activeSelf; });

            //선택 표시
            Tutorial_CheckBox0.SetActive(true);
            Tutorial_CheckBox1.SetActive(true);
            yield return new WaitWhile(() => { return !nextTutorai; });

            //숨기기
            Tutorial_FieldWindow.SetActive(false);
            Tutorial_CheckBox0.SetActive(false);
            Tutorial_CheckBox1.SetActive(false);
        }
    }

    public void NextTutorial()
    {
        if (Tutorial_FieldWindow.activeSelf)
        {
            nextTutorai = true;
            TutorialMain.FieldTutorialEnd();
        }
    }
}
