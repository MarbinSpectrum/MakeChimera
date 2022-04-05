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
                //NULL값인 경우 데이터를 매니저를 가져온다.
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
    /// : 씬이동
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void LoadScene(string sceneName)
    {
        Instance.loadAni.Play();
        Instance.sceneName = sceneName;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 애니메이션 종료 후에 이동처리
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void MoveStart()
    {
        SceneManager.LoadScene(sceneName);
    }
}
