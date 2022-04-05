using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageUI : MonoBehaviour
{
    public static DamageUI Instance;
    [SerializeField] private DamageText monsterDamage;
    [SerializeField] private Canvas canvas;
    private List<DamageText> textList = new List<DamageText>();
    [SerializeField] private Vector2 offset;

    private void Awake()
    {
        Instance = this;
        canvas.worldCamera = Camera.main;
    }

    private DamageText GetText()
    {
        for (int i = 0; i < textList.Count; i++)
            if (textList[i].animation.isPlaying == false)
                return textList[i];
        return null;
    }

    public static void ShowDamage(int value, bool critical)
    {
        if (Instance == null)
            return;

        DamageText damage = Instance.GetText();

        //데미지를 표시할 위치를 정한다.
        Vector3 monsterPos = FieldSystem.Instance.nowMonster.transform.position +
            new Vector3(Instance.offset.x, Instance.offset.y, 0);

        //해당 위치에서 조금 랜덤한 위치에 나오도록 설정한다.
        Vector2 randomPos = Random.insideUnitCircle;
        monsterPos += new Vector3(randomPos.x, randomPos.y, 0);

        if (damage == null)
        {
            damage = Instantiate(Instance.monsterDamage);
            Instance.textList.Add(damage);
        }

        damage.transform.parent = Instance.transform;
        damage.transform.position = monsterPos;
        damage.transform.localScale = new Vector3(1, 1, 1);
        damage.gameObject.SetActive(true);
        damage.animation.Play();
        damage.text.text = DamageString(value,critical);
    }

    private static string NumberString(int value,bool critical)
    {
        if (critical)
            return $"<sprite=1{value}>";
        else
            return $"<sprite={value}>";
    }

    private static string DamageString(int value,bool critical)
    {
        string str = "";
        List<int> temp = new List<int>();
        while(value > 0)
        {
            temp.Add(value % 10);
            value /= 10;
        }
        if (critical)
            str += "<size=2.2>";
        else
            str += "<size=1.5>";
        for (int i = temp.Count - 1; i >= 0; i--)
            str += NumberString(temp[i], critical);
        str += "</size>";
        return str;
    }
}

