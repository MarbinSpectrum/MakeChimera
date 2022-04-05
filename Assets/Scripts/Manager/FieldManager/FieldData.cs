using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class FieldData : SerializedMonoBehaviour
{
    [Title("필드 이름")]
    public string fieldName;

    [Title("이동하는 씬이름")]
    public string moveSceneName;

    [Title("필드 풍경")]
    [OnInspectorGUI("DrawPreview", append: true)]
    public Texture2D fieldImg;

    [Title("등장 몬스터")]
    public List<string> spawnMonster = new List<string>();

    [TextArea]
    [Title("필드 설명")]
    public string fieldExplain;

    private Sprite m_fieldSprite;
    public Sprite fieldSprite
    {
        get
        {
            if (m_fieldSprite == null)
            {
                Texture2D texture = fieldImg;
                float w = 100f / (float)texture.width;
                float h = 100f / (float)texture.height;
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
                m_fieldSprite = sprite;
            }
            return m_fieldSprite;
        }
    }

    private void DrawPreview()
    {
        if (fieldImg == null) return;

        GUIStyle backViewStyle = new GUIStyle("label")
        {
            fontSize = 10,
            alignment = TextAnchor.MiddleCenter
        };

        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label(fieldImg, backViewStyle, GUILayout.Width(400), GUILayout.Height(200));
        GUILayout.EndVertical();
    }
}
