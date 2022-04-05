using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SpawnSlot : MonoBehaviour
{
    [SerializeField] Image monsterIcon;
    [SerializeField] TextMeshProUGUI monsterNumText;
    [SerializeField] Animator animator;

    private int MonsterNum;
    public int monsterNum
    {
        set
        {
            MonsterNum = value;
            SetNum(MonsterNum);
        }
        get
        {
            return MonsterNum;
        }
    }

    public void SetData(string monsterName,int monsterNum)
    {
        MonsterData monsterData = MonsterManager.GetMonster(monsterName);
        Sprite monsterIconSprite = monsterData.monsterIcon;

        monsterIcon.sprite = monsterIconSprite;
        this.monsterNum = monsterNum;
    }

    public void SetNum(int monsterNum)
    {
        monsterNumText.text = $" X {monsterNum}";
        if(monsterNum == 0)
        {
            animator.SetTrigger("Hide");
            Invoke("InvokeHide", 0.2f);
        }
    }

    public void InvokeHide()
    {
        gameObject.SetActive(false);
    }
}
