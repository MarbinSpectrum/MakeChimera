using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;

    [SerializeField] private Animation garbageMove;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayableDirector playerExit;
    [SerializeField] private TutorialTalkBox tutorial_playerMove;
    [SerializeField] private TutorialTalkBox tutorial_playerBattle;
    [SerializeField] private TutorialTalkBox tutorial_playerItemGet;
    [SerializeField] private GameObject dontClick;
    private bool nowEvent;

    private void Awake()
    {
        Instance = this;
    }

    public void StartTutorial()
    {
        garbageMove.Play();
        BackGround.RunAni();

        playerTransform.parent = transform.parent;

        StartCoroutine(Tutorial());

        IEnumerator Tutorial()
        {
            yield return new WaitForSeconds(1);

            //플레이어 이동 설명
            tutorial_playerMove.TalkOpen();
            nowEvent = true;
            yield return new WaitWhile(() => { return nowEvent; });

            //이동 설명 끝난후 잠시 이동
            yield return new WaitForSeconds(2);

            GameManager.GameStart();
            yield return new WaitWhile(() => { return GameManager.GetNowBattle() == false; });

            //플레이어 전투 설명
            dontClick.SetActive(true);
            tutorial_playerBattle.TalkOpen();
            yield return new WaitForSeconds(1);
            
            GameManager.SetRunCycle(false);
            nowEvent = true;
            yield return new WaitWhile(() => { return nowEvent; });
            dontClick.SetActive(false);

            yield return new WaitWhile(() => { return GameManager.GetNowBattle() == true; });

            //플레이어 아이템 획득 설명
            tutorial_playerItemGet.TalkOpen();
            yield return new WaitForSeconds(1);

            nowEvent = true;
            yield return new WaitWhile(() => { return nowEvent; });

            playerTransform.parent = playerExit.transform.GetChild(0);
            playerExit.Play();
            BackGround.StopAni();
        }
    }

    public static void EventEnd()
    {
        Instance.nowEvent = false;
    }

    public void EndTutorial()
    {
        //PlayData playData = DataManager.Instance.playData;
        //playData.firstPlay = false;
        //DataManager.SaveData();
        MoveScene.LoadScene("Main");
    }
}
