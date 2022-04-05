using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


public class DecomposeItemSortUI : SerializedMonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject body;
    [SerializeField] private Dictionary<SortBy, GameObject> selectList = new Dictionary<SortBy, GameObject>();
    private bool isOn = false;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �����츦 ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnOffWindow()
    {
        OnOffWindow(!isOn);
    }
    public void OnOffWindow(bool state)
    {
        isOn = state;
        if (isOn)
            animator.SetTrigger("ON");
        else
            animator.SetTrigger("OFF");
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ���ı������� �����ϴ� ��ư
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SortBtn(string sortBy)
    {
        SortBy sort = (SortBy)Enum.Parse(typeof(SortBy), sortBy);
        foreach (KeyValuePair<SortBy, GameObject> pair in selectList)
        {
            //���� ������ ���Ĺ�ư�� ����Ʈ ǥ��
            if(pair.Key == sort)
                pair.Value.SetActive(true);
            else
                pair.Value.SetActive(false);
        }

        //���ı������� ����
        DecomposeUI.SortItemList(sort);
    }
}
