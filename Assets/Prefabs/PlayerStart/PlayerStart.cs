using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class PlayerStart : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private Transform playerTransform;

    private void Start()
    {
        playableDirector.stopped += TimeLineStop;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �÷��̾� ���� �ִϸ��̼��� ������ ó��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void TimeLineStop(PlayableDirector aDirector)
    {
        playerTransform.parent = transform.parent;
        BackGround.RunAni();
        GameManager.GameStart();

        gameObject.SetActive(false);
    }
}
