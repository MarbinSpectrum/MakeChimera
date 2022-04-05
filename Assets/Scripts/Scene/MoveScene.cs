using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    private static MoveScene m_instance;
    public static MoveScene Instance
    {
        get
        {
            if (m_instance == null)
            {
                //NULL���� ��� �����͸� �Ŵ����� �����´�.
                GameObject managerObj = Instantiate(Resources.Load("System/MoveScene/MoveScene") as GameObject);
                m_instance = managerObj.GetComponent<MoveScene>();
                m_instance.name = "MoveScene";

                DontDestroyOnLoad(m_instance.gameObject);
            }
            return m_instance;
        }
    }

    [SerializeField] private PlayableDirector loadAni;
    private string sceneName;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���̵�
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void LoadScene(string sceneName)
    {
        Instance.loadAni.Play();
        Instance.sceneName = sceneName;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �ִϸ��̼� ���� �Ŀ� �̵�ó��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void MoveStart()
    {
        SceneManager.LoadScene(sceneName);
    }
}
