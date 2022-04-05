using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftCompleteGauge : MonoBehaviour
{
    private float m_gauge = 0;
    [SerializeField] private Image bar;
    [SerializeField] private TextMeshProUGUI statText;
    public float gauge
    {
        set
        {
            m_gauge = value;
            if(bar != null)
            {
                bar.fillAmount = m_gauge;
            }
        }
        get
        {
            return m_gauge;
        }
    }

    public void SetText(string s)
    {
        statText.text = s;
    }
}
