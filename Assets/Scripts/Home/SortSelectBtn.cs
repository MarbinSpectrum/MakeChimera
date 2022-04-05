using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class SortSelectBtn : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<SortBy, string> sortByText = new Dictionary<SortBy, string>();
    [SerializeField] private TextMeshProUGUI text;

    public void SetText(SortBy sortBy)
    {
        text.text = sortByText[sortBy];
    }
}
