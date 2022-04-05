using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public enum CraftDropDownMenu
{
    CanCraft,
    CantCraft
}

public class CraftDropDown : SerializedMonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Dictionary<CraftDropDownMenu, string> groupByText = new Dictionary<CraftDropDownMenu, string>();
    [SerializeField] private Dictionary<CraftDropDownMenu, GameObject> optionSelects = new Dictionary<CraftDropDownMenu, GameObject>();
    [SerializeField] private TextMeshProUGUI text;

    private bool isOn = false;
    private CraftDropDownMenu nowOption;

    public void Init(CraftDropDownMenu option)
    {
        SelectOption(option);
        isOn = false;
        OnOffWindow(isOn);
    }

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
    /// : ��Ӵٿ� �޴� �ؽ�Ʈ ǥ��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetText()
    {
        SetText(nowOption);
    }
    public void SetText(CraftDropDownMenu groupBy)
    {
        text.text = groupByText[groupBy];
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : ��Ӵٹ� �޴� ���ý�
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SelectOption(string optionName)
    {
        CraftDropDownMenu option = (CraftDropDownMenu)Enum.Parse(typeof(CraftDropDownMenu), optionName);
        SelectOption(option);
    }
    public void SelectOption(CraftDropDownMenu option)
    {
        Craft.Instance.nowOption = option;
        Craft.SelectMenu();
        foreach (KeyValuePair<CraftDropDownMenu, GameObject> pair in optionSelects)
            pair.Value.SetActive(false);
        optionSelects[option].SetActive(true);
        SetText(option);
    }
}
