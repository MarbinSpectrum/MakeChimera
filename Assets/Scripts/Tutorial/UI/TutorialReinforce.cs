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
            Tutorial_SelectSlot.SetActive(true);
            yield return new WaitWhile(() => { return !Tutorial_SelectSlotFlag.activeSelf; });

            //UI ������ ǥ��
            Tutorial_SelectSlotCheckBox.SetActive(true);
            ScrollSlots.enabled = false;
            yield return new WaitWhile(() => { return !nextTutorai; });

            //UI �ڷ� ǥ��
            Tutorial_SelectSlotCheckBox.SetActive(false);
            Tutorial_SelectSlot.SetActive(false);
            ScrollSlots.enabled = true;

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
