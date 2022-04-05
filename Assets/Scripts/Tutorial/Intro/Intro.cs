using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Intro : MonoBehaviour
{
    [SerializeField] private PlayableDirector prologueTimeline;

    private void Awake()
    {
        prologueTimeline.stopped += PrologueEnd;
        prologueTimeline.Play();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���ѷα� ����
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    private static void PrologueEnd(PlayableDirector aDirector)
    {
        MoveScene.LoadScene("Tutorial");
    }
}
