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

            //Ʃ�丮�� ǥ��
            Tutorial_SelectField.SetActive(true);
            yield return new WaitWhile(() => { return !Tutorial_SelectFieldFlag.activeSelf; });

            //���� ǥ��
            Tutorial_SelectFieldCheckBox.SetActive(true);
            ScrollSlots.enabled = false;
            yield return new WaitWhile(() => { return !nextTutorai; });

            //�����
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
