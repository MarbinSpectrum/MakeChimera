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
            Tutorial_FieldWindow.SetActive(true);
            yield return new WaitWhile(() => { return !Tutorial_FieldWindowFlag.activeSelf; });

            //���� ǥ��
            Tutorial_CheckBox0.SetActive(true);
            Tutorial_CheckBox1.SetActive(true);
            yield return new WaitWhile(() => { return !nextTutorai; });

            //�����
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
