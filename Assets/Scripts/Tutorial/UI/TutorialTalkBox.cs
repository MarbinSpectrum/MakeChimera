using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTalkBox : MonoBehaviour
{
    [SerializeField] private GameObject body;

    public void TalkOpen()
    {
        body.SetActive(true);
    }

    public void TalkClose()
    {
        Tutorial.EventEnd();
        Debug.Log(transform.name);
        body.SetActive(false);    
    }
}
