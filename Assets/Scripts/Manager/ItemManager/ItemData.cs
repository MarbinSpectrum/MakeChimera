using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum SortBy
{
    Name,
    AttackMax,
    AttackMin,
    CriticalPer,
    CriticalDamage,
}
public enum SortType
{
    ASC,
    DES
}

public class ItemData : SerializedMonoBehaviour, IComparable<ItemData>, IEquatable<ItemData>
{
    public string itemDataName;

    [Title("아이템 정보")]
    [LabelText("아이템 이름")]
    public string itemName;

    [LabelText("아이템 이미지")]
    [OnInspectorGUI("DrawPreview", append: true)]
    public Texture2D itemImg;
    private void DrawPreview()
    {
        if (itemImg == null)
            return;

        GUIStyle backViewStyle = new GUIStyle("label")
        {
            fontSize = 10,
            alignment = TextAnchor.MiddleCenter
        };

        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label(itemImg, backViewStyle, GUILayout.Width(200), GUILayout.Height(200));
        GUILayout.EndVertical();
    }

    [LabelText("아이템 설명")]
    [TextArea]
    public string itemExplain;

    [System.NonSerialized] public Vector2 size;
    private Sprite m_itemSprite;
    public Sprite itemSprite
    {
        get
        {
            if(m_itemSprite == null)
            {
                Texture2D texture = itemImg;
                float w = 100f / (float)texture.width;
                float h = 100f / (float)texture.height;
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
                m_itemSprite = sprite;
                size = new Vector3(w, h, 1);
            }
            return m_itemSprite;
        }
    }

    public int CompareTo(ItemData other)
    {
        if (other == null)
            return 1;

        else
            return this.itemName.CompareTo(other.itemName);
    }

    public bool Equals(ItemData other)
    {
        if (other == null)
            return false;
        ItemData objAsPart = other as ItemData;
        if (objAsPart == null)
            return false;
        else
            return itemName == other.itemName;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 아이템을 만들 수 있는지 검사
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static bool CanMake(List<RequireItemClass> requireItemList)
    {
        foreach (RequireItemClass require in requireItemList)
        {
            string item = require.item;
            int num = require.itemNum;
            if (Inventory.HasItem(item, num) == false)
                return false;
        }
        return true;
    }
}
