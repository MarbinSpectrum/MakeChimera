using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class FieldData : SerializedMonoBehaviour
{
    [Title("�ʵ� �̸�")]
    public string fieldName;

    [Title("�̵��ϴ� ���̸�")]
    public string moveSceneName;

    [Title("�ʵ� ǳ��")]
    [OnInspectorGUI("DrawPreview", append: true)]
    public Texture2D fieldImg;

    [Title("���� ����")]
    public List<string> spawnMonster = new List<string>();

    [TextArea]
    [Title("�ʵ� ����")]
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
